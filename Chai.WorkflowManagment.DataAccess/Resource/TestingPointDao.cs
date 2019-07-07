
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain.Resource;
namespace Chai.ZADS.DataAccess.Resource
{
    public class TestingPointDao : BaseDao
    {
        public TestingPointType GetTestingPointById(int testingPointId)
        {
            string sql = "SELECT * FROM TestingPointType where Id = @testingPointId";

            
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@testingPointId", cm, testingPointId);

                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr != null)
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            return GetTestingPoint(dr);
                        }
                    }
                }
            }
            return null;
        }

        private static TestingPointType GetTestingPoint(SqlDataReader dr)
        {
            TestingPointType testingPoint = new TestingPointType
            {
                Id = DatabaseHelper.GetInt32("Id", dr),
                Name = DatabaseHelper.GetString("TypeName", dr),
                Description = DatabaseHelper.GetString("Description", dr),

            };

            return testingPoint;
        }

        private static void SetTestingPoint(SqlCommand cm, TestingPointType testingPoint)
        {
            DatabaseHelper.InsertStringNVarCharParam("@TypeName", cm, testingPoint.Name);
            DatabaseHelper.InsertStringNVarCharParam("@Description", cm, testingPoint.Description);

        }

        public void Save(TestingPointType testingPoint)
        {
            string sql = "INSERT INTO TestingPointType (TypeName, Description) VALUES (@TypeName, @Description) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                SetTestingPoint(cm, testingPoint);
                testingPoint.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        public void Update(TestingPointType testingPoint)
        {
            string sql = "Update TestingPointType SET TypeName = @TypeName, Description = @Description  where Id = @testingPointId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@testingPointId", cm, testingPoint.Id);
                SetTestingPoint(cm, testingPoint);
                cm.ExecuteNonQuery();
            }
        }

        public void Delete(int testingPointId)
        {
            string sql = "Delete TestingPointType where Id = @testingPointId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@testingPointId", cm, testingPointId);
                cm.ExecuteNonQuery();
            }
        }

        public IList<TestingPointType> GetListOfTestingPoint(string value)
        {
            string sql = "SELECT * FROM TestingPointType ";
            
            IList<TestingPointType> lstTestingPoint = new List<TestingPointType>();

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {

                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            lstTestingPoint.Add(GetTestingPoint(dr));
                        }
                    }
                }
            }
            return lstTestingPoint;
        }
    }
}
