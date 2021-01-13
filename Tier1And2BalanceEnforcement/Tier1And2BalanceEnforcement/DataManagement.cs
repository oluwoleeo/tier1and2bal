using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tier1And2BalanceEnforcement
{
    public class DataManagement
    {
        public static List<Migrated> GetMigratedAccounts(DapperContext dbc)
        {
            var con = dbc.Connection;
            List<Migrated> migratedAccounts = new List<Migrated>();
            StringBuilder query = new StringBuilder();
            query.Append("select rownum, a.foracid AccountNumber, b.acct_name AccountName, ");
            query.Append("a.sol_id BranchSOL, a.source_schm SourceScheme, ");
            query.Append("a.target_schm TargetScheme, ");
            query.Append("to_date(a.moved_time) MovedDate, a.clr_bal_amt AccountBalance, ");
            query.Append("(select distinct email from crmuser.phoneemail where orgkey = (select cif_id from tbaadm.gam where foracid=a.foracid) and email is not null and rownum = 1) Email, ");
            query.Append("(select distinct phoneno from crmuser.phoneemail where orgkey = (select cif_id from tbaadm.gam where foracid=a.foracid) and phoneno is not null and rownum=1) Phone ");
            query.Append("from custom.tiermig_h a left join tbaadm.gam b on a.foracid = b.foracid ");
            query.Append("where a.clr_bal_amt > 0");

            try
            {
                Log.WriteEvent("Connection opened in GetMigratedAccounts method");
                migratedAccounts = con.Query<Migrated>(query.ToString()).ToList();
                Log.WriteEvent(migratedAccounts?.Count + " rows retrieved by GetMigratedAccounts");
            }
            catch (Exception ex)
            {
                Log.WriteError($"Error in GetMigratedAccounts: {ex.Message}.\nStack trace: {ex.StackTrace}");
            }
            con.Close();
            Log.WriteEvent("Connection closed in GetMigratedAccounts method");
            return migratedAccounts;
        }

        public static PhoneAlertDetails GetCustomerAlertPhone(string accountNumber, DapperContext dbc)
        {
            PhoneAlertDetails ep = new PhoneAlertDetails();

            Log.WriteEvent("Looking for phone number in GetCustomerAlertPhone for " + accountNumber);

            var con = dbc.Connection;
            StringBuilder query = new StringBuilder();

            query.Append("Select AccountNo CustomerAccountNumber, Mobileno CustomerAlertPhone ");
            query.Append("FROM custom.alertsetup ");
            query.Append("WHERE ACCOUNTNO in :accNum and SMS_ALERT_FLG = 'Y'");

            try
            {
                ep = con.Query<PhoneAlertDetails>(query.ToString(), new { accNum = accountNumber }).FirstOrDefault();

                Log.WriteEvent("Connection opened in GetCustomerAlertPhone method");

                if (ep != null)
                {
                    Log.WriteEvent($"Phone number for {accountNumber} is {ep.CustomerAlertPhone} ");
                }
                else
                {
                    Log.WriteEvent($"Phone number for {accountNumber} is {ep} ");
                }
            }
            catch (Exception ex)
            {
                Log.WriteError($"Error in GetCustomerAlertPhone: {ex.Message}.\nStack trace: {ex.StackTrace}");
            }

            con.Close();
            Log.WriteEvent("Connection closed in GetCustomerAlertPhone method");

            return ep;
        }

        public static EmailAlertDetails GetCustomerAlertEmail(string accountNumber, DapperContext dbc)
        {
            EmailAlertDetails ep = new EmailAlertDetails();

            Log.WriteEvent("Looking for email address in GetCustomerAlertEmail for " + accountNumber);

            var con = dbc.Connection;
            StringBuilder query = new StringBuilder();

            query.Append("Select AccountNo CustomerAccountNumber, Email CustomerAlertEmail ");
            query.Append("FROM custom.alertsetup ");
            query.Append("WHERE ACCOUNTNO = :accNum and EMAIL_ALERT_FLG = 'Y'");

            try
            {
                ep = con.Query<EmailAlertDetails>(query.ToString(), new { accNum = accountNumber }).FirstOrDefault();

                Log.WriteEvent("Connection opened in GetCustomerAlertEmail method");

                if (ep != null)
                {
                    Log.WriteEvent($"E-mail for {accountNumber} is {ep.CustomerAlertEmail} ");
                }
                else
                {
                    Log.WriteEvent($"E-mail for {accountNumber} is {ep} ");
                }

            }
            catch (Exception ex)
            {
                Log.WriteError($"Error in GetCustomerAlertEmail: {ex.Message}\nStack trace: {ex.StackTrace}");
            }

            con.Close();
            Log.WriteEvent("Connection closed in GetCustomerAlertEmail method");

            return ep;
        }

        public static List<FailedToMigrate> GetNonMigratedAccounts(DapperContext dbc)
        {
            var con = dbc.Connection;
            List<FailedToMigrate> fAccounts = new List<FailedToMigrate>(); ;
            StringBuilder query = new StringBuilder();
            query.Append("select rownum, a.foracid AccountNumber, b.acct_name AccountName, ");
            query.Append("a.sol_id BranchSOL, a.source_schm SourceScheme, ");
            query.Append("a.target_schm TargetScheme, a.clr_bal_amt AccountBalance, ");
            query.Append("a.comments Reason ");
            query.Append("from custom.tiermig a left join tbaadm.gam b on a.foracid = b.foracid where a.moved_flg='F'");
            //query.Append("from custom.tiermig a left join tbaadm.gam b on a.foracid = b.foracid");

            try
            {
                Log.WriteEvent("Connection opened in GetNonMigratedAccounts method");

                fAccounts = con.Query<FailedToMigrate>(query.ToString()).ToList();
                Log.WriteEvent(fAccounts?.Count + " rows retrieved by GetNonMigratedAccounts");
            }
            catch (Exception ex)
            {
                Log.WriteError($"Error in GetNonMigratedAccounts: {ex.Message}\nStack trace: {ex.StackTrace}");
            }

            con.Close();
            Log.WriteEvent("Connection closed in GetNonMigratedAccounts method");

            return fAccounts;
        }

        public static List<ToBeNotified> GetNotExceededAccounts(DapperContext dbc)
        {
            var con = dbc.Connection;
            List<ToBeNotified> notExceeded = new List<ToBeNotified>();
            StringBuilder query = new StringBuilder();

            query.Append("select a.foracid AccountNumber, b.acct_name AccountName, ");
            query.Append("a.sol_id BranchSOL, a.schm_code SchemeCode, ");
            query.Append("a.clr_bal_amt AccountBalance, a.balance_percent_threshold Percentage, ");
            query.Append("(select distinct email from crmuser.phoneemail where orgkey = (select cif_id from tbaadm.gam where foracid=a.foracid) and email is not null and rownum = 1) CustomerEmail ");
            query.Append("from custom.tiermig_msg a left join tbaadm.gam b on a.foracid = b.foracid ");
            query.Append("where a.balance_percent_threshold <= 100 and a.email_flg = 'N'");

            try
            {
                Log.WriteEvent("Connection opened in GetNotExceededAccounts method");
                notExceeded = con.Query<ToBeNotified>(query.ToString()).ToList();
                Log.WriteEvent(notExceeded?.Count + " rows retrieved by GetNotExceededAccounts");

                /*var sql = query.ToString();
                var aa = con.Query<List<ToBeNotified>>(sql).ToList();

                var sqlcomm = new OracleCommand();
                sqlcomm.CommandText = sql;
                sqlcomm.CommandType = CommandType.Text;
                sqlcomm.Connection = new OracleConnection(ConfigurationManager.ConnectionStrings["FbnMockDBConString"].ConnectionString);
                if (sqlcomm.Connection.State != ConnectionState.Open)
                    sqlcomm.Connection.Open();
                var reader = sqlcomm.ExecuteReader();
                var mylist = new List<ToBeNotified>();
                while (reader.Read())
                {
                    var myobj = new ToBeNotified
                    {
                        AccountBalance = Convert.ToString(reader["ACCOUNTBALANCE"])                     

                    };
                    mylist.Add(myobj);
                }
                Log.WriteEvent(aa.Count + " rows retrieved by GetNotExceededAccounts");
                */
            }
            catch (Exception ex)
            {
                Log.WriteError($"Error in GetNotExceededAccounts: {ex.Message}\nStack trace: {ex.StackTrace}");
            }

            con.Close();
            Log.WriteEvent("Connection closed in GetNotExceededAccounts method");
            return notExceeded;
        }

        public static List<ToBeNotified> GetExceededAccounts(DapperContext dbc)
        {
            var con = dbc.Connection;
            List<ToBeNotified> exceeded = new List<ToBeNotified>();
            StringBuilder query = new StringBuilder();

            query.Append("select a.foracid AccountNumber, (select acct_name from tbaadm.gam where foracid=a.foracid) AccountName, ");
            query.Append("a.sol_id BranchSOL, a.schm_code SchemeCode, ");
            query.Append("a.clr_bal_amt AccountBalance, a.balance_percent_threshold Percentage, ");
            query.Append("(select distinct email from crmuser.phoneemail where orgkey = (select cif_id from tbaadm.gam where foracid=a.foracid) and email is not null and rownum = 1) CustomerEmail, ");
            query.Append("(select distinct phoneno from crmuser.phoneemail where orgkey = (select cif_id from tbaadm.gam where foracid=a.foracid) and phoneno is not null and rownum=1) CustomerPhone ");
            query.Append("from custom.tiermig_msg a ");
            query.Append("where a.balance_percent_threshold > 100 and a.email_flg = 'N'");

            try
            {
                Log.WriteEvent("Connection opened in GetExceededAccounts method");
                exceeded = con.Query<ToBeNotified>(query.ToString()).ToList();
                Log.WriteEvent(exceeded?.Count + " rows retrieved by GetExceededAccounts");
            }
            catch (Exception ex)
            {
                Log.WriteError($"Error in GetExceededAccounts: {ex.Message}\nStack trace: {ex.StackTrace}");
            }

            con.Close();
            Log.WriteEvent("Connection closed in GetExceededAccounts method");
            return exceeded;
        }

        public static List<EmailAlertDetails> GetCustomersAlertEmail(List<string> accountNumbers, DapperContext dbc)
        {
            List<EmailAlertDetails> ep = new List<EmailAlertDetails>();
            int len = accountNumbers.Count;

            Log.WriteEvent(len + " accounts passed to GetCustomersAlertEmail");

            if (len > 0)
            {
                var con = dbc.Connection;
                StringBuilder query = new StringBuilder();

                query.Append("Select AccountNo CustomerAccountNumber, Email CustomerAlertEmail ");
                query.Append("FROM custom.alertsetup ");
                query.Append("WHERE ACCOUNTNO in :accNums and EMAIL_ALERT_FLG = 'Y'");

                if (len > 1000)
                {
                    List<string> accountNumbers1 = accountNumbers.Take(1000).ToList();
                    List<string> accountNumbers2 = accountNumbers.Skip(1000).Take(1000).ToList();

                    try
                    {
                        Log.WriteEvent("Connection opened in GetCustomersAlertEmail method");
                        List<EmailAlertDetails> ep1 = con.Query<EmailAlertDetails>(query.ToString(), new { accNums = accountNumbers1 }).ToList();
                        Log.WriteEvent(ep1?.Count + " e-mail rows retrieved by GetCustomersAlertEmail (batch 1)");
                        ep.AddRange(ep1);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError($"Error in GetCustomersAlertEmail (batch 1): {ex.Message}\nStack trace: {ex.StackTrace}");
                    }

                    try
                    {
                        Log.WriteEvent("Connection opened in GetCustomersAlertEmail method");
                        List<EmailAlertDetails> ep2 = con.Query<EmailAlertDetails>(query.ToString(), new { accNums = accountNumbers2 }).ToList();
                        Log.WriteEvent(ep2?.Count + " e-mail rows retrieved by GetCustomersAlertEmail (batch 2)");
                        ep.AddRange(ep2);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError($"Error in GetCustomersAlertEmail (batch 2): {ex.Message}\nStack trace: {ex.StackTrace}");
                    }
                }
                else if (len <= 1000)
                {
                    try
                    {
                        Log.WriteEvent("Connection opened in GetCustomersAlertEmail method");
                        ep = con.Query<EmailAlertDetails>(query.ToString(), new { accNums = accountNumbers }).ToList();
                        Log.WriteEvent(ep?.Count + " e-mail rows retrieved by GetCustomersAlertEmail.");
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError($"Error in GetCustomersAlertEmail: {ex.Message}\nStack trace: {ex.StackTrace}");
                    }
                }

                con.Close();
                Log.WriteEvent("Connection closed in GetCustomersAlertEmail method");
            }

            return ep;
        }

        public static List<PhoneAlertDetails> GetCustomersAlertPhone(List<string> accountNumbers, DapperContext dbc)
        {
            List<PhoneAlertDetails> ep = new List<PhoneAlertDetails>();
            int len = accountNumbers.Count;

            Log.WriteEvent(len + " accounts passed to GetCustomersAlertPhone");

            if (len > 0)
            {
                var con = dbc.Connection;

                StringBuilder query = new StringBuilder();

                query.Append("Select AccountNo CustomerAccountNumber, Mobileno CustomerAlertPhone ");
                query.Append("FROM custom.alertsetup ");
                query.Append("WHERE ACCOUNTNO in :accNums and SMS_ALERT_FLG = 'Y'");

                if (len > 1000)
                {
                    List<string> accountNumbers1 = accountNumbers.Take(1000).ToList();
                    List<string> accountNumbers2 = accountNumbers.Skip(1000).Take(1000).ToList();

                    try
                    {
                        Log.WriteEvent("Connection opened in GetCustomersAlertPhone method");
                        List<PhoneAlertDetails> ep1 = con.Query<PhoneAlertDetails>(query.ToString(), new { accNums = accountNumbers }).ToList();
                        Log.WriteEvent(ep1?.Count + " phone number rows retrieved by GetCustomersAlertPhone (batch 1)");
                        ep.AddRange(ep1);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError($"Error in GetCustomersAlertPhone (batch 1): {ex.Message}\nStack trace: {ex.StackTrace}");
                    }

                    try
                    {
                        Log.WriteEvent("Connection opened in GetCustomersAlertPhone method");
                        List<PhoneAlertDetails> ep2 = con.Query<PhoneAlertDetails>(query.ToString(), new { accNums = accountNumbers }).ToList();
                        Log.WriteEvent(ep2?.Count + " phone number rows retrieved by GetCustomersAlertPhone (batch 2)");
                        ep.AddRange(ep2);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError($"Error in GetCustomersAlertPhone (batch 2): {ex.Message}\nStack trace: {ex.StackTrace}");
                    }
                }
                else if (len <= 1000)
                {
                    try
                    {
                        Log.WriteEvent("Connection opened in GetCustomersAlertPhone method");
                        ep = con.Query<PhoneAlertDetails>(query.ToString(), new { accNums = accountNumbers }).ToList();
                        Log.WriteEvent(ep?.Count + " phone number rows retrieved by GetCustomersAlertPhone.");
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError($"Error in GetCustomersAlertPhone: {ex.Message}\nStack trace: {ex.StackTrace}");
                    }
                }

                con.Close();
                Log.WriteEvent("Connection closed in GetCustomersAlertPhone method");
            }

            return ep;
        }

        public static int UpdateRecords(string flag,string account, DapperContext dbc)
        {
            int rows = -1;

            if (flag=="E")
            {
                var con = dbc.Connection;
                StringBuilder query = new StringBuilder();
                DateTime present = DateTime.Now;

                query.Append("Update custom.tiermig_msg ");
                query.Append("set email_flg='Y', email_sent_time = :sentTime ");
                query.Append("where foracid= :accNum and email_flg='N' ");

                try
                {
                    Log.WriteEvent("Connection opened in UpdateRecords for email info update");
                    rows = con.Execute(query.ToString(), new { accNum = account,sentTime = present });
                }
                catch (Exception ex)
                {
                    Log.WriteError($"Error in UpdateRecords for email info update: {ex.Message}");
                }

                con.Close();
                Log.WriteEvent("Connection closed in UpdateRecords method for email info update");
            }
            else if(flag=="S")
            {
                var con = dbc.Connection;
                StringBuilder query = new StringBuilder();
                DateTime present = DateTime.Now;

                query.Append("Update custom.tiermig_msg ");
                query.Append("set sms_flg='Y', sms_sent_time = :sentTime ");
                query.Append("where foracid= :accNum and sms_flg='N' ");

                try
                {
                    Log.WriteEvent("Connection opened in UpdateRecords for SMS info update");
                    rows = con.Execute(query.ToString(), new { accNum = account, sentTime = present });
                }
                catch (Exception ex)
                {
                    Log.WriteError($"Error in UpdateRecords for SMS info update: {ex.Message}");
                }

                con.Close();
                Log.WriteEvent("Connection closed in UpdateRecords method for SMS info update");
            }
            return rows;
        }
    }
}
