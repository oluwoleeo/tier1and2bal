using System;
using System.ServiceProcess;
using System.Timers;
using System.Runtime.InteropServices;

namespace Tier1And2BalanceEnforcement
{
    public partial class Tier1And2BalanceEnforcementService : ServiceBase
    {
        DateTime scheduleTime;
        Timer timer;

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        public Tier1And2BalanceEnforcementService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                // Update the service state to Start Pending.  
                ServiceStatus serviceStatus = new ServiceStatus();
                serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
                serviceStatus.dwWaitHint = 100000;
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);

                Log.ServiceLog("In OnStart");
                Log.ServiceLog($"Service Started {DateTime.Now.ToString()}");

                scheduleTime = DateTime.Today.AddDays(1).AddHours(-1).AddMinutes(50);
                double interval = scheduleTime.Subtract(DateTime.Now).TotalMilliseconds;

                // timer = new Timer(60000);
                timer = new Timer(interval);
                timer.Elapsed += new ElapsedEventHandler(OnTimer);
                timer.Start();

                // Update the service state to Running.  
                serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            }
            catch (Exception ex)
            {
                Log.ServiceLog($"\nError: {ex.Message}\n, stack trace: {ex.StackTrace}\n\n");
            }
        }

        protected void OnTimer(object sender, ElapsedEventArgs args)
        {
            Log.ServiceLog($"Timer elapsed, timestamp: {DateTime.Now.ToString()}");
            ExecuteNotification();

            scheduleTime = DateTime.Today.AddDays(2).AddHours(-1).AddMinutes(50);

            timer.Interval = scheduleTime.Subtract(DateTime.Now).TotalMilliseconds;

            Log.ServiceLog($"Interval to tomorrow's run: {timer.Interval} milliseconds");
            //timer.Interval = 60000;
        }

        public void ExecuteNotification()
        {
            timer.Stop();
            Log.ServiceLog($"Notification start: {DateTime.Now.ToString()}");
            Notify.InitProcess();
            Log.ServiceLog($"Notification end: {DateTime.Now.ToString()}");
            timer.Start();
        }

        protected override void OnStop()
        {
            try
            {
                Log.ServiceLog("In OnStop.");
                Log.ServiceLog($"Service stopped: {DateTime.Now.ToString()}");
            }
            catch (Exception ex)
            {
                Log.ServiceLog($"\nError: {ex.Message}\n, stack trace: {ex.StackTrace}\n\n");
            }

            timer.Stop();
            timer.Dispose();
        }
    }
}
