using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain.Resource;

namespace Chai.ZADS.DataAccess.Resource
{
    public class TestDao : BaseDao
    {

        public Test GetTestById(int testId)
        {
            string sql = "SELECT TestingAreaId, InstrumentId, TestId, TestName, TestType, Description, ";
            sql += "(select InstrumentName from Instrument where InstrumentId = Test.InstrumentId) as InsName, ";
            sql += "(select AreaName from TestingArea where TestingAreaId = Test.TestingAreaId) as AreaName ";
            sql += "FROM Test where TestId = @TestId ";
            sql += "SELECT u.TestId, u.ProductId, u.Id, u.Rate, p.ProductName, p.BasicUnit, p.PackSize ";
            sql += "FROM ProductUsage AS u INNER JOIN Product AS p ON u.ProductId = p.ProductID ";
            sql += "where u.TestId = @TestId";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@TestId", cm, testId);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                Test test = FetchTest(dr);
                                FetchProductUsage(dr, test);
                                return test;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private Test FetchTest(SqlDataReader dr)
        {
            Test t = new Test()
            {
                Id = DatabaseHelper.GetInt32("TestId", dr),
                TestName = DatabaseHelper.GetString("TestName", dr),
                InstrumentId = DatabaseHelper.GetInt32("InstrumentId", dr),
                TestingAreaId = DatabaseHelper.GetInt32("TestingAreaId", dr),
                TestType = DatabaseHelper.GetString("TestType", dr),
                Description = DatabaseHelper.GetString("Description", dr),
                InstrumentName = DatabaseHelper.GetString("InsName", dr),
                TestingAreaName = DatabaseHelper.GetString("AreaName", dr)
            };
            return t;
        }

        private void FetchProductUsage(SqlDataReader dr, Test test)
        {
            while (dr.Read())
            {
                ProductUsage p = new ProductUsage()
                {

                    Id = DatabaseHelper.GetInt32("Id", dr),
                    TestId = DatabaseHelper.GetInt32("TestId", dr),
                    ProductId = DatabaseHelper.GetInt32("ProductId", dr),
                    Rate = DatabaseHelper.GetMoney("Rate", dr),
                    ProductName = DatabaseHelper.GetString("ProductName", dr),
                    BasicUnit = DatabaseHelper.GetString("BasicUnit", dr),
                    PackSize = DatabaseHelper.GetInt32("PackSize", dr)
                };

                test.ProductUsages.Add(p);
            }
        }

        private void SetTest(SqlCommand cm, Test test)
        {
            DatabaseHelper.InsertInt32Param("@TestingAreaId", cm, test.TestingAreaId);
            DatabaseHelper.InsertInt32Param("@InstrumentId", cm, test.InstrumentId);
            DatabaseHelper.InsertStringNVarCharParam("@TestName", cm, test.TestName);
            DatabaseHelper.InsertStringNVarCharParam("@Description", cm, test.Description);
            DatabaseHelper.InsertStringNVarCharParam("@TestType", cm, test.TestType);
        }
        
        private void SetProductUsage(SqlCommand cm, ProductUsage ProductUsage)
        {
            DatabaseHelper.InsertInt32Param("@TestId", cm, ProductUsage.TestId);
            DatabaseHelper.InsertInt32Param("@ProductId", cm, ProductUsage.ProductId);
            DatabaseHelper.InsertMoneyParam("@Rate", cm, ProductUsage.Rate);
        }
        
        public void SaveTest(Test test, SqlTransaction tr)
        {
            string sql = "INSERT INTO Test(TestingAreaId, InstrumentId, TestName, TestType, Description) VALUES ";
            sql +="(@TestingAreaId, @InstrumentId, @TestName, @TestType, @Description) SELECT @@identity";
            
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, tr))
            {
                SetTest(cm, test);
                test.Id = int.Parse(cm.ExecuteScalar().ToString());
                
                foreach (ProductUsage pu in test.ProductUsages)
                {
                    pu.TestId = test.Id;
                    SaveProductUsage(pu, tr);
                }
            }
        }

        private void SaveProductUsage(ProductUsage ProductUsage, SqlTransaction tr)
        {
            string sql = "INSERT INTO ProductUsage(ProductId,TestId,Rate) VALUES (@ProductId, @TestId, @Rate) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, tr))
            {
                SetProductUsage(cm, ProductUsage);
                ProductUsage.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        public void UpdateTest(Test test, SqlTransaction tr)
        {
            string sql = "Update Test SET TestingAreaId = @TestingAreaId, InstrumentId = @InstrumentId, TestName=@TestName, ";
            sql += "TestType = @TestType, Description = @Description  where TestId = @TestId ";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, tr))
            {
                DatabaseHelper.InsertInt32Param("@TestId", cm, test.Id);
                SetTest(cm, test);
                cm.ExecuteNonQuery();
                UpdateProductUsage(test, tr);
            }
        }

        private void UpdateProductUsage(Test test, SqlTransaction tr)
        {
            string sql = "Update ProductUsage SET ProductId = @ProductId, TestId=@TestId, Rate=@Rate  where Id = @Id";
            string sqlDelete = "Delete ProductUsage where Id = @Id";

            foreach (ProductUsage pu in test.ProductUsages)
            {
                if (pu.IsDirty && pu.Id > 0)
                {
                    using (SqlCommand cm = new SqlCommand(sqlDelete, DefaultConnection, tr))
                    {
                        DatabaseHelper.InsertInt32Param("@Id", cm, pu.Id);
                        cm.ExecuteNonQuery();
                    }
                }
                else if (!pu.IsDirty && !pu.IsNew())
                {
                    using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, tr))
                    {
                        DatabaseHelper.InsertInt32Param("@Id", cm, pu.Id);
                        SetProductUsage(cm, pu);
                        cm.ExecuteNonQuery();
                    }
                }
                else if (!pu.IsDirty && pu.IsNew())
                {
                    pu.TestId = test.Id;
                    SaveProductUsage(pu, tr);
                }

            }
        }

        public void DeleteTest(Test test)
        {
            string sql = "DELETE FROM Test  WHERE TestId = @TestId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@TestId", cm, test.Id);
                cm.ExecuteNonQuery();
            }
        }

        public IList<TestInfo> GetListOfTestInfo(string filiter)
        {
            string sql = "SELECT TestId, InsturmentId, TestName, TestType, Description, ";
            sql += "(select InstrumentName from Instrument where InstrumentId = Test.InstrumentId) as InsName, ";
            sql += "(select AreaName from TestingArea where TestingAreaId = Test.TestingAreaId) as AreaName FROM Test ";

            if (filiter != null)
                sql += filiter;
            
            IList<TestInfo> lstTest = new List<TestInfo>();

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
                                lstTest.Add(FetchTestInfo(dr));
                            }
                        }
                    }
                }
            }

            return lstTest;
        }

        private TestInfo FetchTestInfo(SqlDataReader dr)
        {
            TestInfo t = new TestInfo()
            {
                Id = DatabaseHelper.GetInt32("TestId", dr),
                TestName = DatabaseHelper.GetString("TestName", dr),
                TestType = DatabaseHelper.GetString("TestType", dr),
                Description = DatabaseHelper.GetString("Description", dr),
                InstrumentName = DatabaseHelper.GetString("InsName", dr),
                TestingAreaName = DatabaseHelper.GetString("AreaName", dr),
                InsturmentId = DatabaseHelper.GetInt32("InsturmentId", dr)
            };
            return t;
        }

        public IList<TestDone> GetTestDonebyFacility(int facilityid, int testId, string dateFiliter)
        {
            string sql = "SELECT FacilityId, InstrumentId, Id, TestId, TestType, NoTestDone, Month, Year FROM TestDone ";
            sql += String.Format("WHERE FacilityId = {0} AND TestId = {1} AND {3} ORDER BY Year, Month ", facilityid, testId, dateFiliter);

            IList<TestDone> lstTest = new List<TestDone>();

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
                                lstTest.Add(FetchTestDone(dr));
                            }
                        }
                    }
                }
            }

            return lstTest;
        }

        private TestDone FetchTestDone(SqlDataReader dr)
        {
            TestDone t = new TestDone()
            {
                Id = DatabaseHelper.GetInt32("Id", dr),
                TestId = DatabaseHelper.GetInt32("TestId", dr),
                TestType = DatabaseHelper.GetString("TestType", dr),
                FacilityId = DatabaseHelper.GetInt32("FacilityId", dr),
                InstrumentId = DatabaseHelper.GetInt32("InstrumentId", dr),
                NoTestDone = DatabaseHelper.GetInt32("NoTestDone", dr),
                Year = DatabaseHelper.GetInt32("Year", dr),
                Month = DatabaseHelper.GetInt32("Month", dr)
                
            };
            return t;
        }

        public void SaveTestDone(TestDone test, SqlTransaction tr)
        {
            string sql = "INSERT INTO TestDone([FacilityId], [InstrumentId], [TestId],[TestType],[NoTestDone],[Month],[Year])";
            sql += "VALUES(@FacilityId, @InstrumentId, @TestId, @TestType, @NoTestDone, @Month, @Year) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, tr))
            {
                SetTestDone(cm, test);
                test.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        private void SetTestDone(SqlCommand cm, TestDone test)
        {
            DatabaseHelper.InsertInt32Param("@FacilityId", cm, test.FacilityId);
            DatabaseHelper.InsertInt32Param("@InstrumentId", cm, test.InstrumentId);
            DatabaseHelper.InsertInt32Param("@TestId", cm, test.TestId);
            DatabaseHelper.InsertStringNVarCharParam("@TestType", cm, test.TestType);
            DatabaseHelper.InsertInt32Param("@NoTestDone", cm, test.NoTestDone);            
            DatabaseHelper.InsertInt32Param("@Month", cm, test.Month);
            DatabaseHelper.InsertInt32Param("@Year", cm, test.Year);
        }

        public void ExcPimaTestDoneCounter(SqlTransaction tr)
        {
            string sql = "PimaTestDoneCounter";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, tr))
            {
                cm.CommandType = System.Data.CommandType.StoredProcedure;
                cm.ExecuteNonQuery();
            }
        }
    }
}
