using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain;

namespace Chai.ZADS.DataAccess
{
    public class RoleDao : BaseDao
    {
        public Role GetRoleById(int roleid)
        {
            string sql = "SELECT RoleId, Name, PermissionLevel FROM Role where RoleId = @RoleId";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@RoleId", cm, roleid);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                return GetRole(dr);
                            }
                        }
                    }
                }
            }
            return null;
        }

        private static Role GetRole(SqlDataReader dr)
        {
            Role role = new Role
            {
                Id = DatabaseHelper.GetInt32("RoleId", dr),
                Name = DatabaseHelper.GetString("Name", dr),
                PermissionLevel = DatabaseHelper.GetInt32("PermissionLevel", dr)
            };

            return role;
        }
        private static void SetRole(SqlCommand cm, Role role)
        {
            DatabaseHelper.InsertStringNVarCharParam("@Name", cm, role.Name);
            DatabaseHelper.InsertInt32Param("@PermissionLevel", cm, role.PermissionLevel);
        }

        public void Save(Role role)
        {
            string sql = "INSERT INTO Role(Name, PermissionLevel) VALUES (@Name, @PermissionLevel) SELECT @@identity";
            
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                SetRole(cm, role);
                role.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        public void Update(Role role)
        {
            string sql = "Update Role SET Name =@Name , PermissionLevel = @PermissionLevel where RoleId = @RoleId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@RoleId", cm, role.Id);
                SetRole(cm, role);
                cm.ExecuteNonQuery();
            }
        }

        public void Delete(Role role)
        {
            string sql = "Delete Role where RoleId = @RoleId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@RoleId", cm, role.Id);
                cm.ExecuteNonQuery();
            }
        }

        public IList<Role> GetListOfRole()
        {
            string sql = "SELECT RoleId, Name, PermissionLevel FROM Role order by PermissionLevel";
            IList<Role> lstrole = new List<Role>();

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
                                lstrole.Add(GetRole(dr));
                            }
                        }
                    }
                }
            }
            return lstrole;
        }
    }
}
