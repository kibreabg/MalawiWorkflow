using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain;
using Chai.ZADS.Enums;

namespace Chai.ZADS.DataAccess.Admin
{
    public class TabDao :BaseDao
    {
        private NodeDao _nodeDao;
        private TaskPanDao _taskpanDao;

        public TabDao()
        {
            _nodeDao = new NodeDao();
            _taskpanDao = new TaskPanDao();
        }

        public Tab GetTabById(int tabid)
        {
            string sql = "SELECT [ModuleId], [TabId], [TabName], [Position], [Description] FROM Tab where TabId = @TabId  "
                + "SELECT ModuleId, Name, FolderPath FROM PocModule where ModuleId = (SELECT ModuleId from Tab where TabId = @TabId) "
                + "SELECT [TabId], [NodeId], [Id], [Position] FROM PopupMenu where TabId = @TabId order by Position "
                + "SELECT TabRoleId, ViewAllowed, Role.RoleId as role_id, Name, PermissionLevel "
                + "FROM TabRole INNER JOIN Role ON TabRole.RoleId = Role.RoleId WHERE TabRole.TabId = @TabId";

            Tab tab = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@TabId", cm, tabid);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                tab = ReadTab(dr);
                                dr.NextResult();
                                ReadPocModule(dr, tab);
                                dr.NextResult();
                                ReadPopupMenus(dr, tab);
                                dr.NextResult();
                                ReadTabPermissions(dr, tab);
                            }
                        }
                    }
                }
            }
            if(tab != null)
                tab.TaskPans = _taskpanDao.GetListOfAllTaskPans(tab.Id);

            return tab;
        }

        private Tab ReadTab(SqlDataReader dr)
        {
            Tab tab = new Tab
            {
                Id = DatabaseHelper.GetInt32("TabId", dr),
                TabName = DatabaseHelper.GetString("TabName", dr),
                Position = DatabaseHelper.GetInt32("Position", dr),
                Description = DatabaseHelper.GetString("Description", dr),
            };

            return tab;
        }
        private void ReadPocModule(SqlDataReader dr, Tab tab)
        {
            dr.Read();
            tab.PocModule = new PocModule(DatabaseHelper.GetString("Name", dr), DatabaseHelper.GetString("FolderPath", dr)) { Id = DatabaseHelper.GetInt32("ModuleId", dr) };
        }

        private void ReadPopupMenus(SqlDataReader dr, Tab tab)
        {
            while (dr.Read())
            {
                PopupMenu tp = new PopupMenu
                {
                    Id = DatabaseHelper.GetInt32("Id", dr),
                    TabId = DatabaseHelper.GetInt32("TabId", dr),
                    Position = DatabaseHelper.GetInt32("Position", dr)
                };

                tp.Node = _nodeDao.GetNodeById(DatabaseHelper.GetInt32("NodeId", dr));

                tab.PopupMenus.Add(tp);
            }
        }

        private void ReadTabPermissions(SqlDataReader dr, Tab tab)
        {
            while (dr.Read())
            {
                TabRole  tr = new TabRole
                {
                    Id = DatabaseHelper.GetInt32("TabRoleId", dr),
                    TabId = tab.Id,
                    ViewAllowed = DatabaseHelper.GetBoolean("ViewAllowed", dr)
                };

                tr.Role = new Role
                {
                    Id = DatabaseHelper.GetInt32("role_id", dr),
                    Name = DatabaseHelper.GetString("Name", dr),
                    PermissionLevel = DatabaseHelper.GetInt32("PermissionLevel", dr)
                };

                tab.TabRoles.Add(tr);
            }

        }
        private static void SetTab(SqlCommand cm, Tab tab)
        {
            DatabaseHelper.InsertStringNVarCharParam("@TabName", cm, tab.TabName);
            DatabaseHelper.InsertInt32Param("@Position", cm, tab.Position);
            DatabaseHelper.InsertStringNVarCharParam("@Description", cm, tab.Description);
        }

        public void Save(Tab tab, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO Tab([ModuleId], [TabName], [Position], [Description]) "
                        +"VALUES(@ModuleId, @TabName, @Position, @Description)  SELECT @@identity";

            tab.Position = GetMaxTabPosition();

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@ModuleId", cm, tab.PocModule.Id);
                SetTab(cm, tab);
                tab.Id = int.Parse(cm.ExecuteScalar().ToString());

                foreach (PopupMenu pm in tab.PopupMenus)
                {
                    pm.TabId = tab.Id;
                    SavePopupMenu(pm, sqltransaction);
                }

                foreach (TabRole tr in tab.TabRoles)
                {
                    tr.TabId = tab.Id;
                    SaveTabRole(tr, sqltransaction);
                }
            }
        }

        private void SavePopupMenu(PopupMenu pm, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO PopupMenu([TabId],[NodeId],[Position]) "
                        +"VALUES(@TabId,@NodeId,@Position) SELECT @@identity";
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                SetPopupMenu(cm, pm);
                pm.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        private void SetPopupMenu(SqlCommand cm, PopupMenu np)
        {
            DatabaseHelper.InsertInt32Param("@TabId", cm, np.TabId);
            DatabaseHelper.InsertInt32Param("@NodeId", cm, np.Node.Id);
            DatabaseHelper.InsertInt32Param("@Position", cm, np.Position);
        }
      
        private void SaveTabRole(TabRole  tr, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO TabRole ([TabId] ,[RoleId],[ViewAllowed]) VALUES(@TabId, @RoleId, @ViewAllowed) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                SetTabRole(cm, tr);
                tr.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        private void SetTabRole(SqlCommand cm, TabRole tr)
        {
            DatabaseHelper.InsertInt32Param("@TabId", cm, tr.TabId);
            DatabaseHelper.InsertInt32Param("@RoleId", cm, tr.Role.Id);
            DatabaseHelper.InsertBooleanParam("@ViewAllowed", cm, tr.ViewAllowed);
        }

        public void Update(Tab tab, SqlTransaction sqltransaction)
        {
            string sql = "UPDATE Tab  SET [TabName] = @TabName, [Position] = @Position, [Description] = @Description WHERE TabId = @Id";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@Id", cm, tab.Id);
                SetTab(cm, tab);
                cm.ExecuteNonQuery();
                UpdatePopupMenus(tab, sqltransaction);
                UpdateTabRoles(tab, sqltransaction);
            }
        }
        
        private void UpdatePopupMenus(Tab tab, SqlTransaction sqltransaction)
        {
            string sql = "DELETE FROM PopupMenu WHERE Id = @Id";

            foreach (PopupMenu np in tab.PopupMenus)
            {
                if (np.IsDirty && np.Id > 0)
                {
                    using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
                    {
                        DatabaseHelper.InsertInt32Param("@Id", cm, np.Id);
                        cm.ExecuteNonQuery();
                    }
                }
                else if (np.IsNew() && !np.IsDirty)
                {
                    np.TabId = tab.Id;
                    SavePopupMenu(np, sqltransaction);
                }
            }
        }

        private void UpdateTabRoles(Tab tab, SqlTransaction sqltransaction)
        {
            string sql = "DELETE FROM TabRole WHERE TabRoleId = @Id";

            foreach (TabRole tr in tab.TabRoles)
            {
                if (tr.IsDirty && tr.Id > 0)
                {
                    using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
                    {
                        DatabaseHelper.InsertInt32Param("@Id", cm, tr.Id);
                        cm.ExecuteNonQuery();
                    }
                }
                else if (tr.IsNew() && !tr.IsDirty)
                {
                    tr.TabId = tab.Id;
                    SaveTabRole(tr, sqltransaction);
                }
            }
        }

        public void Delete(Tab tab, SqlTransaction sqltransaction)
        {
            string sql = "Delete Tab where TabId = @Id";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@Id", cm, tab.Id);
                cm.ExecuteNonQuery();
            }
        }
              
        public IList<Tab> GetListOfAllTabs()
        {
            IList<Tab> lstnode = new List<Tab>();
            string sql = "SELECT [ModuleId], [TabId], [TabName], [Position], [Description] FROM Tab Order by Position";

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
                                lstnode.Add(ReadTab(dr));
                            }
                        }
                    }
                }
            }

            foreach (Tab t in lstnode)
            {
                GetOtherTabClass(t);
                t.TaskPans = _taskpanDao.GetListOfAllTaskPans(t.Id);
            }
            
            return lstnode;
        }

        private void GetOtherTabClass(Tab tab)
        {
            string sql = "SELECT ModuleId, Name, FolderPath FROM PocModule where ModuleId = (SELECT ModuleId from Tab where TabId = @TabId) "
                + "SELECT [TabId], [NodeId], [Id], [Position] FROM PopupMenu where TabId = @TabId order by Position "
                + "SELECT TabRoleId, ViewAllowed, Role.RoleId as role_id, Name, PermissionLevel "
                + "FROM TabRole INNER JOIN Role ON TabRole.RoleId = Role.RoleId WHERE TabRole.TabId = @TabId";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@TabId", cm, tab.Id);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            ReadPocModule(dr, tab);
                            dr.NextResult();
                            ReadPopupMenus(dr, tab);
                            dr.NextResult();
                            ReadTabPermissions(dr, tab);
                        }
                    }
                }
            }
        }

        private int GetMaxTabPosition()
        {
            string sql = "SELECT MAX(Position) as MP FROM Tab";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    object obj = cm.ExecuteScalar();
                    if (!(obj is  DBNull))
                        return Convert.ToInt32(obj);
                }
            }
            return 1;
        }

        public void MoveUp(Tab tab)
        {
            SortTabPosition(tab, 1);
        }

        public void MoveDown(Tab tab)
        {
            SortTabPosition(tab, 2);
        }

        private void SortTabPosition(Tab tab, int direction)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand("UpdateTabPosition", con))
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    DatabaseHelper.InsertInt32Param("@TabId", cm, tab.Id);
                    DatabaseHelper.InsertInt32Param("@Direction", cm, direction); //UP = 1 , Down = 2
                    cm.ExecuteNonQuery();
                }
            }
        }

    }
}
