
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.ZADS.CoreDomain.Location;
using System.Data.SqlClient;

namespace Chai.ZADS.DataAccess.Location
{
    public class ProvinceDao : BaseDao
    {
        private FacilityDao _facilityDao;

        public ProvinceDao()
        {
            _facilityDao = new FacilityDao();
        }

        public Province GetProvinceById(int provinceid)
        {
            string sql = "SELECT Id, ProvinceName, ProvinceCode,ProvinceShortName,ProvinceMapId FROM Province where Id = @provinceid ";
            sql += "SELECT Id, ProvinceId,DistrictName,Description,DistrictCode,DistrictShortName FROM District  where ProvinceId = @provinceid ";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@provinceid", cm, provinceid);
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                Province pr = FetchProvince(dr);
                                dr.NextResult();
                                FetchDistrict(dr, pr);
                                return pr;
                            }
                        }
                    }
                }
            }

            return null;

        }

        private Province FetchProvince(SqlDataReader dr)
        {
            Province pr = new Province()
            {
                Id = DatabaseHelper.GetInt32("Id", dr),
                ProvinceName = DatabaseHelper.GetString("ProvinceName", dr),
                ProvinceCode = DatabaseHelper.GetString("ProvinceCode", dr),
                ProvinceShortNAme = DatabaseHelper.GetString("ProvinceShortName", dr),
                MapId = DatabaseHelper.GetString("ProvinceMapId", dr)
            };
            return pr;
        }

        private void FetchDistrict(SqlDataReader dr, Province pr)
        {
            while (dr.Read())
            {
                District dst = new District()
                {
                    Id = DatabaseHelper.GetInt32("Id", dr),
                    ProvinceId = DatabaseHelper.GetInt32("ProvinceId", dr),
                    DistrictName = DatabaseHelper.GetString("DistrictName", dr),
                    Description = DatabaseHelper.GetString("Description", dr),
                    ProvinceName = pr.ProvinceName,
                    DistrictCode = DatabaseHelper.GetString("DistrictCode", dr),
                    DistrictShortName = DatabaseHelper.GetString("DistrictShortName", dr)
                };

                dst.Facilitys = _facilityDao.GetListOfFacilityInfo(String.Format(" DistrictId = {0} ", dst.Id));
                pr.Districts.Add(dst);
            }
        }

        private void SetProvince(SqlCommand cm, Province province)
        {
            DatabaseHelper.InsertStringNVarCharParam("@ProvinceName", cm, province.ProvinceName);
            DatabaseHelper.InsertStringNVarCharParam("@ProvinceCode", cm, province.ProvinceCode);
            DatabaseHelper.InsertStringNVarCharParam("@ProvinceShortName", cm, province.ProvinceShortNAme);
            DatabaseHelper.InsertStringNVarCharParam("@ProvinceMapId", cm, province.MapId);
        }

        private void SetDistrict(SqlCommand cm, District district)
        {
            DatabaseHelper.InsertInt32Param("@ProvinceId", cm, district.ProvinceId);
            DatabaseHelper.InsertStringNVarCharParam("@DistrictName", cm, district.DistrictName);
            DatabaseHelper.InsertStringNVarCharParam("@Description", cm, district.Description);
            DatabaseHelper.InsertStringNVarCharParam("@DistrictCode", cm, district.DistrictCode);
            DatabaseHelper.InsertStringNVarCharParam("@DistrictShortName", cm, district.DistrictShortName);
        }

        public void Save(Province province, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO Province(ProvinceName, ProvinceCode,ProvinceShortName,ProvinceMapId) "
            + "VALUES (@ProvinceName,@ProvinceCode,@ProvinceShortName,@ProvinceMapId) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                SetProvince(cm, province);
                province.Id = int.Parse(cm.ExecuteScalar().ToString());

                foreach (District dist in province.Districts)
                {
                    dist.ProvinceId = province.Id;
                    SaveDistrict(dist, sqltransaction);
                }
            }
        }

        private void SaveDistrict(District district, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO District(DistrictName, ProvinceId ,Description,DistrictCode,DistrictShortName) "
                       + "VALUES (@DistrictName,@ProvinceId,@Description,@DistrictCode,@DistrictShortName) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                SetDistrict(cm, district);
                district.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }


        public void Update(Province province, SqlTransaction sqltransaction)
        {
            string sql = "Update Province SET ProvinceName =@ProvinceName ,ProvinceCode =@ProvinceCode,ProvinceShortName=@ProvinceShortName,"
                + "ProvinceMapId=@ProvinceMapId  where Id = @provinceid";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@provinceid", cm, province.Id);
                SetProvince(cm, province);
                cm.ExecuteNonQuery();

                UpdateDistrict(province, sqltransaction);
            }
        }

        private void UpdateDistrict(Province pro, SqlTransaction sqltransaction)
        {
            string sql = "Update District SET DistrictName =@DistrictName ,ProvinceId=@ProvinceId, Description = @Description,"
                       + "DistrictCode = @DistrictCode,DistrictShortName=@DistrictShortName where Id = @districtid";

            string sqlDelete = "Delete District where Id = @districtid";

            foreach (District dl in pro.Districts)
            {
                if (dl.IsDirty && dl.Id > 0)
                {
                    using (SqlCommand cm = new SqlCommand(sqlDelete, DefaultConnection, sqltransaction))
                    {
                        DatabaseHelper.InsertInt32Param("@districtid", cm, dl.Id);
                        cm.ExecuteNonQuery();
                    }
                }
                else if (!dl.IsDirty && !dl.IsNew())
                {
                    using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
                    {
                        DatabaseHelper.InsertInt32Param("@districtid", cm, dl.Id);
                        SetDistrict(cm, dl);
                        cm.ExecuteNonQuery();
                    }
                }
                else if (!dl.IsDirty && dl.IsNew())
                {
                    using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
                    {
                        dl.ProvinceId = pro.Id;
                        SaveDistrict(dl, sqltransaction);
                    }
                }
            }

        }
        public void Delete(Province pro, SqlTransaction sqltransaction)
        {
            string sql = "Delete Province where Id = @id ";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@id", cm, pro.Id);
                cm.ExecuteNonQuery();
            }
        }

        public IList<ProvinceInfo> GetListOfProvinceInfo(string filiter)
        {
            string sql = "SELECT Id, ProvinceName, ProvinceCode,ProvinceShortName,ProvinceMapId FROM Province ";
            if (!string.IsNullOrEmpty(filiter))
                sql += " WHERE " + filiter;

            IList<ProvinceInfo> lstDoc = new List<ProvinceInfo>();

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
                                lstDoc.Add(ReadProvinceInfo(dr));
                            }
                        }
                    }
                }
            }

            return lstDoc;
        }

        private ProvinceInfo ReadProvinceInfo(SqlDataReader dr)
        {
            ProvinceInfo pri = new ProvinceInfo()
            {

                Id = DatabaseHelper.GetInt32("Id", dr),
                ProvinceName = DatabaseHelper.GetString("ProvinceName", dr),
                ProvinceCode = DatabaseHelper.GetString("ProvinceCode", dr),
                ProvinceShortNAme = DatabaseHelper.GetString("ProvinceShortName", dr),
                MapId = DatabaseHelper.GetString("ProvinceMapId", dr)
            };

            return pri;
        }

        public IList GetNumberOfFacilities(int ProvinceId)
        {
            string sql = "";
            sql = "select COUNT(Facility.FacilityId), Province.ProvinceName AS Province " +
                                 " FROM Facility  LEFT JOIN District ON District.Id = Facility.DistrictId " +
                                                " LEFT JOIN Province ON Province.Id = District.ProvinceId " +
                                  "WHERE 1 = Case When @ProvinceId = 0 Then 1  When @ProvinceId = District.ProvinceId Then 1  END " +
                                                " GROUP BY Province.ProvinceName ";

            IList lst = new ArrayList();

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@ProvinceId", cm, ProvinceId);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            object[] values = new object[dr.FieldCount];
                            dr.GetSqlValues(values);
                            lst.Add(values);
                        }

                    }
                }
            }
            return lst;
        }

        public IList GetNumberOfFacilitiesPerFacilityType(int ProvinceId)
        {
            string sql = "";
            sql = "select COUNT(Facility.FacilityId), FacilityType.FacilityTypeName AS FacilityType " +
                                 " FROM Facility  LEFT JOIN FacilityType ON FacilityType.Id = Facility.FacilityTypeId " +
                                                " LEFT JOIN District ON District.Id = Facility.DistrictId " +
                                  "WHERE 1 = Case When @ProvinceId = 0 Then 1  When @ProvinceId = District.ProvinceId Then 1  END " +
                                                "GROUP BY FacilityType.FacilityTypeName ";

            IList lst = new ArrayList();

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@ProvinceId", cm, ProvinceId);
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            object[] values = new object[dr.FieldCount];
                            dr.GetSqlValues(values);
                            lst.Add(values);
                        }

                    }
                }
            }
            return lst;
        }

        public IList GetPercentageOfFacilityType(int ProvinceId)
        {
            string sql = "select COUNT(Facility.FacilityTypeId) As FType, FacilityType.FacilityTypeName AS FacilityType " +

                                "  FROM Facility  LEFT JOIN FacilityType ON FacilityType.Id = Facility.FacilityTypeId " +
                                                " LEFT JOIN District ON District.Id = Facility.DistrictId " +
                                                " LEFT JOIN Province ON Province.Id = District.ProvinceId " +
                                "  WHERE 1 = Case When @ProvinceId = 0 Then 1  When @ProvinceId = District.ProvinceId Then 1  END " +
                                                " GROUP BY FacilityType.FacilityTypeName ";

            IList lst = new ArrayList();

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@ProvinceId", cm, ProvinceId);

                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            object[] values = new object[dr.FieldCount];
                            dr.GetSqlValues(values);
                            lst.Add(values);
                        }

                    }
                }
            }
            return lst;

        }

    }}
