using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain;

namespace Chai.ZADS.DataAccess
{
    public class UserDao: BaseDao
    {
        public User GetUserById(int userid)
        {
            string sql = "SELECT UserId,UserName, Password,FirstName, LastName,Email,IsActive,LastLogin, LastIp,"
            + " DateCreated, DateModified FROM AppUser WHERE UserId = @UserId "
            + " SELECT  UserRoleId, u.RoleId as Role_Id, PermissionLevel, Name FROM UserRole AS u INNER JOIN Role AS r "
            + " ON u.RoleId = r.RoleId WHERE  (u.UserId = @UserId)";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@UserId", cm, userid);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                return GetUser(dr);
                            }
                        }
                    }
                }
            }
            return null;
        }

        public User GetUserByUsernameAndPassword(string username, string hashedPassword)
        {
            string sql = "SELECT UserId	FROM AppUser WHERE (UserName = @UserName) AND (Password = @Password)";
            int userid = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertStringNVarCharParam("@UserName", cm, username);
                    DatabaseHelper.InsertStringNVarCharParam("@Password", cm, hashedPassword);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                userid = DatabaseHelper.GetInt32("UserId", dr);
                            }
                        }
                    }
                }
            }
            return GetUserById(userid);
        }

        public int? GetUserIdByUsername(string username)
        {
            string sql = "SELECT UserId	FROM AppUser WHERE (UserName = @UserName)";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertStringNVarCharParam("@UserName", cm, username);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                return DatabaseHelper.GetInt32("UserId", dr);
                            }
                        }
                    }
                }
            }
            return null;
        }
        private static User GetUser(SqlDataReader dr)
        {
            User user = new User
            {
                Id = DatabaseHelper.GetInt32("UserId", dr),
                UserName = DatabaseHelper.GetString("UserName", dr),
                Password = DatabaseHelper.GetString("Password", dr),
                FirstName = DatabaseHelper.GetString("FirstName", dr),
                LastName = DatabaseHelper.GetString("LastName", dr),
                Email = DatabaseHelper.GetString("Email", dr),
                IsActive = DatabaseHelper.GetBoolean("IsActive", dr),
                LastLogin = DatabaseHelper.GetNullAuthorizedDateTime("LastLogin", dr),
                LastIp = DatabaseHelper.GetString("LastIp", dr),
                DateCreated = DatabaseHelper.GetDateTime("DateCreated", dr),
                DateModified = DatabaseHelper.GetDateTime("DateModified", dr)
            };

            dr.NextResult();
            while(dr.Read())
            {
                UserRole ur = new UserRole { Id = DatabaseHelper.GetInt32("UserRoleId", dr), UserId = user.Id };
                ur.Role = new Role
                {
                    Id = DatabaseHelper.GetInt32("Role_Id", dr),
                    Name = DatabaseHelper.GetString("Name", dr),
                    PermissionLevel = DatabaseHelper.GetInt32("PermissionLevel", dr)
                };
                user.UserRoles.Add(ur);
            }
            return user;
        }

        private static void SetUser(SqlCommand cm, User user)
        {            
            DatabaseHelper.InsertStringNVarCharParam("@Password", cm, user.Password);
            DatabaseHelper.InsertStringNVarCharParam("@FirstName", cm, user.FirstName);
            DatabaseHelper.InsertStringNVarCharParam("@LastName", cm, user.LastName);
            DatabaseHelper.InsertStringNVarCharParam("@Email", cm, user.Email);
            DatabaseHelper.InsertBooleanParam("@IsActive", cm, user.IsActive);
            DatabaseHelper.InsertDateTimeParam("@LastLogin", cm, user.LastLogin);
            DatabaseHelper.InsertStringNVarCharParam("@LastIp", cm, user.LastIp);            
            DatabaseHelper.InsertDateTimeParam("@DateModified", cm, user.DateModified);
        }

        public int Save(User user)
        {
            string sql = @"INSERT INTO AppUser(UserName, Password, FirstName, LastName, Email, IsActive, LastLogin, LastIp, DateCreated, DateModified)"
            + " VALUES (@UserName, @Password, @FirstName, @LastName, @Email, @IsActive, @LastLogin, @LastIp, @DateCreated, @DateModified)"
            + " SELECT @@identity";
            
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertStringNVarCharParam("@UserName", cm, user.UserName);
                DatabaseHelper.InsertDateTimeParam("@DateCreated", cm, user.DateCreated);
                SetUser(cm, user);
                user.Id = int.Parse(cm.ExecuteScalar().ToString());
                foreach (UserRole ur in user.UserRoles)
                {
                    ur.UserId = user.Id;
                    SaveUserRole(ur);
                }
            }
            return user.Id;
        }

        private void SaveUserRole(UserRole ur)
        {
            string sql = "INSERT INTO UserRole(UserId, RoleId) VALUES (@UserId, @RoleId) SELECT @@identity";
            
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@RoleId", cm, ur.Role.Id);
                DatabaseHelper.InsertInt32Param("@UserId", cm, ur.UserId);
                ur.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        public void Update(User user)
        {
            string sql = "UPDATE AppUser SET	Password = @Password, FirstName = @FirstName, LastName = @LastName, "
            + " Email = @Email,	IsActive = @IsActive, LastLogin = @LastLogin, LastIp = @LastIp,"
            + " DateModified = @DateModified WHERE	UserId = @UserId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@UserId", cm, user.Id);
                SetUser(cm, user);
                cm.ExecuteNonQuery();
                UpdateUserRole(user);
            }
        }

        private void UpdateUserRole(User user)
        {
            string sql = "DELETE FROM UserRole WHERE UserRoleId = @UserRoleId";

            foreach (UserRole ur in user.UserRoles)
            {
                if (ur.IsDirty && ur.Id > 0)
                {
                    using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
                    {
                        DatabaseHelper.InsertInt32Param("@UserRoleId", cm, ur.Id);
                    }
                }
                else if (ur.IsNew() && !ur.IsDirty)
                {
                    SaveUserRole(ur);
                }                
            }
        }

        public void Delete(User user)
        {
            string sql = "DELETE FROM AppUser WHERE UserId = @UserId";
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@UserId", cm, user.Id);
                cm.ExecuteNonQuery();
            }
        }

        public IList<User> GetListOfAllUsers()
        {
            string sql = "SELECT UserId FROM AppUser";
            IList<User> lstuser = new List<User>();
            IList<int> userids = new List<int>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                            userids.Add(DatabaseHelper.GetInt32("UserId", dr));
                    }
                }
            }

            foreach (int id in userids)
            {
                lstuser.Add(GetUserById(id));
            }
            return lstuser;
        }

        public IList<User> SearchUsers(string username)
        {
            if (string.IsNullOrEmpty(username))
                return GetListOfAllUsers();

            string sql = String.Format("SELECT UserId FROM AppUser WHERE UserName like '{0}'", username + "%");
            IList<User> lstuser = new List<User>();
            IList<int> userids = new List<int>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                            userids.Add(DatabaseHelper.GetInt32("UserId", dr));
                    }
                }
            }

            foreach (int id in userids)
            {
                lstuser.Add(GetUserById(id));
            }
            return lstuser;
        }
    }
}
