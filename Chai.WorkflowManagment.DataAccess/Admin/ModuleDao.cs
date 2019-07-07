using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain;
using Chai.ZADS.Enums;

namespace Chai.ZADS.DataAccess.Admin
{
    public class ModuleDao : BaseDao
    {
        public ModuleDao()
        {
        }

        public PocModule GetModuleById(int panid)
        {
            string sql = "SELECT ModuleId, Name, FolderPath FROM PocModule where ModuleId = @ModId  ";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@ModId", cm, panid);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                return ReadPocModule(dr);
                            }
                        }
                    }
                }
            }
            return null;
        }

        private PocModule ReadPocModule(SqlDataReader dr)
        {
            return new PocModule(DatabaseHelper.GetString("Name", dr), DatabaseHelper.GetString("FolderPath", dr)) { Id = DatabaseHelper.GetInt32("ModuleId", dr) };
        }

        private static void SetModule(SqlCommand cm, PocModule mod)
        {
            DatabaseHelper.InsertStringNVarCharParam("@Name", cm, mod.Name);
            DatabaseHelper.InsertStringNVarCharParam("@FolderPath", cm, mod.FolderPath);
        }

        public void Save(PocModule mod, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO PocModule([Name], [FolderPath]) "
                    + "VALUES(@Name,  @FolderPath)  SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                SetModule(cm, mod);
                mod.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        public void Update(PocModule mod, SqlTransaction sqltransaction)
        {
            string sql = "UPDATE PocModule  SET [Name] = @Name,  [FolderPath] = @FolderPath WHERE ModuleId = @ModId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@ModId", cm, mod.Id);
                SetModule(cm, mod);
                cm.ExecuteNonQuery();
            }
        }

        public void Delete(PocModule mod)
        {
            string sql = "Delete PocModule where ModuleId = @ModId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@ModId", cm, mod.Id);
                cm.ExecuteNonQuery();
            }
        }

        public IList<PocModule> GetListOfAllModules()
        {
            IList<PocModule> lstnode = new List<PocModule>();
            string sql = "SELECT [ModuleId], [Name], [FolderPath]  FROM PocModule";

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
                                lstnode.Add(ReadPocModule(dr));
                            }
                        }
                    }
                }
            }
            return lstnode;
        }
    }
}
