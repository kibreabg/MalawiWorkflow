using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;

using Chai.ZADS.CoreDomain.Resource;
using Chai.ZADS.CoreDomain.Location;

namespace Chai.ZADS.DataAccess.Location
{
    public class FacilityDao : BaseDao
    {
        public Facility GetfacilityById(int facilityid)
        {
            string sql = "SELECT  FacilityId, DistrictId, ProvinceId, FacilityName, FacilityTypeId, FacilityCode, FacilityLevel, [FacilityTypeName], [Description],";
            sql += "(SELECT [DistrictName] FROM District where Id = f.DistrictId) as DistrictName, ";
            sql += "(SELECT [ProvinceName] FROM Province where Id = f.ProvinceId) as ProvinceName ";
            sql += "FROM [Facility] as f INNER JOIN FacilityType as t on t.Id = f.FacilityTypeId WHERE [FacilityId] = @FacilityId ";

            sql += "SELECT [FacilityId],[TestingPointTypeId],[Id],[ProvinceId],";
            sql += "(SELECT [TypeName] FROM TestingPointType WHERE Id = TestingPointTypeId) as TypeName ";
            sql += "FROM FacilityTestingPoint where FacilityId = @FacilityId ";

            sql += "SELECT [InstrumentId],[FacilityId],[TestingPointId],[DeviceId],[DeviceCode],[DateInstalled],";
            sql += "[Longitude],[Latitude],[SerialNumber],[MobilNo],[PINNumber], ";
            sql += "(SELECT InstrumentName FROM Instrument as i WHERE i.InstrumentId = d.InstrumentId) as InstrumentName,";
            sql += "(SELECT [TypeName] FROM TestingPointType WHERE Id = (SELECT [TestingPointTypeId] FROM FacilityTestingPoint WHERE Id = d.TestingPointId)) as TypeName ";
            sql += "FROM FacilityDevice d  where FacilityId = @FacilityId";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@FacilityId", cm, facilityid);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                Facility fac = GetFacility(dr);
                                dr.NextResult();
                                FetchFacilityTestingPoint(dr, fac);
                                dr.NextResult();
                                FetchDevice(dr, fac);
                                return fac;
                            }
                        }
                    }
                }
            }
            return null;
        }
       
        private Facility GetFacility(SqlDataReader dr)
        {
            Facility facility = new Facility
            {
                Id = DatabaseHelper.GetInt32("FacilityId", dr),
                DistrictId = DatabaseHelper.GetInt32("DistrictId",dr),
                FacilityName = DatabaseHelper.GetString("FacilityName", dr),
                DistrictName = DatabaseHelper.GetString("DistrictName", dr),
                ProvinceName = DatabaseHelper.GetString("ProvinceName",dr),
                FacilityCode = DatabaseHelper.GetString("FacilityCode",dr),
                FacilityLevel = DatabaseHelper.GetString("FacilityLevel",dr),
                ProvinceId= DatabaseHelper.GetInt32("ProvinceId",dr)            
                
            };
            
            facility.FacilityType = new FacilityType()
            {
                Id = DatabaseHelper.GetInt32("FacilityTypeId", dr),
                FacilityTypeName = DatabaseHelper.GetString("FacilityTypeName", dr),
                Description = DatabaseHelper.GetString("Description", dr)
            };

            return facility;
        }
        private void FetchFacilityTestingPoint(SqlDataReader dr, Facility fac)
        {
            while (dr.Read())
            {
                FacilityTestingPoint fp = new FacilityTestingPoint
                {
                    Id = DatabaseHelper.GetInt32("Id", dr),
                    FacilityId = DatabaseHelper.GetInt32("FacilityId", dr),
                    Testingpointtypeid = DatabaseHelper.GetInt32("TestingPointTypeId", dr),
                    TypeName = DatabaseHelper.GetString("TypeName", dr),
                    ProvinceId = DatabaseHelper.GetInt32("ProvinceId", dr)
                };

                fac.TestingPoints.Add(fp);
            }
        }

        private void FetchDevice(SqlDataReader dr, Facility fac)
        {
            while (dr.Read())
            {
                FacilityDevice device = new FacilityDevice
                {
                    Id = DatabaseHelper.GetInt32("DeviceId", dr),
                    InstrumentId = DatabaseHelper.GetInt32("InstrumentId", dr),
                    FacilityId = DatabaseHelper.GetInt32("FacilityId", dr),
                    TestingPointId = DatabaseHelper.GetInt32("TestingPointId", dr),
                    InstrumentName = DatabaseHelper.GetString("InstrumentName", dr),
                    DeviceCode = DatabaseHelper.GetString("DeviceCode", dr),
                    MobilNo = DatabaseHelper.GetString("MobilNo", dr),
                    PINNumber = DatabaseHelper.GetString("PINNumber", dr),
                    SerialNumber = DatabaseHelper.GetString("SerialNumber", dr),
                    DateInstalled = DatabaseHelper.GetDateTime("DateInstalled", dr),
                    Latitude = DatabaseHelper.GetMoney("Latitude", dr),
                    Longitude = DatabaseHelper.GetMoney("Longitude", dr),
                    TestingPoint = DatabaseHelper.GetString("TypeName", dr)
                };

                fac.Devices.Add(device);
            }
        }
        private static void SetFacility(SqlCommand cm, Facility fac)
        {
            DatabaseHelper.InsertInt32Param("@DistrictId",cm, fac.DistrictId);
            DatabaseHelper.InsertInt32Param("@ProvinceId", cm, fac.ProvinceId);
            DatabaseHelper.InsertInt32Param("@FacilityTypeId", cm, fac.FacilityType.Id);
            DatabaseHelper.InsertStringNVarCharParam("@FacilityName", cm, fac.FacilityName);
            DatabaseHelper.InsertStringNVarCharParam("@FacilityLevel", cm, fac.FacilityLevel);
            DatabaseHelper.InsertStringNVarCharParam("@FacilityCode", cm, fac.FacilityCode);
        }

        public void Save(Facility facility, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO Facility(DistrictId,ProvinceId,FacilityName,FacilityTypeId,FacilityCode,FacilityLevel) "
            + "VALUES (@DistrictId,@ProvinceId,@FacilityName,@FacilityTypeId,@FacilityCode,@FacilityLevel) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection,sqltransaction))
            {
                SetFacility(cm, facility);
                facility.Id = int.Parse(cm.ExecuteScalar().ToString());

                foreach (FacilityTestingPoint fp in facility.TestingPoints)
                {
                    fp.FacilityId = facility.Id;
                    SaveTestingPoint(fp, sqltransaction);
                }

                foreach (FacilityDevice d in facility.Devices)
                {
                    d.FacilityId = facility.Id;
                    SaveDevice(d, sqltransaction);
                }
            }
        }
        public void Update(Facility facility, SqlTransaction sqltransaction)
        {
            string sql = "Update Facility SET DistrictId=@DistrictId,ProvinceId=@ProvinceId, FacilityName=@FacilityName,"
            + "FacilityTypeId=@FacilityTypeId, FacilityCode=@FacilityCode,FacilityLevel=@FacilityLevel where FacilityId = @facilityId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection,sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@facilityId", cm, facility.Id);
                SetFacility(cm, facility);
                cm.ExecuteNonQuery();

                UpdateTestingPoint(facility, sqltransaction);
                UpdateDevice(facility, sqltransaction);
            }
        }

        public void Delete(Facility facility, SqlTransaction sqltransaction)
        {
            string sql = "Delete Facility where FacilityId = @facilityid ";
          
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@facilityid", cm, facility.Id);
                cm.ExecuteNonQuery();
            }
        }

        public IList<FacilityInfo> GetListOfFacilityInfo(string filiter)
        {

            string sql = "SELECT FacilityId, DistrictId, ProvinceId, FacilityName, FacilityTypeId, FacilityCode, FacilityLevel, [FacilityTypeName], [Description], ";
            sql += "(SELECT [DistrictName] FROM District where Id = f.DistrictId) as DistrictName, ";
            sql += "(SELECT [ProvinceName] FROM Province where Id = f.ProvinceId) as ProvinceName ";
            sql += "FROM Facility as f INNER JOIN FacilityType as t on t.Id = f.FacilityTypeId ";

            if (string.IsNullOrEmpty(filiter))
                sql += " where " + filiter;

            IList<FacilityInfo> lstfacility = new List<FacilityInfo>();

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
                                lstfacility.Add(GetFacilityInfo(dr));
                            }
                        }
                    }
                }
            }
            return lstfacility;
        }

        private FacilityInfo GetFacilityInfo(SqlDataReader dr)
        {
            FacilityInfo facility = new FacilityInfo
            {
                Id = DatabaseHelper.GetInt32("FacilityId", dr),
                DistrictId = DatabaseHelper.GetInt32("DistrictId", dr),
                FacilityName = DatabaseHelper.GetString("FacilityName", dr),
                DistrictName = DatabaseHelper.GetString("DistrictName", dr),
                ProvinceName = DatabaseHelper.GetString("ProvinceName", dr),
                FacilityCode = DatabaseHelper.GetString("FacilityCode", dr),
                FacilityLevel = DatabaseHelper.GetString("FacilityLevel", dr),
                ProvinceId = DatabaseHelper.GetInt32("ProvinceId", dr),
                FacilityTypeName = DatabaseHelper.GetString("FacilityTypeName", dr),
            };

            return facility;
        }


        public ListOfInstrumentInFacility GetListOfInstrumentInFacility(int facilityId)
        {
            string sql = "SELECT InstrumentId, COUNT(*) AS NoIns FROM FacilityDevice ";
            sql += String.Format(" WHERE (FacilityId = {0}) AND (IsActive = 1) GROUP BY InstrumentId", facilityId);

            ListOfInstrumentInFacility lst = new ListOfInstrumentInFacility(facilityId);

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
                                lst.NoInstruments.Add(new NoInstrument(DatabaseHelper.GetInt32("InstrumentId", dr), DatabaseHelper.GetInt32("NoIns", dr)));
                            }
                        }
                    }
                }
            }
            return lst;
        }

        private void SetDevice(SqlCommand cm, FacilityDevice div)
        {
            DatabaseHelper.InsertStringNVarCharParam("@DeviceCode", cm, div.DeviceCode);
            DatabaseHelper.InsertInt32Param("@InstrumentId", cm, div.InstrumentId);
            DatabaseHelper.InsertStringNVarCharParam("@MobilNo", cm, div.MobilNo);
            DatabaseHelper.InsertStringNVarCharParam("@PINNumber", cm, div.PINNumber);
            DatabaseHelper.InsertStringNVarCharParam("@SerialNumber", cm, div.SerialNumber);

            DatabaseHelper.InsertInt32Param("@FacilityId", cm, div.FacilityId);
            DatabaseHelper.InsertInt32Param("@TestingPointId", cm, div.TestingPointId);
            DatabaseHelper.InsertDateTimeParam("@DateInstalled", cm, div.DateInstalled);
            DatabaseHelper.InsertMoneyParam("@Longitude", cm, div.Longitude);
            DatabaseHelper.InsertMoneyParam("@Latitude", cm, div.Latitude);
        }
        
        private void SetFacilityTestingPoint(SqlCommand cm, FacilityTestingPoint fp)
        {
            DatabaseHelper.InsertInt32Param("@FacilityId", cm, fp.FacilityId);
            DatabaseHelper.InsertInt32Param("@TestingPointTypeId", cm, fp.Testingpointtypeid);
            DatabaseHelper.InsertInt32Param("@ProvinceId", cm, fp.ProvinceId);
        }

        private void SaveDevice(FacilityDevice div, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO FacilityDevice ([InstrumentId],[FacilityId],[TestingPointId],[DeviceCode],[DateInstalled],"
                + "[Longitude],[Latitude],[SerialNumber],[MobilNo],[PINNumber]) VALUES (@InstrumentId,	@FacilityId,@TestingPointId,"
                + "@DeviceCode,@DateInstalled,	@Longitude,	@Latitude,@SerialNumber,@MobilNo,@PINNumber) SELECT @@identity";
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                SetDevice(cm, div);
                div.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        private void UpdateDevice(Facility fac, SqlTransaction sqltransaction)
        {
            string sql = "UPDATE FacilityDevice SET "
            + "[InstrumentId] = @InstrumentId, "
            + "[FacilityId] = @FacilityId, "
            + "[TestingPointId] = @TestingPointId, "
            + "[DeviceCode] = @DeviceCode,"
            + "[DateInstalled] = @DateInstalled,"
            + "[Longitude] = @Longitude,"
            + "[Latitude] = @Latitude,"
            + "[SerialNumber] = @SerialNumber,"
            + "[MobilNo] = @MobilNo,"
            + "[PINNumber] = @PINNumber"
            + "WHERE [DeviceId] = @DeviceId ";

            string sqlDelete = "Delete FacilityDevice where DeviceId = @DeviceId";

            foreach (FacilityDevice dl in fac.Devices)
            {
                if (dl.IsDirty && dl.Id > 0)
                {
                    using (SqlCommand cm = new SqlCommand(sqlDelete, DefaultConnection, sqltransaction))
                    {
                        DatabaseHelper.InsertInt32Param("@DeviceId", cm, dl.Id);
                        cm.ExecuteNonQuery();
                    }
                }
                else if (!dl.IsDirty && !dl.IsNew())
                {
                    using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
                    {
                        DatabaseHelper.InsertInt32Param("@DeviceId", cm, dl.Id);
                        SetDevice(cm, dl);
                        cm.ExecuteNonQuery();
                    }
                }
                else if (!dl.IsDirty && dl.IsNew())
                {
                    dl.FacilityId = fac.Id;
                    SaveDevice(dl, sqltransaction);
                }
            }
        }

        private void SaveTestingPoint(FacilityTestingPoint fp, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO FacilityTestingPoint(FacilityId, TestingPointTypeId,ProvinceId) "
                         + "VALUES (@FacilityId, @TestingPointTypeId,@ProvinceId) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection,sqltransaction))
            {
                SetFacilityTestingPoint(cm, fp);
                fp.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        private void UpdateTestingPoint(Facility fac, SqlTransaction sqltransaction)
        {
            string sql = "Update FacilityTestingPoint SET FacilityId=@FacilityId, TestingPointTypeId=@TestingPointTypeId, "
                       + "ProvinceId = @ProvinceId where Id = @Id";

            string sqlDelete = "Delete FacilityTestingPoint where Id = @Id";

            foreach (FacilityTestingPoint dl in fac.TestingPoints)
            {
                if (dl.IsDirty && dl.Id > 0)
                {
                    using (SqlCommand cm = new SqlCommand(sqlDelete, DefaultConnection, sqltransaction))
                    {
                        DatabaseHelper.InsertInt32Param("@Id", cm, dl.Id);
                        cm.ExecuteNonQuery();
                    }
                }
                else if (!dl.IsDirty && !dl.IsNew())
                {
                    using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
                    {
                        DatabaseHelper.InsertInt32Param("@Id", cm, dl.Id);
                        SetFacilityTestingPoint(cm, dl);
                        cm.ExecuteNonQuery();
                    }
                }
                else if (!dl.IsDirty && dl.IsNew())
                {
                    dl.FacilityId = fac.Id;
                    SaveTestingPoint(dl, sqltransaction);
                }
            }
        }
    }
    
}
