
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain.Resource;
using System.Collections;

namespace Chai.ZADS.DataAccess.Resource
{
    public class InstrumentDao : BaseDao
    {
        public Instrument GetInstrumentById(int instrumentId)
        {
            string sql = "SELECT [TestCategory],[InstrumentId],[InstrumentName],[ControlTobeDone],[NoControlTestRun],[MaxThroughPut],";
            sql += "[MonthMaxTPut],[Description] FROM Instrument where InstrumentId = @instrumentId";
            
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@instrumentId", cm, instrumentId);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                return GetInstrument(dr);
                            }
                        }
                    }
                }
            }

            return null;
        }

        private static Instrument GetInstrument(SqlDataReader dr)
        {
            Instrument instrument = new Instrument
            {
                Id = DatabaseHelper.GetInt32("InstrumentId", dr),
                InstrumentName = DatabaseHelper.GetString("InstrumentName", dr),
                Description = DatabaseHelper.GetString("Description", dr),
                ControlTobeDone = DatabaseHelper.GetString("ControlTobeDone", dr),
                NoControlTestRun = DatabaseHelper.GetInt32("NoControlTestRun", dr),
                MaxThroughPut = DatabaseHelper.GetInt32("MaxThroughPut", dr),
                MonthMaxTPut = DatabaseHelper.GetInt32("MonthMaxTPut", dr),
                TestCategory = DatabaseHelper.GetString("TestCategory", dr),
                Barcode = DatabaseHelper.GetString("Barcode", dr),
                ExpiryDate = DatabaseHelper.GetDateTime("ExpiryDate", dr),
                Mean = DatabaseHelper.GetDouble("Mean", dr)
            };

            return instrument;
        }

        private static void SetInstrument(SqlCommand cm, Instrument instruemnt)
        {
            DatabaseHelper.InsertStringNVarCharParam("@Description", cm, instruemnt.Description);
            DatabaseHelper.InsertStringNVarCharParam("@InstrumentName", cm, instruemnt.InstrumentName);
            DatabaseHelper.InsertStringNVarCharParam("@ControlTobeDone", cm, instruemnt.ControlTobeDone);
            DatabaseHelper.InsertInt32Param("@NoControlTestRun", cm, instruemnt.NoControlTestRun);
            DatabaseHelper.InsertInt32Param("@MaxThroughPut", cm, instruemnt.MaxThroughPut);
            DatabaseHelper.InsertInt32Param("@MonthMaxTPut", cm, instruemnt.MonthMaxTPut);
            DatabaseHelper.InsertStringNVarCharParam("@TestCategory", cm, instruemnt.TestCategory);
            DatabaseHelper.InsertStringNVarCharParam("@Barcode", cm, instruemnt.Barcode);
            DatabaseHelper.InsertDateTimeParam("@ExpiryDate", cm, instruemnt.ExpiryDate);
            DatabaseHelper.InsertDoubleParam("@Mean", cm, instruemnt.Mean);
        }

        public void Save(Instrument instruemnt, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO Instrument([TestCategory],[InstrumentName],[ControlTobeDone],[NoControlTestRun],[MaxThroughPut], ";
            sql += "[MonthMaxTPut],[Description], [Mean], [Barcode], [ExpiryDate]) VALUES (@TestCategory, @InstrumentName, @ControlTobeDone,  @NoControlTestRun,";
            sql += "@MaxThroughPut, @MonthMaxTPut, @Description, @Mean, @Barcode, @ExpiryDate) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                SetInstrument(cm, instruemnt);
                instruemnt.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        public void Update(Instrument instruemnt, SqlTransaction sqltransaction)
        {
            string sql = "UPDATE Instrument SET";
            sql += " [TestCategory] = @TestCategory,";
            sql += " [InstrumentName] = @InstrumentName,";
            sql += " [ControlTobeDone] = @ControlTobeDone,";
            sql += " [NoControlTestRun] = @NoControlTestRun,";
            sql += " [MaxThroughPut] = @MaxThroughPut,";
            sql += " [MonthMaxTPut] = @MonthMaxTPut,";
            sql += " [Description] = @Description,";
            sql += " [Barcode] = @Barcode,";
            sql += " [ExpiryDate] = @ExpiryDate,";
            sql += " [Mean] = @Mean";
            sql += " WHERE InstrumentId = @Id";
         
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@Id", cm, instruemnt.Id);
                SetInstrument(cm, instruemnt);
                cm.ExecuteNonQuery();
            }
        }

        public void Delete(int instruemntId)
        {
            string sql = "Delete Instrument where InstrumentId = @instruemntId";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@instruemntId", cm, instruemntId);
                    cm.ExecuteNonQuery();
                }
            }
        }

        public IList<Instrument> GetListOfInstrument(string value)
        {
            string sql = "SELECT [TestCategory], [InstrumentId], [InstrumentName], [ControlTobeDone], [NoControlTestRun], [MaxThroughPut], ";
            sql += "[MonthMaxTPut], [Description], [Mean], [Barcode], [ExpiryDate] FROM Instrument ";

            if (!string.IsNullOrEmpty(value))
                sql += " where " + value;

            IList<Instrument> lstInstrument = new List<Instrument>();
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
                                lstInstrument.Add(GetInstrument(dr));
                            }
                        }
                    }
                }
            }

            return lstInstrument;
        }

    }
}
