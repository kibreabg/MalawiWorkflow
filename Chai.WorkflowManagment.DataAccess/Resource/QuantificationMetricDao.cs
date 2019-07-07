using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain.Resource;
using Chai.ZADS.Enums;
namespace Chai.ZADS.DataAccess.Resource
{
    public class QuantificationMetricDao : BaseDao
    {

        public QuantificationMetric GetQuantificationMetricById(int id)
        {
            string sql = "SELECT q.ProductId, q.Id, q.QuantifyMenu, q.UsageRate, p.ProductName, p.BasicUnit, p.PackSize ";
            sql += "FROM QuantificationMetric AS q INNER JOIN Product AS p ON q.ProductId = p.ProductID  where q.Id = @Id";
            
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@Id", cm, id);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                return FetchQuantificationMetric(dr);
                            }
                        }
                    }
                }
            }
            return null;
        }

        private QuantificationMetric FetchQuantificationMetric(SqlDataReader dr)
        {
            QuantificationMetric t = new QuantificationMetric()
            {
                Id = DatabaseHelper.GetInt32("Id", dr),
                ProductId = DatabaseHelper.GetInt32("ProductId", dr),
                QuantifyMenu = (GeneralQuantifyMenu)Enum.Parse(typeof(GeneralQuantifyMenu), DatabaseHelper.GetString("QuantifyMenu", dr)),
                UsageRate = DatabaseHelper.GetDouble("UsageRate", dr),
                ProductName = DatabaseHelper.GetString("ProductName", dr),
                BasicUnit = DatabaseHelper.GetString("BasicUnit", dr),
                PackSize = DatabaseHelper.GetInt32("PackSize", dr)
            };
            return t;
        }

        private void SetQuantificationMetric(SqlCommand cm, QuantificationMetric q)
        {
            DatabaseHelper.InsertInt32Param("@ProductId", cm, q.ProductId);
            DatabaseHelper.InsertStringNVarCharParam("@QuantifyMenu", cm, q.QuantifyMenu.ToString());
            DatabaseHelper.InsertDoubleParam("@UsageRate", cm, q.UsageRate);
        }

        public void Save(QuantificationMetric qm, SqlTransaction tr)
        {
            string sql = "INSERT INTO QuantificationMetric(ProductId, QuantifyMenu, UsageRate) VALUES (@ProductId, @QuantifyMenu, @UsageRate) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, tr))
            {
                SetQuantificationMetric(cm, qm);
                qm.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }


        public void Update(QuantificationMetric qm, SqlTransaction tr)
        {
            string sql = "Update QuantificationMetric SET ProductId = @ProductId, QuantifyMenu = @QuantifyMenu, UsageRate = @UsageRate  where Id = @Id";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, tr))
            {
                DatabaseHelper.InsertInt32Param("@Id", cm, qm.Id);
                SetQuantificationMetric(cm, qm);
                cm.ExecuteNonQuery();
            }
        }

        public void Delete(int qmetricId)
        {
            string sql = "DELETE FROM QuantificationMetric  WHERE Id = @Id";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@Id", cm, qmetricId);
                    cm.ExecuteNonQuery();
                }
            }
        }

        public IList<QuantificationMetric> GetListOfQuantificationMetric()
        {
            string sql = "SELECT q.ProductId, q.Id, q.QuantifyMenu, q.UsageRate, p.ProductName, p.BasicUnit, p.PackSize ";
            sql += "FROM QuantificationMetric AS q INNER JOIN Product AS p ON q.ProductId = p.ProductID ";

            IList<QuantificationMetric> lstTest = new List<QuantificationMetric>();

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
                                lstTest.Add(FetchQuantificationMetric(dr));
                            }
                        }
                    }
                }
            }

            return lstTest;
        }

    }
}
