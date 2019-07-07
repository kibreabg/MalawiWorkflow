using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain.Resource;

namespace Chai.ZADS.DataAccess.Resource
{
    public class ForecastDao : BaseDao
    {
        public ForecastInfo GetForecastInfoById(int id)
        {
            string sql = "SELECT SELECT [ForecastID], [ForecastDate], [StartingMonth], [StartingYear] ";
            sql += "FROM ForecastInfo where ForecastId = @Id ";
            sql += "SELECT [ForecastId], [FacilityId], [TestId], [Id], [TestType], [IsHistorical], [HistoricalValue], [ForecastValue], ";
            sql +=" [DurationDateTime], [TotalForecastValue]  FROM ForecastedResult where ForecastId = @Id ";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@TestId", cm, id);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                ForecastInfo test = FetchForecastInfo(dr);
                                FetchForecastedResult(dr, test);
                                return test;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private ForecastInfo FetchForecastInfo(SqlDataReader dr)
        {
            ForecastInfo t = new ForecastInfo()
            {
                Id = DatabaseHelper.GetInt32("ForecastId", dr),
                ForecastDate = DatabaseHelper.GetDateTime("ForecastDate", dr),
                StartingMonth = DatabaseHelper.GetString("StartingMonth", dr),
                StartingYear = DatabaseHelper.GetInt32("StartingYear", dr)
            };
            return t;
        }

        private void FetchForecastedResult(SqlDataReader dr, ForecastInfo test)
        {
            while (dr.Read())
            {
                ForecastedResult p = new ForecastedResult()
                {

                    Id = DatabaseHelper.GetInt32("Id", dr),
                    TestId = DatabaseHelper.GetInt32("TestId", dr),
                    DurationDateTime = DatabaseHelper.GetDateTime("DurationDateTime", dr),
                    FacilityId = DatabaseHelper.GetInt32("FacilityId", dr),
                    ForecastId = DatabaseHelper.GetInt32("ForecastId", dr),
                    ForecastValue = DatabaseHelper.GetDouble("ForecastValue", dr),
                    HistoricalValue = DatabaseHelper.GetInt32("HistoricalValue", dr),
                    IsHistorical = DatabaseHelper.GetBoolean("IsHistorical", dr),
                    TotalForecastValue = DatabaseHelper.GetDouble("TotalForecastValue", dr),
                    TestType = DatabaseHelper.GetString("TestType", dr)
                };

                test.ForecastedResults.Add(p);
            }
        }

        private void SetForecastInfo(SqlCommand cm, ForecastInfo test)
        {
            DatabaseHelper.InsertDateTimeParam("@ForecastDate", cm, test.ForecastDate);
            DatabaseHelper.InsertStringNVarCharParam("@StartingMonth", cm, test.StartingMonth);
            DatabaseHelper.InsertInt32Param("@StartingYear", cm, test.StartingYear);
        }
              

        public void SaveForecastInfo(ForecastInfo test, SqlTransaction tr)
        {
            string sql = "INSERT INTO ForecastInfo(ForecastDate, StartingMonth, StartingYear)";
            sql += "VALUES(@ForecastDate, @StartingMonth, @StartingYear) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, tr))
            {
                SetForecastInfo(cm, test);
                test.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }
                
        public void UpdateForecastInfo(ForecastInfo forecast, SqlTransaction tr)
        {
            string sql = "Update Test SET ForecastDate = @ForecastDate, StartingMonth = @StartingMonth, StartingYear = @StartingYear where ForecastId = @Id ";
            
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, tr))
            {
                DatabaseHelper.InsertInt32Param("@Id", cm, forecast.Id);
                SetForecastInfo(cm, forecast);
                cm.ExecuteNonQuery();
            }
        }

        public void DeleteForecastInfo(ForecastInfo forecast)
        {
            string sql = "DELETE FROM ForecastInfo  WHERE ForecastID = @Id";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@Id", cm, forecast.Id);
                cm.ExecuteNonQuery();
            }
        }

        public void SaveForecastedResult(ForecastedResult fr, SqlTransaction tr)
        {
            string sql = "INSERT INTO ForecastedResult ([ForecastId],[FacilityId] ,[TestId], [TestType], [IsHistorical] ,[HistoricalValue], ";
            sql += "[ForecastValue] ,[DurationDateTime],[TotalForecastValue]) VALUES (@ForecastId, @FacilityId, @TestId, @TestType, @IsHistorical, ";
            sql += "@HistoricalValue, @ForecastValue, @DurationDateTime, @TotalForecastValue) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, tr))
            {
                SetForecastedResult(cm, fr);
                fr.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }
        private void SetForecastedResult(SqlCommand cm, ForecastedResult p)
        {
            DatabaseHelper.InsertDateTimeParam("@DurationDateTime", cm, p.DurationDateTime);
            DatabaseHelper.InsertInt32Param("@FacilityId", cm, p.FacilityId);
            DatabaseHelper.InsertMoneyParam("@ForecastId", cm, p.ForecastId);
            DatabaseHelper.InsertDoubleParam("@ForecastValue", cm, p.ForecastValue);
            DatabaseHelper.InsertInt32Param("@HistoricalValue", cm, p.HistoricalValue);
            DatabaseHelper.InsertBooleanParam("@IsHistorical", cm, p.IsHistorical);
            DatabaseHelper.InsertInt32Param("@TestId", cm, p.TestId);
            DatabaseHelper.InsertDoubleParam("@TotalForecastValue", cm, p.TotalForecastValue);
            DatabaseHelper.InsertStringNVarCharParam("@TestType", cm, p.TestType);
        }
    }
}
