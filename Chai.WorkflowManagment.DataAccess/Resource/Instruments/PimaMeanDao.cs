using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain.Resource;
using System.Collections;

namespace Chai.ZADS.DataAccess.Resource
{
    public class PimaMeanDao : BaseDao
    {
        public PimaMean GetPimaMeanByDeviceId(int id)
        {
            string sql = "SELECT  [DeviceId], [Id], [Mean], [Barcode], [ExpiryDate] FROM PimaMean where DeviceId = @DeviceId";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@DeviceId", cm, id);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                return GetPimaMean(dr);
                            }
                        }
                    }
                }
            }

            return null;
        }

        private PimaMean GetPimaMean(SqlDataReader dr)
        {
            PimaMean pimaMean = new PimaMean
            {
                Id = DatabaseHelper.GetInt32("Id", dr),
                Barcode = DatabaseHelper.GetString("Barcode", dr),
                ExpiryDate = DatabaseHelper.GetDateTime("ExpiryDate", dr),
                Mean = DatabaseHelper.GetDouble("Mean", dr),
                DeviceId = DatabaseHelper.GetInt32("DeviceId", dr),
            };

            return pimaMean;
        }

        private static void SetPimaMean(SqlCommand cm, PimaMean pimaMean)
        {
            DatabaseHelper.InsertStringNVarCharParam("@Barcode", cm, pimaMean.Barcode);
            DatabaseHelper.InsertDateTimeParam("@ExpiryDate", cm, pimaMean.ExpiryDate);
            DatabaseHelper.InsertDoubleParam("@Mean", cm, pimaMean.Mean);
            DatabaseHelper.InsertInt32Param("@DeviceId", cm, pimaMean.DeviceId);
        }

        public void Save(PimaMean pimaMean)
        {
            string sql = "INSERT INTO PimaMean ([DeviceId], [Mean], [Barcode], [ExpiryDate]) VALUES (@DeviceId, @Mean, @Barcode, @ExpiryDate) SELECT @@identity";
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                SetPimaMean(cm, pimaMean);
                pimaMean.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        public void Update(PimaMean pimaMean)
        {
            string sql = "UPDATE PimaMean SET";
            sql += " [Barcode] = @Barcode,";
            sql += " [ExpiryDate] = @ExpiryDate,";
            sql += " [Mean] = @Mean,";
            sql += " [DeviceId] = @DeviceId";
            sql += " WHERE Id = @Id";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@Id", cm, pimaMean.Id);
                SetPimaMean(cm, pimaMean);
                cm.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            string sql = "Delete PimaMean where Id = @Id";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@Id", cm, id);
                    cm.ExecuteNonQuery();
                }
            }
        }
    }
}
