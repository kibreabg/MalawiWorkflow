using System;
using System.Data;
using System.Data.SqlClient;
using Chai.WorkflowManagment.Shared.Settings;

namespace Chai.WorkflowManagment.DBConnection
{    
    public class DatabaseConn : IConnectionManager
    {
        private SqlConnection _connection;
        private static IConnectionManager _theUniqueInstance;
        private SqlTransaction sqlTransaction;

        public SqlConnection GetSqlConnection()
        {
            return _connection;
        }

        private DatabaseConn()
        {
            InitConnections(TechnicalSettings.DatabaseLoginName, TechnicalSettings.DatabasePassword, TechnicalSettings.DatabaseServerName, TechnicalSettings.DatabaseName, "240");
        }

        private bool _connectionInitSuceeded = false;

        public bool ConnectionInitSuceeded 
        { 
            get { return _connectionInitSuceeded; } 
        }

        private void InitConnections(string pLogin, string pPassword, string pServer, string pDatabase, string pTimeout)
        {
            try
            {
                _connection = new SqlConnection(
                    String.Format("user id={0};password={1};data source={2};persist security info=False;initial catalog={3};connection timeout={4};Asynchronous Processing=true",
                                  pLogin, pPassword, pServer, pDatabase, pTimeout));
                _connection.Open();

                _connectionInitSuceeded = true;
            }
            catch
            {
                _connectionInitSuceeded = false;
            }
        }

        public static IConnectionManager GetInstance()
        {
            if (_theUniqueInstance == null)
                return _theUniqueInstance = new DatabaseConn();
            else
                return _theUniqueInstance;
        }

        public static void KillSingleton()
        {
            _theUniqueInstance = null;
        }
              

        public SqlConnection SqlConnection
        {
            get
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    try
                    {
                        _connection.Open();
                    }
                    catch (SqlException sqlEx)
                    {
                        throw new ApplicationException("Unable to connect to database (" + _connection.DataSource + "/" + _connection.Database + "). Please contact your local IT administrator.", sqlEx);
                    }
                }
                return _connection;
            }
        }

        public SqlTransaction GetSqlTransaction()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                try
                {
                    _connection.Open();
                }
                catch (SqlException ex)
                {
                    throw new ApplicationException("Unable to connect to database (" + _connection.DataSource + "/" + _connection.Database +
                        "). Please contact your local IT administrator.", ex);
                }
            }
            else
            {
                try
                {
                    throw new ApplicationException("COUCOU");
                }
                catch (ApplicationException ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.StackTrace);
                }
                sqlTransaction = _connection.BeginTransaction();
            }
            return sqlTransaction;
        }

        public void CloseConnection()
        {
            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
        }
      

        public static bool CheckSQLServerConnection()
        {
            string sqlConnection = String.Format(@"user id={0};password={1};data source={2};persist security info=False;initial catalog=MASTER;connection timeout=10",
                TechnicalSettings.DatabaseLoginName, TechnicalSettings.DatabasePassword, TechnicalSettings.DatabaseServerName);
            SqlConnection connection = new SqlConnection(sqlConnection);
            try
            {
                connection.Open();
                connection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CheckSQLDatabaseConnection()
        {
            string sqlConnection = String.Format(@"user id={0};password={1};data source={2};persist security info=False;initial catalog={3};connection timeout=10",
                TechnicalSettings.DatabaseLoginName, TechnicalSettings.DatabasePassword, TechnicalSettings.DatabaseServerName,TechnicalSettings.DatabaseName);
            SqlConnection connection = new SqlConnection(sqlConnection);
            try
            {
                connection.Open();
                connection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

}
