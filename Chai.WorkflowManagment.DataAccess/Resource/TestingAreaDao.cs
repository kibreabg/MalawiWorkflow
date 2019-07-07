using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain.Resource;

namespace Chai.ZADS.DataAccess.Resource
{
    public class TestingAreaDao : BaseDao
    {
        public TestingArea GetTestingAreaById(int testingAreaId)
        {
            string sql = "SELECT [TestingAreaId], [AreaName], [TestCategory] FROM TestingArea where TestingAreaId = @testingAreaId ";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@testingAreaId", cm, testingAreaId);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                return FetchTestingArea(dr);
                            }
                        }
                    }
                }
            }
            return null;
        }

        private TestingArea FetchTestingArea(SqlDataReader dr)
        {
            TestingArea testingArea = new TestingArea
            {
                Id = DatabaseHelper.GetInt32("TestingAreaId", dr),
                TestingAreaName = DatabaseHelper.GetString("AreaName", dr),
                TestCategory = DatabaseHelper.GetString("TestCategory", dr)
            };

            return testingArea;
        }
        
        private static void SetTestingArea(SqlCommand cm, TestingArea testingArea)
        {
            DatabaseHelper.InsertStringNVarCharParam("@AreaName", cm, testingArea.TestingAreaName);
            DatabaseHelper.InsertStringNVarCharParam("@TestCategory", cm, testingArea.TestCategory);
        }
        
        public void Save(TestingArea testingArea,SqlTransaction tr)
        {
            string sql = "INSERT INTO TestingArea(AreaName, TestCategory) VALUES (@AreaName, @TestCategory) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, tr))
            {
                SetTestingArea(cm, testingArea);
                testingArea.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }
      
        public void Update(TestingArea testingArea,SqlTransaction tr)
        {
            string sql = "Update TestingArea SET AreaName = @AreaName, TestCategory = @TestCategory  where TestingAreaId = @testingAreaId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection,tr))
            {
                DatabaseHelper.InsertInt32Param("@testingAreaId", cm, testingArea.Id);
                SetTestingArea(cm, testingArea);
                cm.ExecuteNonQuery();
            }
        }
       
        public void Delete(int testingAreaId)
        {
            string sql = " Delete TestingArea where TestingAreaId = @testingAreaId ";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@testingAreaId", cm, testingAreaId);
                cm.ExecuteNonQuery();
            }
        }
       
        public IList<TestingArea> GetListOfTestingArea()
        {
            string sql = "SELECT * FROM TestingArea ";

            IList<TestingArea> lstTestingArea = new List<TestingArea>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                lstTestingArea.Add(FetchTestingArea(dr));
                            }
                        }
                    }
                }
            }
            return lstTestingArea;
        }

    }
}
