
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.POC.CoreDomain.Resource;
namespace Chai.POC.DataAccess.Resource
{
    public class GroupDao : BaseDao
    {
        public TestingGroup GetGroupById(int groupId)
        {
            string sql = "SELECT TestingGroup.*, TestingArea.TestingAreaName FROM TestingGroup  LEFT JOIN TestingArea ON TestingArea.Id = TestingGroup.TestingAreaId where TestingGroup.Id = @groupId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@groupId", cm, groupId);

                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr != null)
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            return GetGroup(dr);
                        }
                    }
                }
            }
            return null;
        }

        private static TestingGroup GetGroup(SqlDataReader dr)
        {
            TestingGroup group = new TestingGroup
            {
                Id = DatabaseHelper.GetInt32("Id", dr),
                TestingAreaId = DatabaseHelper.GetInt32("TestingAreaId", dr),
                GroupName = DatabaseHelper.GetString("GroupName", dr),
                Description = DatabaseHelper.GetString("Description", dr),
                TestingAreaName = DatabaseHelper.GetString("TestingAreaName", dr)
            };

            return group;
        }

        private static void SetGroup(SqlCommand cm, TestingGroup group)
        {
            DatabaseHelper.InsertInt32Param("@TestingAreaId", cm, group.TestingAreaId);
            DatabaseHelper.InsertStringNVarCharParam("@GroupName", cm, group.GroupName);
            DatabaseHelper.InsertStringNVarCharParam("@Description", cm, group.Description);

        }

        public void Save(TestingGroup group)
        {
            string sql = "INSERT INTO TestingGroup(TestingAreaId, GroupName, Description) VALUES (@TestingAreaId, @GroupName, @Description) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                SetGroup(cm, group);
                group.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        public void Update(TestingGroup group)
        {
            string sql = "Update TestingGroup SET TestingAreaId =@TestingAreaId, GroupName =@GroupName, Description=@Description  where Id = @groupId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@groupId", cm, group.Id);
                SetGroup(cm, group);
                cm.ExecuteNonQuery();
            }
        }

        public void Delete(int groupId)
        {
            string sql = "Delete TestingGroup where Id = @groupId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@groupId", cm, groupId);
                cm.ExecuteNonQuery();
            }
        }

        public IList<TestingGroup> GetListOfGroup(string groupName, int testingAreaId)
        {
            string sql;
            sql = "SELECT TestingGroup.*, TestingArea.TestingAreaName FROM TestingGroup LEFT JOIN TestingArea ON TestingArea.Id = TestingGroup.TestingAreaId WHERE " +
                  " 1 = Case When @testingAreaId = 0 Then 1  When @testingAreaId = TestingGroup.TestingAreaId Then 1  END AND " +
                  " 1 = Case When @groupName = '' Then 1  When TestingGroup.GroupName like '%' + @groupName + '%' Then 1 END  order by TestingGroup.GroupName";


            IList<TestingGroup> lstGroup = new List<TestingGroup>();

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {

                DatabaseHelper.InsertStringVarCharParam("@groupName", cm, groupName);
                DatabaseHelper.InsertInt32Param("@testingAreaId", cm, testingAreaId);

                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            lstGroup.Add(GetGroup(dr));
                        }
                    }
                }
            }
            return lstGroup;
        }
    }
}
