using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Tier1And2BalanceEnforcement
{
    public class Report
    {
        string lFilePath = "";
        public Report(char flg)
        {
            if (flg=='s')
            {
                string lastPickedPath = ConfigurationManager.AppSettings["lastpicked"];
                DateTime present = DateTime.Now;

                if (!Directory.Exists(lastPickedPath))
                {
                    Directory.CreateDirectory(lastPickedPath);
                }

                lFilePath = lastPickedPath + "\\LastPicked.txt";

                try
                {
                    if (!File.Exists(lFilePath))
                    {
                        using (StreamWriter sw = new StreamWriter(lFilePath, false))
                        {
                            sw.WriteLine(0);
                            sw.Flush();
                            sw.Close();
                        }
                        Log.ReportLog("LastPicked.txt created, 0 written");
                    }
                    else
                    {
                        Log.ReportLog("LastPicked.txt exists");
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        public bool GenerateSuccessfulReport(List<Migrated> currentMigrated)
        {
            DateTime present = DateTime.Now;

            string reportPath = ConfigurationManager.AppSettings["MigrationSuccessReport"];

            if (!Directory.Exists(reportPath))
            {
                Directory.CreateDirectory(reportPath);
            }

            string filePath = reportPath + "\\" + present.ToString("ddMMyyyy") + " migrated accounts report.csv";

            try
            {
                Log.ReportLog($"Successfully migrated report process for {present.ToString("ddMMyyyy")} begin");

                if (!File.Exists(filePath))
                {
                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\"", "S/N", "Account number", "Account name", "Account balance", "Branch SOL ID", "Former scheme code", "New scheme code", "Moved date"));
                        sw.Flush();
                        sw.Close();
                    }
                }

                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    int count = 1;

                    foreach (Migrated acc in currentMigrated)
                    {
                        sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\"", count++, acc.AccountNumber, acc.AccountName, acc.AccountBalance, acc.BranchSOL, acc.SourceScheme, acc.TargetScheme, acc.MovedDate));
                        sw.Flush();
                    }

                    sw.Close();
                }
                Log.ReportLog($"Successfully migrated report process for {present.ToString("ddMMyyyy")} end");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool GenerateFailedReport(List<FailedToMigrate> fAccounts)
        {
            DateTime present = DateTime.Now;

            string reportPath = ConfigurationManager.AppSettings["MigrationFailedReport"];

            if (!Directory.Exists(reportPath))
            {
                Directory.CreateDirectory(reportPath);
            }

            string filePath = reportPath + "\\" + present.ToString("ddMMyyyy") + " unmigrated accounts report.csv";

            try
            {
                Log.ReportLog($"Failed to migrate report process for {present.ToString("ddMMyyyy")} begin");

                if (!File.Exists(filePath))
                {
                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\"", "S/N", "Account number", "Account name", "Account balance", "Branch SOL ID", "Former scheme code", "New scheme code","Reason"));
                        sw.Flush();
                        sw.Close();
                    }
                }

                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    int count = 1;

                    foreach (FailedToMigrate acc in fAccounts)
                    {
                        sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\"", count++, acc.AccountNumber, acc.AccountName, acc.AccountBalance, acc.BranchSOL, acc.SourceScheme, acc.TargetScheme,acc.Reason));
                        sw.Flush();
                    }

                    sw.Close();
                }
                Log.ReportLog($"Failed to migrate report process for {present.ToString("ddMMyyyy")} end");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
