using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain;
using Chai.ZADS.Enums;

namespace Chai.ZADS.DataAccess.Admin
{
    public class TaskPanDao : BaseDao
    {
        private NodeDao _nodeDao;

        public TaskPanDao()
        {
            _nodeDao = new NodeDao();
        }

        public TaskPan GetTaskPanById(int panid)
        {
            string sql = "SELECT [TabId], [TaskPanId], [Title], [Position], [ImagePath]  FROM TaskPan where TaskPanId = @TaskPanId  "
                + "SELECT [TaskPanId], [NodeId], [Id], [Position]  FROM TaskPanNode where TaskPanId = @TaskPanId order by Position";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@TaskPanId", cm, panid);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                TaskPan pan = ReadPan(dr);
                                dr.NextResult();
                                ReadPanNodes(dr, pan);
                                return pan;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private TaskPan ReadPan(SqlDataReader dr)
        {
            TaskPan pan = new TaskPan
            {
                Id = DatabaseHelper.GetInt32("TaskPanId", dr),
                Title = DatabaseHelper.GetString("Title", dr),
                Position = DatabaseHelper.GetInt32("Position", dr),
                ImagePath = DatabaseHelper.GetString("ImagePath", dr),
                TabId = DatabaseHelper.GetInt32("TabId", dr),
            };

            return pan;
        }
              
        private void ReadPanNodes(SqlDataReader dr, TaskPan pan)
        {
            while (dr.Read())
            {
                TaskPanNode tp = new TaskPanNode
                {
                    Id = DatabaseHelper.GetInt32("Id", dr),
                    TaskPanId = DatabaseHelper.GetInt32("TaskPanId", dr),
                    Position = DatabaseHelper.GetInt32("Position", dr)
                };

                tp.Node = _nodeDao.GetNodeById(DatabaseHelper.GetInt32("NodeId", dr));

                pan.TaskPanNodes.Add(tp);
            }

        }

        private static void SetTaskPan(SqlCommand cm, TaskPan pan)
        {
            DatabaseHelper.InsertInt32Param("@TabId", cm, pan.TabId);
            DatabaseHelper.InsertStringNVarCharParam("@Title", cm, pan.Title);
            DatabaseHelper.InsertInt32Param("@Position", cm, pan.Position);
            DatabaseHelper.InsertStringNVarCharParam("@ImagePath", cm, pan.ImagePath);            
        }

        public void Save(TaskPan pan, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO TaskPan([TabId], [Title], [Position], [ImagePath]) "
                    + "VALUES(@TabId, @Title, @Position, @ImagePath)  SELECT @@identity";
                        
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                SetTaskPan(cm, pan);
                pan.Id = int.Parse(cm.ExecuteScalar().ToString());

                foreach (TaskPanNode np in pan.TaskPanNodes)
                {
                    np.TaskPanId = pan.Id;
                    SaveTaskPanNode(np, sqltransaction);
                }
            }
        }

        private void SaveTaskPanNode(TaskPanNode np, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO TaskPanNode([TaskPanId],[NodeId],[Position]) "
                       + "VALUES(@TaskPanId, @NodeId, @Position) SELECT @@identity";
                        
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                SetTaskPanNode(cm, np);
                np.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        private void SetTaskPanNode(SqlCommand cm, TaskPanNode np)
        {
            DatabaseHelper.InsertInt32Param("@TaskPanId", cm, np.TaskPanId);
            DatabaseHelper.InsertInt32Param("@NodeId", cm, np.Node.Id);
            DatabaseHelper.InsertInt32Param("@Position", cm, np.Position);
        }

        public void Update(TaskPan pan, SqlTransaction sqltransaction)
        {
            string sql = "UPDATE TaskPan  SET [TabId] = @TabId,  [Title] = @Title ,[Position] = @Position ,[ImagePath] = @ImagePath WHERE TaskPanId = @PanId";
            
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@PanId", cm, pan.Id);
                SetTaskPan(cm, pan);
                cm.ExecuteNonQuery();
                UpdateTaskPanNodes(pan, sqltransaction);
            }
        }

        private void UpdateTaskPanNodes(TaskPan pan, SqlTransaction sqltransaction)
        {
            string sql = "DELETE FROM TaskPanNode WHERE Id = @Id";

            foreach (TaskPanNode np in pan.TaskPanNodes)
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
                    np.TaskPanId = pan.Id;
                    SaveTaskPanNode(np, sqltransaction);
                }
            }
        }

        public void Delete(TaskPan pan, SqlTransaction sqltransaction)
        {
            string sql = "Delete TaskPan where TaskPanId = @PanId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@PanId", cm, pan.Id);
                cm.ExecuteNonQuery();
            }
        }

        public IList<TaskPan> GetListOfAllTaskPans(int tabid)
        {
            IList<TaskPan> lstnode = new List<TaskPan>();
            string sql = String.Format("SELECT [TabId], [TaskPanId], [Title], [Position], [ImagePath]  FROM TaskPan where TabId = {0} Order by Position", tabid);

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
                                lstnode.Add(ReadPan(dr));
                            }
                        }
                    }
                }
            }

            foreach (TaskPan pan in lstnode)
            {
                GetTaskPanNodes(pan);
            }


            return lstnode;
        }

        private void GetTaskPanNodes(TaskPan pan)
        {
            string sql = "SELECT [TaskPanId], [NodeId], [Id], [Position]  FROM TaskPanNode where TaskPanId = @TaskPanId order by Position";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@TaskPanId", cm, pan.Id);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            ReadPanNodes(dr, pan);
                        }
                    }
                }
            }
        }

        public void MoveUpPan(int panid)
        {
            SortTabPosition(panid, 1);
        }

        public void MoveDownPan(int panid)
        {
            SortTabPosition(panid, 2);
        }

        private void SortTabPosition(int panid, int direction)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand("UpdateTaskpanPosition", con))
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    DatabaseHelper.InsertInt32Param("@PanId", cm, panid);
                    DatabaseHelper.InsertInt32Param("@Direction", cm, direction); //UP = 1 , Down = 2
                    cm.ExecuteNonQuery();
                }
            }
        }

        public int GetMaxTaskpanPosition()
        {
            string sql = "SELECT MAX(Position) as MP FROM TaskPan";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    object obj = cm.ExecuteScalar();
                    if (!(obj  is DBNull))
                        return Convert.ToInt32(obj) + 1;
                    
                }
            }
            return 1;
        }
       
        public void MoveUpPanNode(int id)
        {
            SortPanNodePosition(id, 1);
        }

        public void MoveDownPanNode(int id)
        {
            SortPanNodePosition(id, 2);
        }

        private void SortPanNodePosition(int id, int direction)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand("UpdateTaskpanNodePosition", con))
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    DatabaseHelper.InsertInt32Param("@PanNodeId", cm, id);
                    DatabaseHelper.InsertInt32Param("@Direction", cm, direction); //UP = 1 , Down = 2
                    cm.ExecuteNonQuery();
                }
            }
        }
    }
}
