using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain;
using Chai.ZADS.Enums;


namespace Chai.ZADS.DataAccess.Admin
{
    public class NodeDao : BaseDao
    {
        public Node GetNodeById(int nodeid)
        {
            string sql = "SELECT ModuleId, NodeId, Title, FilePath, ImagePath, Description, PageId  "
                + "FROM Node where NodeId = @NodeId  "
                + "SELECT ModuleId, Name, FolderPath FROM PocModule where ModuleId = (SELECT ModuleId from Node where NodeId = @NodeId) "
                + "SELECT NodeRoleId, ViewAllowed, EditAllowed, Role.RoleId as role_id, Name, PermissionLevel "
                + "FROM NodeRole INNER JOIN Role ON NodeRole.RoleId = Role.RoleId WHERE NodeRole.NodeId = @NodeId";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@NodeId", cm, nodeid);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                Node node = ReadNode(dr);
                                dr.NextResult();
                                ReadPocModule(dr, node);
                                dr.NextResult();
                                ReadNodePermissions(dr, node);
                                return node;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public Node GetNodeByPageId(string pageid)
        {
            string sql = "SELECT ModuleId, NodeId, Title, FilePath, ImagePath, Description, PageId  "
                + "FROM Node where PageId = @PageId  DECLARE @NodeId INT "
                + "SELECT @NodeId = [NodeId] FROM Node WHERE PageId = @PageId  "
                + "SELECT ModuleId, Name, FolderPath FROM PocModule where ModuleId = (SELECT ModuleId from Node where NodeId = @NodeId) "
                + "SELECT NodeRoleId, ViewAllowed, EditAllowed, Role.RoleId as role_id, Name, PermissionLevel "
                + "FROM NodeRole INNER JOIN Role ON NodeRole.RoleId = Role.RoleId WHERE NodeRole.NodeId = @NodeId";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertStringNVarCharParam("@PageId", cm, pageid);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                Node node = ReadNode(dr);
                                dr.NextResult();
                                ReadPocModule(dr, node);
                                dr.NextResult();
                                ReadNodePermissions(dr, node);
                                return node;
                            }
                        }
                    }
                }
            }

            return null;
        }
        private Node ReadNode(SqlDataReader dr)
        {
            Node node = new Node
            {
                Id = DatabaseHelper.GetInt32("NodeId", dr),
                Title = DatabaseHelper.GetString("Title", dr),
                FilePath = DatabaseHelper.GetString("FilePath", dr),
                ImagePath = DatabaseHelper.GetString("ImagePath", dr),
                Description = DatabaseHelper.GetString("Description", dr),
                PageID = DatabaseHelper.GetString("PageId", dr)
            };
            return node;
        }

        private void ReadPocModule(SqlDataReader dr, Node node)
        {
            dr.Read();
            node.PocModule = new PocModule(DatabaseHelper.GetString("Name", dr), DatabaseHelper.GetString("FolderPath", dr)) { Id = DatabaseHelper.GetInt32("ModuleId", dr) };
        }

        private void ReadNodePermissions(SqlDataReader dr, Node node)
        {
            while (dr.Read())
            {
                NodePermission np = new NodePermission
                {
                    Id = DatabaseHelper.GetInt32("NodeRoleId", dr),
                    NodeId = node.Id,
                    EditAllowed = DatabaseHelper.GetBoolean("EditAllowed", dr),
                    ViewAllowed = DatabaseHelper.GetBoolean("ViewAllowed", dr)
                };

                np.Role = new Role
                {
                    Id = DatabaseHelper.GetInt32("role_id", dr),
                    Name = DatabaseHelper.GetString("Name", dr),
                    PermissionLevel = DatabaseHelper.GetInt32("PermissionLevel", dr)
                };

                node.NodePermissions.Add(np);
            }

        }

        private static void SetNode(SqlCommand cm, Node node)
        {
            DatabaseHelper.InsertInt32Param("@ModuleId", cm, node.PocModule.Id);
            DatabaseHelper.InsertStringNVarCharParam("@Title", cm, node.Title);
            DatabaseHelper.InsertStringNVarCharParam("@FilePath", cm, node.FilePath);
            DatabaseHelper.InsertStringNVarCharParam("@ImagePath", cm, node.ImagePath);
            DatabaseHelper.InsertStringNVarCharParam("@Description", cm, node.Description);
            DatabaseHelper.InsertStringNVarCharParam("@PageId", cm, node.PageID);
        }

        public void Save(Node node, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO Node( [ModuleId], [Title], [FilePath], [ImagePath],  [Description], [PageId]) "
                         + "VALUES( @ModuleId, @Title, @FilePath, @ImagePath, @Description, @PageId) "
                         + "SELECT @@identity";
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                SetNode(cm, node);
                node.Id = int.Parse(cm.ExecuteScalar().ToString());
                foreach (NodePermission np in node.NodePermissions)
                {
                    np.NodeId = node.Id;
                    SaveNodePermission(np, sqltransaction);
                }
            }
        }

        private void SaveNodePermission(NodePermission np, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO [NodeRole]([NodeId], [RoleId], [ViewAllowed], [EditAllowed])"
                    + "VALUES(@NodeId, @RoleId, @ViewAllowed, @EditAllowed) SELECT @@identity";
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                SetNodePermission(cm, np);
                np.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        private void SetNodePermission(SqlCommand cm, NodePermission np)
        {
            DatabaseHelper.InsertInt32Param("@NodeId", cm, np.NodeId);
            DatabaseHelper.InsertInt32Param("@RoleId", cm, np.Role.Id);
            DatabaseHelper.InsertBooleanParam("@ViewAllowed", cm, np.ViewAllowed);
            DatabaseHelper.InsertBooleanParam("@EditAllowed", cm, np.EditAllowed);
        }

        public void Update(Node node, SqlTransaction sqltransaction)
        {
            string sql = "UPDATE [Node] SET [ModuleId] = @ModuleId, [Title] = @Title, [FilePath] = @FilePath, "
            + "[ImagePath] = @ImagePath, [Description] = @Description, PageId = @PageId WHERE NodeId = @NodeId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@NodeId", cm, node.Id);
                SetNode(cm, node);
                cm.ExecuteNonQuery();
                UpdateNodePermission(node, sqltransaction);
            }
        }

        private void UpdateNodePermission(Node node, SqlTransaction sqltransaction)
        {
            string sql = "DELETE FROM NodeRole WHERE NodeRoleId = @NodeRoleId";

            foreach (NodePermission np in node.NodePermissions)
            {
                if (np.IsDirty && np.Id > 0)
                {
                    using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
                    {
                        DatabaseHelper.InsertInt32Param("@NodeRoleId", cm, np.Id);
                        cm.ExecuteNonQuery();
                    }
                }
                else if (np.IsNew() && !np.IsDirty)
                {
                    np.NodeId = node.Id;
                    SaveNodePermission(np, sqltransaction);
                }
            }
        }

        public void Delete(Node node)
        {
            string sql = "Delete Node where NodeId = @NodeId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@NodeId", cm, node.Id);
                cm.ExecuteNonQuery();
            }
        }

        public IList<Node> GetListOfAllNodes()
        {
            return GetListOfNodes("SELECT ModuleId, NodeId, Title, FilePath, ImagePath, Description, PageId  FROM Node");
        }

        public IList<Node> GetListOfNodesByModuleId(int moduleid)
        {
            string sql = String.Format("SELECT ModuleId, NodeId, Title, FilePath, ImagePath, Description, PageId  FROM Node where ModuleId = {0}", moduleid);

            return GetListOfNodes(sql);
        }

        private IList<Node> GetListOfNodes(string sql)
        {
            IList<Node> lstnode = new List<Node>();

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
                                lstnode.Add(ReadNode(dr));
                            }
                        }
                    }
                }
            }
            foreach (Node node in lstnode)
            {
                GetOtherNodeClass(node);
            }


            return lstnode;
        }

        private void GetOtherNodeClass(Node node)
        {
            string sql = "SELECT ModuleId, Name, FolderPath FROM PocModule where ModuleId = (SELECT ModuleId from Node where NodeId = @NodeId) "
               + "SELECT NodeRoleId, ViewAllowed, EditAllowed, Role.RoleId as role_id, Name, PermissionLevel "
               + "FROM NodeRole INNER JOIN Role ON NodeRole.RoleId = Role.RoleId WHERE NodeRole.NodeId = @NodeId";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@NodeId", cm, node.Id);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            ReadPocModule(dr, node);
                            dr.NextResult();
                            ReadNodePermissions(dr, node);
                        }
                    }
                }
            }
        }

    }
}
