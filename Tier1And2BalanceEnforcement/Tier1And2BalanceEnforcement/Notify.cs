using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration;
using System.IO;

namespace Tier1And2BalanceEnforcement
{
    public class Notify
    {
        static string svrInternal = ConfigurationManager.AppSettings["SMTPsvrInternal"];
        static string svrExternal = ConfigurationManager.AppSettings["SMTPsvrExternal"];
        static int port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
        static string pwd = ConfigurationManager.AppSettings["SMTPpwd"];
        static string usr = ConfigurationManager.AppSettings["SMTPusr"];

        static List<ToBeNotified> excdAccs1 = new List<ToBeNotified>(); //to be used by SendSMS

        public static void InitProcess()
        {
            DapperContext db = new DapperContext();
            Log.WriteEvent("Begin Process Start");

            try
            {
                List<ToBeNotified> combined = new List<ToBeNotified>();
                List<Migrated> migAccs = DataManagement.GetMigratedAccounts(db); // accounts successfully migrated
                List<FailedToMigrate> fAccs = DataManagement.GetNonMigratedAccounts(db); // accounts failing migration
                List<ToBeNotified> notExcdAccs = DataManagement.GetNotExceededAccounts(db);  // accounts whose balance is < 100
                List<ToBeNotified> excdAccs = DataManagement.GetExceededAccounts(db);  

                int neaEmailsFromFinacle = (from nea in notExcdAccs where !string.IsNullOrWhiteSpace(nea.CustomerEmail) select nea).Count();
                Log.WriteEvent($"{neaEmailsFromFinacle} emails retrieved by GetNotExceededAccounts");


                int eaEmailsFromFinacle = (from ea in excdAccs where !string.IsNullOrWhiteSpace(ea.CustomerEmail) select ea).Count();
                int eaPhonesFromFinacle = (from ea in excdAccs where !string.IsNullOrWhiteSpace(ea.CustomerPhone) select ea).Count();
                Log.WriteEvent($"{eaEmailsFromFinacle} emails retrieved by GetExceededAccounts");
                Log.WriteEvent($"{eaPhonesFromFinacle} phones retrieved by GetExceededAccounts");

                List<ToBeNotified> emailNull = new List<ToBeNotified>();  // list of not exceeded details whose email were not retrieved from crmuser.phoneemail
                List<ToBeNotified> emailNull1 = new List<ToBeNotified>(); // list of exceeded details whose email were not retrieved from crmuser.phoneemail
                List<ToBeNotified> phoneNull = new List<ToBeNotified>();
                int i = 1;
                int j = 1;

                foreach (ToBeNotified nea in notExcdAccs)
                {
                    nea.RedactedAccountNumber = Utility.RedactString(nea.AccountNumber);

                    if (nea.CustomerEmail == null)
                    {
                        emailNull.Add(nea);
                    }
                }

                var accNums = (from d in emailNull select d.AccountNumber).ToList();
                List<EmailAlertDetails> emails = DataManagement.GetCustomersAlertEmail(accNums, db);
                foreach (ToBeNotified nea in emailNull)  // not exceeded emails retrieved from custom.alertsetup
                {
                    string email = (from em in emails where nea.AccountNumber == em.CustomerAccountNumber select em.CustomerAlertEmail).FirstOrDefault();

                    nea.CustomerEmail = email;
                }

                int neaEmailsFromAlert = (from nea in emailNull where !string.IsNullOrWhiteSpace(nea.CustomerEmail) select nea).Count();
                Log.WriteEvent($"{neaEmailsFromAlert} not exceeded emails retrieved by GetCustomersAlertEmail");

                foreach (ToBeNotified ea in excdAccs)
                {
                    ea.RedactedAccountNumber = Utility.RedactString(ea.AccountNumber);

                    if (!string.IsNullOrWhiteSpace(ea.CustomerPhone))
                    {
                        ea.CustomerPhone = Utility.ProcessPhone(ea.CustomerPhone); // process phone number
                    }

                    if (ea.CustomerEmail == null)
                    {
                        emailNull1.Add(ea);
                    }

                    if (ea.CustomerPhone == null)
                    {
                        phoneNull.Add(ea);
                    }
                }

                var accNumsE = (from d in emailNull1 select d.AccountNumber).ToList();
                var accNumsP = (from d in phoneNull select d.AccountNumber).ToList();
                List<EmailAlertDetails> emails1 = DataManagement.GetCustomersAlertEmail(accNumsE, db);
                List<PhoneAlertDetails> phones = DataManagement.GetCustomersAlertPhone(accNumsP, db);
                foreach (ToBeNotified ea in emailNull1)  // exceeded emails retrieved from custom.alertsetup
                {
                    string email = (from em in emails1 where ea.AccountNumber == em.CustomerAccountNumber select em.CustomerAlertEmail).FirstOrDefault();

                    ea.CustomerEmail = email;
                }

                foreach (ToBeNotified ea in phoneNull) // exceeded phones retrieved from custom.alertsetup
                {
                    string phone = (from ph in phones where ea.AccountNumber == ph.CustomerAccountNumber select ph.CustomerAlertPhone).FirstOrDefault();

                    ea.CustomerPhone = phone;
                }

                int eaEmailsFromAlert = (from ea in emailNull1 where !string.IsNullOrWhiteSpace(ea.CustomerEmail) select ea).Count();
                Log.WriteEvent($"{eaEmailsFromAlert} exceeded emails retrieved by GetCustomersAlertEmail");

                int eaPhonesFromAlert = (from ea in phoneNull where !string.IsNullOrWhiteSpace(ea.CustomerPhone) select ea).Count();
                Log.WriteEvent($"{eaPhonesFromAlert} exceeded phone numbers retrieved by GetCustomersAlertPhone");

                excdAccs1.Clear();  //to avoid records duplication
                excdAccs1.AddRange(excdAccs);

                combined.AddRange(notExcdAccs);
                combined.AddRange(excdAccs);

                foreach (var acc in combined)
                {
                    if (Convert.ToDouble(acc.Percentage) <= Convert.ToDouble("100"))
                        Log.DetailsLog(i++, "NEA", acc);
                    else
                        Log.DetailsLog(j++, "EA", acc);
                }

                SendCustomerMail(combined, "initialMail");
                SendReportMail(migAccs,fAccs);
            }
            catch (Exception ex)
            {
                Log.WriteError($"Error in Notify.InitProcess: {ex.Message} --- {ex.StackTrace}");
            }
            finally
            {
                if (db != null)
                {
                    db.Dispose();
                }

                Log.WriteEvent("Begin Process End.");
            }
        }

        private static MailMessage NotifyCustomerEmail(ToBeNotified customer, bool migratedFlag)
        {
            MailMessage Mail = new MailMessage();

            try
            {
                if (!migratedFlag)
                {
                    Mail.From = new MailAddress("firstbank@firstbanknigeria.com");
                    Mail.Subject = "NOTIFICATION ON YOUR ACCOUNT BALANCE THRESHOLD";
                    Mail.IsBodyHtml = true;
                    StringBuilder emailcontent = new StringBuilder();
                    emailcontent.Append("<head><style type=text/css> H1 {font-size:x-small;font-weight:normal;font-family:Arial;color:#21297B;} H3 {font-size:x-small;font-weight:bold;font-family:Arial;color:#FFF;} H2 {font-size:x-small;font-family:Arial;font-weight:normal;color:#FFF;} H6{margin-top:0;color:#000;}</style></head>");
                    emailcontent.Append("<h1><font size='2' face='arial' _moz-rs-heading=''>Dear<b> " + customer.AccountName + "</b>,<br/>");

                    /*emailcontent.Append($"<br/><br/>Please be informed that the account balance of your account {customer.RedactedAccountNumber} with us is currently {customer.Percentage}% of your maximum allowable account balance<br/><br/>");
                    emailcontent.Append("<br/>Kindly note that your account will be frozen if the account balance exceeds 100% of the threshold.<br/>");*/

                    if (Convert.ToDouble(customer.Percentage)
                        <= Convert.ToDouble("100"))
                    {
                        emailcontent.Append($"<br/>Thank you for choosing us as your preferred financial solutions provider on your life’s journey. We noticed your account balance is close to the cumulative balance threshold for your account type.<br/>");
                    }
                    else
                    {
                        emailcontent.Append($"<br/>Thank you for choosing us as your preferred financial solutions provider on your life’s journey. We noticed you have exceeded the transaction threshold for your account type.<br/>");
                    }

                    emailcontent.Append($"<br/>We would love to remain your preferred partner on your life’s journey and the first step to this partnership is to upgrade your {(customer.SchemeCode == "SA321" ? "First Instant Account" : customer.SchemeCode == "SA310" ? "First Instant Plus Account" : "account")} to our Standard Savings Account, which offers you unrestricted access to our service offerings.<br/>");

                    /*else
                    {
                        emailcontent.Append($"<br/>You have reached the cumulative balance threshold for your account type. Kindly visit the nearest FirstBank branch to upgrade your account.<br/>");
                    }*/

                    /*emailcontent.Append("<br/>We remain committed to putting you first by providing you with excellent service through our alternate channels, extensive branch and ATM networks.<br/>");
                    emailcontent.Append("<br/>Always remember for security of your funds, do not respond to any mail/call asking you to click on links to update your banking details. These are fraudulent and are not initiated by FirstBank.<br/>");
                    emailcontent.Append("<br/>For further enquiries/details, please call 0700FIRSTCONTACT (0700-34778-2668228), 01-4485500, 0708-062-5000, SMS Short Code 30012 or email:&nbsp; <a href='mailto:firstContact@firstbanknigeria.com'>FirstContact@firstbanknigeria.com</a><br/>");*/
                    emailcontent.Append("<br/>This upgrade will be seamless. In order to complete the migration, kindly visit any of our branches with the following documents:<br/>");
                    emailcontent.Append("-  Acceptable means of identification, e.g. National Driver’s License, National ID Card, International Passport or National Voter’s Card.<br/>");
                    emailcontent.Append("-  Utility Bill, e.g. PHCN Bill, Phone Bill, State Disposal, Water Bill etc.<br/>");
                    emailcontent.Append("<br/>Please ensure you submit these documents within one (1) week from receiving this notification to avoid disruptions on your account.<br/>");
                    emailcontent.Append("<br/>Thank you for choosing FirstBank<br/>");
                    emailcontent.Append("<p>  </p>");
                    string emailMsg = emailcontent.ToString();

                    string _AppStartUpPath = AppDomain.CurrentDomain.BaseDirectory;

                    AlternateView View = default(AlternateView);
                    LinkedResource resource = default(LinkedResource);
                    LinkedResource resource2 = default(LinkedResource);

                    string imagepath = " <img src=cid:Image1 /> ";
                    string imagepath2 = " <img src=cid:Image2 /> ";

                    string body2 = imagepath + emailMsg + imagepath2;
                    View = AlternateView.CreateAlternateViewFromString(body2.ToString(), null, "text/html");

                    //link the resource to embed
                    resource = new LinkedResource(_AppStartUpPath + "fbn_logo.jpg");
                    resource2 = new LinkedResource(_AppStartUpPath + "ScamAlert.jpg");

                    //name the resource
                    resource.ContentId = "Image1";
                    resource2.ContentId = "Image2";

                    //add the resource to the alternate view
                    View.LinkedResources.Add(resource);
                    View.LinkedResources.Add(resource2);

                    Mail.Body = emailMsg;
                    // Comment out at GoLive
                    // customer.CustomerEmail = "SN027745@firstbanknigeria.com";

                    Mail.AlternateViews.Add(View);
                    Mail.To.Add(new MailAddress(customer.CustomerEmail));
                    Mail.ReplyToList.Add("firstcontact@firstbanknigeria.com");
                }
            }
            catch (Exception ex)
            {
                Log.WriteError($"{ex.Message} Email to {customer.CustomerEmail} not built");
            }

            return Mail;
        }

        private static void BypassCertificateError()
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

            ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

        }

        private static bool SendCustomerMail(List<ToBeNotified> allAccounts,string typeOfMail)
        {
            string reportDate = DateTime.Today.ToString("dd/MM/yyyy");

            SmtpClient SMTPSVR = new SmtpClient(svrExternal, port)
            {
                EnableSsl = false,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(usr, pwd),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };     

            StringBuilder MailList = new StringBuilder();

            MailMessage customerEmail = new MailMessage();

            if (allAccounts.Count > 0 && typeOfMail== "initialMail")
            {
                Log.EmailNotificationStatusLog("E-mails for accounts 75% and above send process for " + reportDate + " commence.");
                
                DapperContext db = new DapperContext();

                try
                {
                    foreach (var customer in allAccounts)
                    {
                        try
                        {
                            if (string.IsNullOrWhiteSpace(customer.CustomerEmail))
                            {
                                MailList.Clear();
                                MailList.Append($"Customer Acc No: {customer.AccountNumber}, ");
                                MailList.Append($"Customer Name: {customer.AccountName} ");

                                Log.EmailNotificationStatusLog("Email not sent as customer has no email on Finacle and Alert Platform. Details below:\r\n " + MailList.ToString() + "\n\n");
                                continue;
                            }

                            customerEmail = NotifyCustomerEmail(customer, false);

                            if (customerEmail != null && customerEmail.To.Count > 0)
                            {
                                MailList.Clear();
                                MailList.Append($"Customer Acc No: {customer.AccountNumber}, ");
                                MailList.Append($"Customer Name: {customer.AccountName}, ");
                                MailList.Append($"Customer Email: {customer.CustomerEmail} ");

                                BypassCertificateError();

                                SMTPSVR.Send(customerEmail);
                                int i = DataManagement.UpdateRecords("E", customer.AccountNumber, db);

                                if (i > 0)
                                {
                                    Log.WriteEvent($"{i} row's email record updated");
                                }
                                else
                                {
                                    Log.WriteEvent("No email record updated");
                                }

                                Log.EmailNotificationStatusLog("Email successfully sent to customer with details below:\r\n " + MailList.ToString() + "\n\n");
                            }

                        }
                        catch (Exception ex)
                        {
                            Log.WriteError($"Error while sending customer e-mail: {ex.Message} --- {ex.StackTrace}");
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteError($"Error while sending customer e-mail: {ex.Message} --- {ex.StackTrace}");
                }
                finally
                {
                    if (customerEmail != null)
                        customerEmail.Dispose();
                    if (SMTPSVR != null)
                        SMTPSVR.Dispose();
                }

                Log.EmailNotificationStatusLog("E-mails for accounts 75% and above send process for " + reportDate + " completed.");
            }
            /*else if (allAccounts.Count > 0 && typeOfMail=="exceed100")
            {
                Log.EmailNotificationStatusLog("Migrated accounts e-mails for " + reportDate + " send commence.");

                try
                {
                    foreach (var customer in allAccounts)
                    {
                        try
                        {
                            if (string.IsNullOrWhiteSpace(customer.CustomerEmail))
                            {
                                MailList.Clear();
                                MailList.Append($"Customer Acc No: {customer.AccountNumber}, ");
                                MailList.Append($"Customer Name: {customer.AccountName} ");

                                Log.EmailNotificationStatusLog("E-mail not sent as migrated customer has no email on Finacle and Alert Platform. Details below:\r\n " + MailList.ToString() + "\n\n");
                                continue;
                            }

                            customerEmail = NotifyCustomerEmail(customer, true);

                            SMTPSVR.DeliveryMethod = SmtpDeliveryMethod.Network;

                            if (customerEmail != null && customerEmail.To.Count > 0)
                            {
                                MailList.Clear();
                                MailList.Append($"Customer Acc No: {customer.AccountNumber}, ");
                                MailList.Append($"Customer Name: {customer.AccountName}, ");
                                MailList.Append($"Customer Email: {customer.CustomerEmail} ");

                                BypassCertificateError();

                                SMTPSVR.Send(customerEmail);

                                Log.EmailNotificationStatusLog("Email successfully sent to migrated customer with details below:\r\n " + MailList.ToString() + "\n\n");
                            }

                        }
                        catch (Exception ex)
                        {
                            Log.WriteError($"Error while sending migrated customer e-mail: {ex.Message} ---- {ex.StackTrace}");
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteError(ex.Message);
                }
                finally
                {
                    if (customerEmail != null)
                        customerEmail.Dispose();
                    if (SMTPSVR != null)
                        SMTPSVR.Dispose();
                }

                Log.EmailNotificationStatusLog("Migrated accounts e-mail send process for " + reportDate + " completed.");
            }*/
            else
            {
                Log.EmailNotificationStatusLog("No e-mail sent for " + reportDate);
            }

            return true;
        }

        private static bool SendSMS(List<Migrated> migratedAccounts)
        {
            string reportDate = DateTime.Today.ToString("dd/MM/yyyy");

            if (migratedAccounts.Count > 0)
            {
                Log.SMSNotificationStatusLog("SMS for " + reportDate + " send commence.");

                StringBuilder SMSList = new StringBuilder();
                DapperContext db = new DapperContext();

                try
                {
                    foreach (Migrated ea in migratedAccounts)
                    {
                        if (string.IsNullOrWhiteSpace(ea.Phone))
                        {
                            SMSList.Clear();
                            SMSList.Append($"Customer Acc No: {ea.AccountNumber}, ");
                            SMSList.Append($"Customer Name: {ea.AccountName} ");

                            Log.SMSNotificationStatusLog("SMS not sent as customer has no phone on Finacle and Alert Platform. Details below:\r\n " + SMSList.ToString());
                            continue;
                        }

                        SMSList.Clear();
                        SMSList.Append($"Customer Acc No: {ea.AccountNumber}, ");
                        SMSList.Append($"Customer Name: {ea.AccountName}, ");
                        SMSList.Append($"Customer phone: {ea.Phone} ");

                        FbnAlert.ServiceClient smsClient = new FbnAlert.ServiceClient();
                        FbnAlert.SMSMessage smsMsg = new FbnAlert.SMSMessage()
                        {
                            AccountNumber = ea.AccountNumber,
                            AppCode = ConfigurationManager.AppSettings.Get("smsappcode"),
                            Message = GetSMS(ea),
                            MobileNo = ea.Phone
                        };

                        FbnAlert.Output output = smsClient.SendMessage(smsMsg);

                        if (output.Status)
                        {
                            Log.SMSNotificationStatusLog($"SMS sent to customer with details below:\r\n {SMSList.ToString()}\n\n");

                            int i = DataManagement.UpdateRecords("S", ea.AccountNumber, db);

                            if (i > 0)
                            {
                                Log.WriteEvent($"{i} row's SMS record updated");
                            }
                            else
                            {
                                Log.WriteEvent("No record updated");
                            }
                        }
                        else
                        {
                            Log.SMSNotificationStatusLog($"SMS not sent to customer with details below. Reason: {output.Reason} \r\n {SMSList.ToString()}\n\n");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteError($"Error while sending SMS: {ex.Message} --- {ex.StackTrace}");
                }

                Log.SMSNotificationStatusLog("SMS send process for " + reportDate + " completed.");
            }
            else
                Log.SMSNotificationStatusLog("No SMS sent for " + reportDate);

            return true;
        }

        private static string GetSMS(Migrated acc)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("You have reached the cumulative balance threshold for your account type. Kindly visit the nearest FirstBank branch to upgrade your account. Thank you. You First");
            /*msg.Append("Dear " + acc.AccountName + ", ");
            msg.Append($"your account {acc.RedactedAccountNumber} is frozen as it has exceeded the maximum allowable account balance.");
            msg.Append("To enable access to your account, kindly visit any FirstBank branch with a valid means of Identification and a proof of your address e.g. recent utility bill etc. to upgrade your account to a Savings Account. ");
            msg.Append("For further enquiries/details, please call 0700FIRSTCONTACT (0700-34778-2668228), 01-4485500, 0708-062-5000");*/

            return msg.ToString();
        }

        private static bool SendReportMail(List<Migrated> migAccounts, List<FailedToMigrate> fAccounts)
        {
            string lFilePath = ConfigurationManager.AppSettings["lastpickedfile"];

            if (fAccounts.Count > 0) //send report of accounts that could not be migrated
            {
                Report rpt = new Report('f');
                bool status = rpt.GenerateFailedReport(fAccounts);

                if (status)
                {
                    SmtpClient SMTPSVR = new SmtpClient(svrInternal, port);

                    SMTPSVR.EnableSsl = false;
                    SMTPSVR.UseDefaultCredentials = false;
                    SMTPSVR.Credentials = new NetworkCredential(usr, pwd);

                    SMTPSVR.DeliveryMethod = SmtpDeliveryMethod.Network;

                    try
                    {
                        BypassCertificateError();
                        SMTPSVR.Send(NotifyReport('f')); // send to BSC and Compliance
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError($"Error while sending unmigrated accounts report e-mail: {ex.Message}");
                    }
                    finally
                    {
                        if (SMTPSVR != null)
                            SMTPSVR.Dispose();
                    }
                }
                else
                {
                    Log.ReportLog("An error occured while generating the report for unmigrated accounts");
                }
            }

            if (migAccounts.Count > 0) //process and send report for successfully migrated accounts
            {
                DateTime present = DateTime.Now;
                int total = migAccounts.Count;
                Log.ReportLog($"Migrated accounts: {total}");

                Report rpt = new Report('s');

                int lastPickedRow = Convert.ToInt32(File.ReadAllText(lFilePath).Trim());
                int diff = total - lastPickedRow;

                if (diff > 0)
                {
                    Log.ReportLog($"Last picked row is {lastPickedRow}");
                    Log.ReportLog($"Expected number of rows that should be written is {diff}");

                    List<Migrated> currentMigrated = new List<Migrated>();
                    List<Migrated> currentMigratedEmailSMS = new List<Migrated>();

                    currentMigrated.AddRange(migAccounts.Skip(lastPickedRow));

                    /*foreach (Migrated mmm in currentMigrated)
                    {
                        ToBeNotified cust = (from c in excdAccs1 where c.AccountNumber == mmm.AccountNumber select c).FirstOrDefault();

                        if (cust!=null)
                        {
                            currentMigratedEmailSMS.Add(cust);
                        }
                    }*/

                    foreach (Migrated mmm in currentMigrated)
                    {
                        if (string.IsNullOrWhiteSpace(mmm.Email))
                        { 
                            mmm.Email = DataManagement.GetCustomerAlertEmail(mmm.AccountNumber, new DapperContext())?.CustomerAlertEmail;
                        }

                        if (string.IsNullOrWhiteSpace(mmm.Phone))
                        {
                            string phone = DataManagement.GetCustomerAlertPhone(mmm.AccountNumber, new DapperContext())?.CustomerAlertPhone;

                            if (!string.IsNullOrWhiteSpace(phone))
                            {
                                mmm.Phone = Utility.ProcessPhone(phone);
                            }
                            else
                            {
                                mmm.Phone = null;
                            }
                            
                        }
                        else
                        {
                            mmm.Phone = Utility.ProcessPhone(mmm.Phone);
                        }
                            
                        currentMigratedEmailSMS.Add(mmm);
                    }

                    //SendCustomerMail(currentMigratedEmailSMS, "exceed100"); //send migrated customers e-mail
                    SendSMS(currentMigratedEmailSMS);   //send migrated customers SMS

                    Log.ReportLog($"Number of rows to be written is {currentMigrated.Count}");

                    bool status = rpt.GenerateSuccessfulReport(currentMigrated);

                    if (status)
                    {
                        SmtpClient SMTPSVR = new SmtpClient(svrInternal, port);

                        SMTPSVR.EnableSsl = false;
                        SMTPSVR.UseDefaultCredentials = false;
                        SMTPSVR.Credentials = new NetworkCredential(usr, pwd);

                        SMTPSVR.DeliveryMethod = SmtpDeliveryMethod.Network;

                        using (StreamWriter sw = new StreamWriter(lFilePath, false))
                        {
                            sw.WriteLine(total);
                            sw.Flush();
                            sw.Close();
                        }

                        try
                        {
                            BypassCertificateError();
                            SMTPSVR.Send(NotifyReport('s')); // send to BSC and Compliance

                            IEnumerable<IGrouping<string, Migrated>> groupedBySOL = from m in currentMigrated group m by m.BranchSOL;

                            foreach (IGrouping<string, Migrated> grouped in groupedBySOL)
                            {
                                List<Migrated> groupedms = new List<Migrated>();

                                foreach (Migrated acc in grouped)
                                {
                                    groupedms.Add(acc);
                                }

                                SMTPSVR.Send(NotifyBranch(grouped.Key, groupedms)); //send to domiciled branch
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteError($"Error while sending migrated accounts report e-mail: {ex.Message}");
                            return false;
                        }
                        finally
                        {
                            if (SMTPSVR != null)
                                SMTPSVR.Dispose();
                        }

                        return true;
                    }
                    else
                    {
                        Log.ReportLog("An error occured while generating the migrated accounts report");
                        return false;
                    }
                }
                else
                {
                    Log.ReportLog($"No report for {present.ToString("ddMMyyyy")} as there are no additional record in the database");
                    return false;
                }
            }
            else
            {
                Log.ReportLog($"No record in database");
                return false;
            }
        }

        private static MailMessage NotifyReport(char flg)
        {
            MailMessage Mail = new MailMessage();
            DateTime present = DateTime.Now;

            try
            {
                string reportPath = String.Empty;
                string filename = String.Empty;

                if (flg == 's')
                {
                    reportPath = ConfigurationManager.AppSettings["MigrationSuccessReport"];
                    filename = $"{present.ToString("ddMMyyyy")} migrated accounts report.csv";
                }
                else
                {
                    reportPath = ConfigurationManager.AppSettings["MigrationFailedReport"];
                    filename = $"{present.ToString("ddMMyyyy")} unmigrated accounts report.csv";
                }

                ContentType cType = new ContentType();
                cType.MediaType = MediaTypeNames.Application.Octet;
                cType.Name = filename;
                Mail.Attachments.Add(new Attachment(reportPath + "\\" + filename, cType));

                Mail.From = new MailAddress("firstbank@firstbanknigeria.com");

                if (flg=='s')
                {
                    Mail.Subject = $"MIGRATED TIERED ACCOUNTS REPORT FOR {present.ToString("dd/MM/yyyy")}";
                }
                else
                {
                    Mail.Subject = $"UNMIGRATED TIERED ACCOUNTS REPORT FOR {present.ToString("dd/MM/yyyy")}";
                }

                Mail.IsBodyHtml = true;
                StringBuilder emailcontent = new StringBuilder();
                emailcontent.Append("<head><style type=text/css> H1 {font-size:x-small;font-weight:normal;font-family:Arial;color:#21297B;} H3 {font-size:x-small;font-weight:bold;font-family:Arial;color:#FFF;} H2 {font-size:x-small;font-family:Arial;font-weight:normal;color:#FFF;} H6{margin-top:0;color:#000;}</style></head>");
                emailcontent.Append("<h1><font size='2' face='arial' _moz-rs-heading=''>Dear Team,<br/>");

                if (flg=='s')
                {
                    emailcontent.Append("<br/>Kindly find attached today's report of Tier 1 and Tier 2 accounts migrated to generic savings account.<br/>");
                }
                else
                {
                    emailcontent.Append("<br/>Kindly find attached today's report of Tier 1 and Tier 2 accounts that could not be migrated to generic savings account.<br/>");
                }
                
                emailcontent.Append("<br/>Regards<br/>");
                emailcontent.Append("<p>  </p>");
                string emailMsg = emailcontent.ToString();

                string _AppStartUpPath = AppDomain.CurrentDomain.BaseDirectory;

                AlternateView View = default(AlternateView);
                LinkedResource resource = default(LinkedResource);
                LinkedResource resource2 = default(LinkedResource);

                string imagepath = " <img src=cid:Image1 /> ";
                string imagepath2 = " <img src=cid:Image2 /> ";

                string body2 = imagepath + emailMsg + imagepath2;
                View = AlternateView.CreateAlternateViewFromString(body2.ToString(), null, "text/html");

                //link the resource to embed
                resource = new LinkedResource(_AppStartUpPath + "fbn_logo.jpg");
                resource2 = new LinkedResource(_AppStartUpPath + "ScamAlert.jpg");

                //name the resource
                resource.ContentId = "Image1";
                resource2.ContentId = "Image2";

                //add the resource to the alternate view
                View.LinkedResources.Add(resource);
                View.LinkedResources.Add(resource2);

                Mail.Body = emailMsg;

                string email1 = "SN027053@firstbanknigeria.com";
                string email2 = ConfigurationManager.AppSettings["email1"];
                string email3 = ConfigurationManager.AppSettings["email2"];
                string email4 = ConfigurationManager.AppSettings["email3"];

                Mail.AlternateViews.Add(View);
                Mail.To.Add(new MailAddress(email2));
                Mail.To.Add(new MailAddress(email3));
                Mail.CC.Add(new MailAddress(email4));
                Mail.Bcc.Add(new MailAddress(email1));
            }
            catch (Exception ex)
            {
                Log.WriteError($"{ex.Message}Report mail not built");
            }

            return Mail;
        }

        private static MailMessage NotifyBranch(string SOL, List<Migrated> branchMigrated)
        {
            MailMessage Mail = new MailMessage();
            DateTime present = DateTime.Now;
            string email = $"bm{SOL}@firstbanknigeria.com";

            try
            {
                Mail.From = new MailAddress("firstbank@firstbanknigeria.com");
                Mail.Subject = $"YOUR BRANCH'S MIGRATED TIERED ACCOUNT(S) FOR {present.ToString("dd/MM/yyyy")}";
                Mail.IsBodyHtml = true;
                StringBuilder emailcontent = new StringBuilder();
                emailcontent.Append("<head><style type=text/css> H1 {font-size:x-small;font-weight:normal;font-family:Arial;color:#21297B;} H3 {font-size:x-small;font-weight:bold;font-family:Arial;color:#FFF;} H2 {font-size:x-small;font-family:Arial;font-weight:normal;color:#FFF;} H6{margin-top:0;color:#000;} table,th,td { border: 1px solid; border-collapse: collapse; } thead th:nth-child(3) { width: 40%; text-align:center; } tbody tr:nth-child(odd) { background-color: #33e9ff; } tbody tr:nth-child(even) { background-color: #94b5b9; } td { text-align:center; }</style></head>");
                emailcontent.Append("<body><h1><font size='2' face='arial' _moz-rs-heading=''>Dear Team,<br/>");
                emailcontent.Append($"<br/>Kindly find below details of Tier 1 and Tier 2 account(s) domiciled in your branch SOL {SOL} that have been migrated to generic savings account (SA301).<br/>");
                emailcontent.Append($"<br/><table><thead><tr><th>S/N</th><th>Account Number</th><th>Account Name</th><th>Account Balance</th><th>Former scheme code</th><th>New scheme code</th></tr></thead><tbody>");

                int count = 1;

                foreach (Migrated m in branchMigrated)
                {
                    emailcontent.Append($"<tr><td>{count++}</td><td>{m.AccountNumber}</td><td>{m.AccountName}</td><td>{m.AccountBalance}</td><td>{m.SourceScheme}</td><td>{m.TargetScheme}</td></tr>");
                }

                emailcontent.Append("<br/><br/></tbody></table><br/>");
                emailcontent.Append("<br/>Regards<br/>");
                emailcontent.Append("<p>  </p></body>");
                string emailMsg = emailcontent.ToString();

                string _AppStartUpPath = AppDomain.CurrentDomain.BaseDirectory;

                AlternateView View = default(AlternateView);
                LinkedResource resource = default(LinkedResource);
                LinkedResource resource2 = default(LinkedResource);

                string imagepath = " <img src=cid:Image1 /> ";
                string imagepath2 = " <img src=cid:Image2 /> ";

                string body2 = imagepath + emailMsg + imagepath2;
                View = AlternateView.CreateAlternateViewFromString(body2.ToString(), null, "text/html");

                //link the resource to embed
                resource = new LinkedResource(_AppStartUpPath + "fbn_logo.jpg");
                resource2 = new LinkedResource(_AppStartUpPath + "ScamAlert.jpg");

                //name the resource
                resource.ContentId = "Image1";
                resource2.ContentId = "Image2";

                //add the resource to the alternate view
                View.LinkedResources.Add(resource);
                View.LinkedResources.Add(resource2);

                Mail.Body = emailMsg;
                Mail.AlternateViews.Add(View);

                Mail.To.Add(new MailAddress(email));

            }
            catch (Exception ex)
            {
                Log.WriteError($"{ex.Message}Report mail for branch {SOL} not built");
            }

            return Mail;
        }
    }
}