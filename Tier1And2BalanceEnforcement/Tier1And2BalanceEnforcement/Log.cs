using System;
using System.IO;
using System.Configuration;

namespace Tier1And2BalanceEnforcement
{
    public class Log
    {
        public static void WriteEvent(string eventToWrite)
        {
            string eventsLogPath = ConfigurationManager.AppSettings["EventLogs"];
            DateTime present = DateTime.Now;

            if (!Directory.Exists(eventsLogPath))
            {
                Directory.CreateDirectory(eventsLogPath);
            }

            string filePath = eventsLogPath + "\\" + present.ToString("ddMMyyyy") + " event log.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("Timestamp: " + present.ToString("dd/MM/yy HH:mm:ss") + ", event: " + eventToWrite);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void WriteError(string errorToWrite)
        {
            string errorLogPath = ConfigurationManager.AppSettings["ErrorLogs"];
            DateTime present = DateTime.Now;

            if (!Directory.Exists(errorLogPath))
            {
                Directory.CreateDirectory(errorLogPath);
            }

            string filePath = errorLogPath + "\\" + present.ToString("ddMMyyyy") + " error log.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("Timestamp: " + present.ToString("dd/MM/yy HH:mm:ss") + ", error: " + errorToWrite);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void DetailsLog(int i, string flag, ToBeNotified dets)
        {
            string detailsLogPath = ConfigurationManager.AppSettings["DetailsLogs"];
            DateTime present = DateTime.Now;

            if (!Directory.Exists(detailsLogPath))
            {
                Directory.CreateDirectory(detailsLogPath);
            }

            string filePath = detailsLogPath + "\\" + present.ToString("ddMMyyyy") + " details log.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    if (flag == "EA")
                    {
                        sw.WriteLine("Timestamp: " + present.ToString("dd/MM/yy HH:mm:ss") + ", " + $"Id: {i}, Account balance: {dets.AccountBalance}, Account Name: {dets.AccountName}, Account number: {dets.AccountNumber}, Branch SOL: {dets.BranchSOL}, Percentage: {dets.Percentage}, Scheme code: {dets.SchemeCode}, Customer email: {dets.CustomerEmail}, Customer phone: {dets.CustomerPhone}");
                    }
                    else if (flag == "NEA")
                    {
                        sw.WriteLine("Timestamp: " + present.ToString() + ", " + $"Id: {i}, Account balance: {dets.AccountBalance}, Account Name: {dets.AccountName}, Account number: {dets.AccountNumber}, Branch SOL: {dets.BranchSOL}, Percentage: {dets.Percentage}, Scheme code: {dets.SchemeCode}, Customer email: {dets.CustomerEmail}");
                    }
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void EmailNotificationStatusLog(string status)
        {
            string statusLogPath = ConfigurationManager.AppSettings["EmailNotifyStatusLogs"];
            DateTime present = DateTime.Now;

            if (!Directory.Exists(statusLogPath))
            {
                Directory.CreateDirectory(statusLogPath);
            }

            string filePath = statusLogPath + "\\" + present.ToString("ddMMyyyy") + " email notification log.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("Timestamp: " + present.ToString("dd/MM/yy HH:mm:ss") + ", status: " + status);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void SMSNotificationStatusLog(string status)
        {
            string statusLogPath = ConfigurationManager.AppSettings["SMSNotifyStatusLogs"];
            DateTime present = DateTime.Now;

            if (!Directory.Exists(statusLogPath))
            {
                Directory.CreateDirectory(statusLogPath);
            }

            string filePath = statusLogPath + "\\" + present.ToString("ddMMyyyy") + " SMS notification log.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("Timestamp: " + present.ToString("dd/MM/yy HH:mm:ss") + ", status: " + status);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void ServiceLog(string status)
        {
            string statusLogPath = ConfigurationManager.AppSettings["ServiceLog"];
            DateTime present = DateTime.Now;

            if (!Directory.Exists(statusLogPath))
            {
                Directory.CreateDirectory(statusLogPath);
            }

            string filePath = statusLogPath + "\\Tier1&2BalanceEnforcement service log.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("Timestamp: " + present.ToString("dd/MM/yy HH:mm:ss") + ", status: " + status);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void ReportLog(string status)
        {
            string reportstatusLogPath = ConfigurationManager.AppSettings["reportslog"];
            DateTime present = DateTime.Now;

            if (!Directory.Exists(reportstatusLogPath))
            {
                Directory.CreateDirectory(reportstatusLogPath);
            }

            string filePath = reportstatusLogPath + "\\" + present.ToString("ddMMyyyy") + " report status log.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine("Timestamp: " + present.ToString("dd/MM/yy HH:mm:ss") + ", event: " + status);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
