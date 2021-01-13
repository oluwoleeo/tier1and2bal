using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace Tier1And2BalanceEnforcement
{
    public class DapperContext : IDisposable
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["FbnMockDBConString"].ConnectionString;
        private static string providerName = ConfigurationManager.ConnectionStrings["FbnMockDBConString"].ProviderName;
        private static bool connectionOnDemand = ConfigurationManager.AppSettings["ConnectionOnDemand"] == null || ConfigurationManager.AppSettings["ConnectionOnDemand"] == "true" ? true : false;
        private readonly object @lock = new object();
        private IDbConnection cn;
        public IDbConnection Connection { get { return GetConnection(); } }
        private IDbTransaction transaction;

        public DapperContext()
        {
        }

        public DapperContext(string connectionStringName)
        {
            _connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            providerName = ConfigurationManager.ConnectionStrings[connectionStringName].ProviderName;
        }

        public IDbTransaction GetTransaction()
        {
            if (cn != null && transaction != null) { return transaction; }
            if (cn != null)
            {
                if (cn.State == ConnectionState.Open) { cn.Close(); }
                cn.Open();
                transaction = cn.BeginTransaction();
                return transaction;
            }
            return null;
        }

        public void CommitTransaction()
        {
            if (cn == null || transaction == null) { return; }
            transaction.Commit();
            cn.Close();
            if (!connectionOnDemand)
                cn.Open();

            transaction = null;
        }

        public void RollbackTransaction()
        {
            if (cn == null || transaction == null) { return; }
            if (cn.State != ConnectionState.Closed  && cn.State !=  ConnectionState.Broken) 
                transaction.Rollback();
            cn.Close();
            if (!connectionOnDemand)
                cn.Open();

            transaction = null;
        }

        public void OpenIfNot()
        {

            if (cn != null && cn.State != ConnectionState.Broken && cn.State == ConnectionState.Closed) { cn.Open(); }
        }

        public void CloseIfNot()
        {
            if (cn != null && cn.State != ConnectionState.Closed) { cn.Close(); }
        }

        private IDbConnection GetConnection()
        {
            if (cn == null)
            {
                lock (@lock)
                {
                    try
                    {
                        if (providerName == "System.Data.SqlClient")
                        {
                            cn = new SqlConnection(_connectionString);
                            Log.WriteEvent("SQL connection instance gotten");
                        }
                        else if (providerName == "Oracle.ManagedDataAccess")
                        {
                            cn = new OracleConnection(_connectionString);
                            Log.WriteEvent("Oracle connection instance gotten");
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("providerName", "Unable to resolve ADO.NET provider name");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError($"Error in DapperContext.GetConnection: {ex.Message} --- {ex.StackTrace}");
                    }
                   
                    //if (!connectionOnDemand)
                    //    cn.Open();
                }
            }

            try
            {
                cn.Open();
            }
            catch (Exception ex)
            {
                Log.WriteError($"Error trying to open connection in DapperContext.GetConnection: {ex.Message} --- {ex.StackTrace}");
            }

            return cn;
        }

        #region IDisposable

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (transaction != null)
                    { transaction.Dispose(); }
                    if (cn != null)
                    { cn.Dispose(); }
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
