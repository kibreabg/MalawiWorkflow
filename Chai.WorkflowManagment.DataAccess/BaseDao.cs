using System;
using System.Data.SqlClient;
using System.Text;
using Chai.ZADS.DBConnection;

namespace Chai.ZADS.DataAccess
{
    public class BaseDao
    {
        private readonly ConnectionManager _connectionManager = null;
        private Exception _sqlError = null;

        protected BaseDao()
        {
            _connectionManager = ConnectionManager.GetInstance();
        }

        protected SqlConnection DefaultConnection
        {
            get 
            {
                return _connectionManager == null ? null : _connectionManager.SqlConnection;
            }
        }
        
        protected string ConnectionString
        {
            get { return ConnectionManager.ConnectionString; }
        }

        public void OnSqlInfoMessageEventHandler(object sender,  SqlInfoMessageEventArgs e)
        {
            //checks for any errors.
            if (e.Errors.Count > 0)
            {
                StringBuilder strerror = new StringBuilder();
                foreach (SqlError error in e.Errors)
                {
                    strerror.AppendLine(error.Number.ToString() + " = " +
                                        error.Message);
                }

                //DefaultConnection.InfoMessage -= new SqlInfoMessageEventHandler(OnSqlInfoMessageEventHandler);
                //throw  new Exception(strerror.ToString());
                _sqlError = new Exception(strerror.ToString());
            }
        }

        public Exception SqlError
        {
            get { return _sqlError; }
        }

        public void ClearSqlError()
        {
            _sqlError = null;
        }

    }
}
