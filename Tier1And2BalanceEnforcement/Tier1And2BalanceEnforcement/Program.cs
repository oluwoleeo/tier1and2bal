using System.ServiceProcess;

namespace Tier1And2BalanceEnforcement
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Tier1And2BalanceEnforcementService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
