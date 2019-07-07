using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Drawing;
using System.IO;
using Chai.ZADS.Shared;

namespace Chai.ZADS.DataAccess
{
	/// <summary>
	/// Summary description for DatabaseHelper.
    /// </summary>
	public class DatabaseHelper
	{
		
        #region "Get Values from the parameters"


        private static bool IsDBNullValue(string pColName,SqlDataReader pReader)
		{
            return pReader.GetValue(pReader.GetOrdinal(pColName)) == DBNull.Value ? true : false;
		}

		public static string GetString(string pColName,SqlDataReader pReader)
		{
		    return !IsDBNullValue(pColName,pReader) ? pReader.GetString(pReader.GetOrdinal(pColName)) : null;
		}

	    public static int GetInt32(string pColName,SqlDataReader pReader)
		{
            if (!IsDBNullValue(pColName, pReader))
                return pReader.GetInt32(pReader.GetOrdinal(pColName));
            else
                return 0;
		}

		public static int? GetNullAuthorizedInt32(string pColName,SqlDataReader pReader)
		{
		    return IsDBNullValue(pColName, pReader) ? (int?) null : GetInt32(pColName, pReader);
		}

        public static int? GetNullAuthorizedSmallInt(string pColName, SqlDataReader pReader)
        {
            return IsDBNullValue(pColName, pReader) ? (int?)null : GetSmallInt(pColName, pReader);
        }

	    public static bool? GetNullAuthorizedBoolean(string pColName,SqlDataReader pReader)
	    {
	        return IsDBNullValue(pColName, pReader) ? (bool?) null : GetBoolean(pColName, pReader);
	    }

	    public static bool GetBoolean(string pColName,SqlDataReader pReader)
		{
            if (!IsDBNullValue(pColName, pReader))
                return pReader.GetBoolean(pReader.GetOrdinal(pColName));
            return false;
		}

		public static decimal GetMoney(string pColName,SqlDataReader pReader)
		{
            if (!IsDBNullValue(pColName, pReader))
                return pReader.GetDecimal(pReader.GetOrdinal(pColName));
            return 0;
		}

        public static byte[]  GetBytes(string pColName, SqlDataReader pReader)
        {
            return (byte[])pReader.GetValue(pReader.GetOrdinal(pColName));
        }

		public static decimal? GetNullAuthorizedMoney(string pColName,SqlDataReader pReader)
		{
		    return IsDBNullValue(pColName,pReader) ? (decimal?)null : GetMoney(pColName,pReader);
		}

	    public static double GetDouble(string pColName,SqlDataReader pReader)
		{
            if (!IsDBNullValue(pColName, pReader))
                return pReader.GetDouble(pReader.GetOrdinal(pColName));
            else
                return 0;
		}

		public static double? GetNullAuthorizedDouble(string pColName,SqlDataReader pReader)
		{
		    return IsDBNullValue(pColName, pReader) ? (double?) null : GetDouble(pColName, pReader);
		}

	    public static DateTime GetDateTime(string pColName,SqlDataReader pReader)
		{
            if (!IsDBNullValue(pColName, pReader))
                return pReader.GetDateTime(pReader.GetOrdinal(pColName));
            else
                return DateTime.MinValue;
		}

		public static DateTime? GetNullAuthorizedDateTime(string pColName,SqlDataReader pReader)
		{
		    return IsDBNullValue(pColName, pReader) ? (DateTime?) null : GetDateTime(pColName, pReader);
		}

        public static Guid GetGuid(string pColName,SqlDataReader pReader)
		{
			return pReader.GetSqlGuid(pReader.GetOrdinal(pColName)).Value;
        }

	    public static char GetChar(string pColName,SqlDataReader pReader)
		{
			return pReader.GetString(pReader.GetOrdinal(pColName)).ToCharArray()[0];
		}

        public static Image GetPhoto(string pString, SqlDataReader pReader)
        {
            if (IsDBNullValue(pString, pReader))
                return null;

            byte[] temp = (byte[])pReader.GetValue(pReader.GetOrdinal(pString));
            return new Bitmap(new MemoryStream(temp));
        }

		public static int GetSmallInt(string pColName,SqlDataReader pReader)
		{
			return pReader.GetInt16(pReader.GetOrdinal(pColName));
        }
        #endregion

        #region "Set values to the parameters"

        public static void InsertMoneyParam(string pParamName,SqlCommand pCommand,decimal? pVal)
		{
            if (!pVal.HasValue)
            {
                pCommand.Parameters.AddWithValue(pParamName, SqlMoney.Null);
            }
            else
            {
                pCommand.Parameters.Add(pParamName, SqlDbType.Money);
                pCommand.Parameters[pParamName].Value = pVal.Value;
            }
		}

        public static void InsertMoneyParam(string pParamName, SqlCommand pCommand, decimal pVal)
        {

            pCommand.Parameters.Add(pParamName, SqlDbType.Money);
            pCommand.Parameters[pParamName].Value = pVal;
        }

        public static void InsertInt32Param(string pParamName, SqlCommand pCommand, int pVal)
		{
			pCommand.Parameters.Add(pParamName,SqlDbType.Int);
			pCommand.Parameters[pParamName].Value = pVal;
		}

        public static void InsertSmalIntParam(string pParamName, SqlCommand pCommand, int pVal)
		{
			pCommand.Parameters.Add(pParamName,SqlDbType.SmallInt);
			pCommand.Parameters[pParamName].Value = pVal;		
		}

        public static void InsertNullValue(string pParamName, SqlCommand pCommand)
		{
			pCommand.Parameters.AddWithValue(pParamName,SqlInt32.Null);
		}

        public static void InsertInt32Param(string pParamName, SqlCommand pCommand, int? pVal)
		{
			if (!pVal.HasValue)
			{
				pCommand.Parameters.AddWithValue(pParamName,SqlInt32.Null);				
			}
			else
			{
				pCommand.Parameters.Add(pParamName,SqlDbType.Int);
				pCommand.Parameters[pParamName].Value = pVal.Value;
			}
		}

        public static void InsertDoubleParam(string pParamName, SqlCommand pCommand, double pVal)
		{
			pCommand.Parameters.Add(pParamName,SqlDbType.Float);
			pCommand.Parameters[pParamName].Value = pVal;
		}

        public static void InsertDoubleParam(string pParamName, SqlCommand pCommand, double? pVal)
		{
			if (!pVal.HasValue)
			{
				pCommand.Parameters.AddWithValue(pParamName,DBNull.Value);				
			}
			else
			{
				pCommand.Parameters.Add(pParamName,SqlDbType.Float);
				pCommand.Parameters[pParamName].Value = pVal.Value;
			}
		}

        public static void InsertBooleanParam(string pParamName, SqlCommand pCommand, bool? pVal)
		{
			if (!pVal.HasValue)
			{
				pCommand.Parameters.AddWithValue(pParamName,DBNull.Value);				
			}
			else
			{
				pCommand.Parameters.Add(pParamName,SqlDbType.Bit);
				pCommand.Parameters[pParamName].Value = pVal.Value;
			}
		}

        public static void InsertBooleanParam(string pParamName, SqlCommand pCommand, bool pVal)
		{
			pCommand.Parameters.Add(pParamName,SqlDbType.Bit);
			pCommand.Parameters[pParamName].Value = pVal;
		}

        public static void InsertStringNVarCharParam(string pParamName, SqlCommand pCommand, string pVal)
		{
			if (pVal != null)
			{
				pCommand.Parameters.Add(pParamName,SqlDbType.NVarChar);
				pCommand.Parameters[pParamName].Value = pVal;
			}
			else
				pCommand.Parameters.AddWithValue(pParamName,DBNull.Value);
		}

        public static void InsertStringVarCharParam(string pParamName, SqlCommand pCommand, string pVal)
		{
			if (pVal != null)
			{
				pCommand.Parameters.Add(pParamName,SqlDbType.VarChar,pVal.Length);
				pCommand.Parameters[pParamName].Value = pVal;
			}
			else
				pCommand.Parameters.AddWithValue(pParamName,DBNull.Value);
		}

        public static void InsertCharParam(string pParamName, SqlCommand pCommand, char pVal)
		{
			pCommand.Parameters.Add(pParamName,SqlDbType.Char,1);
			pCommand.Parameters[pParamName].Value = pVal;
		}

        public static void InsertGuidParam(string pParamName, SqlCommand pCommand, Guid pVal)
        {
            if (pVal != null)
            {
                pCommand.Parameters.Add(pParamName, SqlDbType.UniqueIdentifier);
                pCommand.Parameters[pParamName].Value = pVal;
            }
            else
                pCommand.Parameters.AddWithValue(pParamName, DBNull.Value);
        }

        public static void InsertDateTimeParam(string pParamName, SqlCommand pCommand, DateTime pVal)
		{
			pCommand.Parameters.Add(pParamName,SqlDbType.DateTime);
			pCommand.Parameters[pParamName].Value = pVal.Date;
		}

        public static void InsertDateTimeParam(string pParamName, SqlCommand pCommand, DateTime? pVal)
		{
			if (!pVal.HasValue)
			{
				pCommand.Parameters.AddWithValue(pParamName,DBNull.Value);			
			}
			else
			{
				pCommand.Parameters.Add(pParamName,SqlDbType.DateTime);
                pCommand.Parameters[pParamName].Value = pVal.Value;
			}
		}
 
  #endregion  

      public static void InsertObjectParam(string pParamName, SqlCommand pCommand, object pVal)
        {
            if (null == pVal)
                InsertNullValue(pParamName, pCommand);
            else if (pVal is int)
                InsertInt32Param(pParamName, pCommand, (int) pVal);
            else if (pVal is double)
                InsertDoubleParam(pParamName, pCommand, (double) pVal);
            else if (pVal is bool)
                InsertBooleanParam(pParamName, pCommand, (bool) pVal);
            else if (pVal is DateTime)
                InsertDateTimeParam(pParamName, pCommand, (DateTime) pVal);
            else if (pVal is decimal)
                InsertMoneyParam(pParamName, pCommand, (decimal)pVal);
            else if (pVal is char)
                InsertCharParam(pParamName, pCommand, (char) pVal);
            else
                InsertStringNVarCharParam(pParamName, pCommand, pVal as string);
        }
	}


}
