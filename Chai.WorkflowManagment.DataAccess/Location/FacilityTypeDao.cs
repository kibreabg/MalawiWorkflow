
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain.Location;
using Chai.ZADS.DataAccess;

namespace Chai.ZADS.DataAccess.Location
{
    public class FacilityTypeDao : BaseDao
    {
        public FacilityType GetFacilityTypeById(int facilitytypeId)
        {
            string sql = "SELECT FacilityType.* FROM FacilityType where Id = @facilitytypeId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@facilitytypeId", cm, facilitytypeId);

                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr != null)
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            return GetFacilityType(dr);
                        }
                    }
                }
            }
            return null;
        }

        private static FacilityType GetFacilityType(SqlDataReader dr)
        {
            FacilityType llg = new FacilityType
            {
                Id = DatabaseHelper.GetInt32("Id", dr),
                FacilityTypeName = DatabaseHelper.GetString("FacilityTypeName", dr),
                Description = DatabaseHelper.GetString("Description", dr),

            };

            return llg;
        }

        private static void SetFacilityType(SqlCommand cm, FacilityType facilitytype)
        {
            DatabaseHelper.InsertStringNVarCharParam("@FacilityTypeName", cm, facilitytype.FacilityTypeName);
            DatabaseHelper.InsertStringNVarCharParam("@Description", cm, facilitytype.Description);

        }

        public void Save(FacilityType facilitytype)
        {
            string sql = "INSERT INTO FacilityType(FacilityTypeName, Description) VALUES (@FacilityTypeName, @Description) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                SetFacilityType(cm, facilitytype);
                facilitytype.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        public void Update(FacilityType facilitytype)
        {
            string sql = "Update FacilityType SET FacilityTypeName =@FacilityTypeName, Description=@Description  where Id = @facilitytypeId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@facilitytypeId", cm, facilitytype.Id);
                SetFacilityType(cm, facilitytype);
                cm.ExecuteNonQuery();
            }
        }

        public void Delete(int facilitytypeId)
        {
            string sql = "Delete FacilityType where Id = @facilitytypeId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@facilitytypeId", cm, facilitytypeId);
                cm.ExecuteNonQuery();
            }
        }

        public IList<FacilityType> GetListOfFacilityType(string value)
        {
            string sql = "SELECT * FROM FacilityType ";

            IList<FacilityType> lstFacilityType = new List<FacilityType>();

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            lstFacilityType.Add(GetFacilityType(dr));
                        }
                    }
                }
            }
            return lstFacilityType;
        }
    }
}
