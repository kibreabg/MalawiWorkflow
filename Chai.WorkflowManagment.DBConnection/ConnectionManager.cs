using System;
using System.Data.SqlClient;
using Chai.WorkflowManagment.Shared.Settings;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.DBConnection
{
    public class ConnectionManager
    {
        private readonly IConnectionManager _connectionManager;
        private static ConnectionManager _theUniqueInstance;

        private ConnectionManager()
        {
            _connectionManager = DatabaseConn.GetInstance();
        }
                
        public static ConnectionManager GetInstance()
        {
            return _theUniqueInstance ?? (_theUniqueInstance = new ConnectionManager());
        }

        
        public static void KillSingleton()
        {
            DatabaseConn.KillSingleton();
            _theUniqueInstance = null;
        }

        public SqlConnection SqlConnection
        {
            get { return _connectionManager.SqlConnection; }
        }
                
        public SqlTransaction GetSqlTransaction()
        {
            return _connectionManager.GetSqlTransaction();
        }

        public void CloseConnection()
        {
            _connectionManager.CloseConnection();
        }
        
        public bool ConnectionInitSuceeded
        {
            get { return _connectionManager.ConnectionInitSuceeded; }
        }
               
        public static bool CheckSQLServerConnection()
        {
            return DatabaseConn.CheckSQLServerConnection();
        }

        public static bool CheckSQLDatabaseConnection()
        {
            return DatabaseConn.CheckSQLDatabaseConnection();
        }
                
        public static string ConnectionString
        {
            get
            {
                return TechnicalConfig.GetConfiguration()["ConnectionString"];
            }
        }
    }
}
