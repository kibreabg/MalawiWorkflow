using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain;
using Chai.ZADS.Enums;
using Chai.ZADS.CoreDomain.Resource;
using Chai.ZADS.CoreDomain.Inventorys.Reports;

namespace Chai.ZADS.DataAccess.Inventorys
{
    public class DocumentDao : BaseDao
    {
        public Document GetDocumentById(int docid)
        {
            string sql = "SELECT [SiteId], [RegionId], [IsVoided], [DocumentID], [DocumentType], [DocumentStatus], [ReferenceNumber],  "
                + "[DocumentDisplayNumber], [DispatchDocumentId], [AlphaPart], [IntegerPart], [DocumentDateTime], [ModifiedBy], [ModifiedDateTime], "
                + "[SiteName], [RegionName] FROM Document WHERE [DocumentID] = @DocumentID "
                + "DECLARE @DocumentType INT SELECT @DocumentType = [DocumentType] FROM Document WHERE [DocumentID] = @DocumentID "
                + "IF @DocumentType = 1 BEGIN "
                + "SELECT dl.[DocumentID], dl.[OwnerDocumentType], dl.[DocumentLineID], dl.[Description], dl.[UnitPrice], dl.[SubTotal], dl.[IssuedQuantity], dl.[ReturnedQuantity], "
                + "P.ProductID, P.ProductName, P.SerialNo, P.BasicUnit, P.PackSize, (select TypeName from ProductType where TypeID= P.ProductTypeId ) as TypeName, "
                + "il.[LineID], il.[DocumentDateTime], il.[Quantity] as il_qty, il.[IsVoided] as il_isvoid "
                + "FROM DocumentLine as dl INNER JOIN  Product AS P ON dl.ProductID = P.ProductID "
                + "LEFT JOIN InventoryOutgoingLine as il on dl.DocumentLineID = il.DocumentLineID  WHERE dl.DocumentID = @DocumentID "
                + "END ELSE BEGIN "
                + "SELECT dl.[DocumentID], dl.[OwnerDocumentType], dl.[DocumentLineID], dl.[Description], dl.[UnitPrice], dl.[SubTotal], dl.[IssuedQuantity], dl.[ReturnedQuantity], "
                + "P.ProductID, P.ProductName, P.SerialNo, P.BasicUnit, P.PackSize, (select TypeName from ProductType where TypeID= P.ProductTypeId ) as TypeName, "
                + "il.[LineID], il.[DocumentDateTime], il.[Quantity] as il_qty, il.[IsVoided] as il_isvoid "
                + "FROM DocumentLine as dl INNER JOIN Product AS P ON dl.ProductID = P.ProductID "
                + "LEFT JOIN InventoryIncomingLine as il on dl.DocumentLineID = il.DocumentLineID  WHERE dl.DocumentID = @DocumentID END ";


            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertInt32Param("@DocumentID", cm, docid);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                Document doc = FetchDocument(dr);
                                dr.NextResult();
                                FetchDocumentLine(dr, doc);
                                //dr.NextResult();
                                //FetchDocumentLine(dr, doc);
                                return doc;
                            }
                        }
                    }
                }
            }

            return null;

        }

        private Document FetchDocument(SqlDataReader dr)
        {
            Document doc = new Document()
            {
                SiteId = DatabaseHelper.GetInt32("SiteId", dr),
                RegionId = DatabaseHelper.GetInt32("RegionId", dr),
                IsVoided = DatabaseHelper.GetBoolean("IsVoided", dr),
                Id = DatabaseHelper.GetInt32("DocumentID", dr),
                ReferenceNumber = DatabaseHelper.GetString("ReferenceNumber", dr),
                DocumentDisplayNumber = DatabaseHelper.GetString("DocumentDisplayNumber", dr),
                DispatchDocumentId = DatabaseHelper.GetInt32("DispatchDocumentId", dr),
                AlphaPart = DatabaseHelper.GetString("AlphaPart", dr),
                IntegerPart = DatabaseHelper.GetInt32("IntegerPart", dr),
                DocumentDateTime = DatabaseHelper.GetDateTime("DocumentDateTime", dr),
                SiteName = DatabaseHelper.GetString("SiteName", dr),
                RegionName = DatabaseHelper.GetString("RegionName", dr),
                ModifiedBy = DatabaseHelper.GetString("ModifiedBy", dr),
                ModifiedDateTime = DatabaseHelper.GetDateTime("ModifiedDateTime", dr)
            };

            doc.DocumentType = (DocumentType)Enum.ToObject(typeof(DocumentType), DatabaseHelper.GetInt32("DocumentType", dr));
            doc.Status = (DocumentStatus)Enum.Parse(typeof(DocumentStatus), DatabaseHelper.GetString("DocumentStatus", dr));
            return doc;
        }

        private void FetchDocumentLine(SqlDataReader dr, Document doc)
        {
            while (dr.Read())
            {
                DocumentLine dl = new DocumentLine()
                {
                    DocumentId = DatabaseHelper.GetInt32("DocumentID", dr),
                    OwnerDocumentType = (DocumentType)Enum.ToObject(typeof(DocumentType), DatabaseHelper.GetInt32("OwnerDocumentType", dr)),
                    Id = DatabaseHelper.GetInt32("DocumentLineID", dr),
                    Description = DatabaseHelper.GetString("Description", dr),
                    UnitPrice = DatabaseHelper.GetMoney("UnitPrice", dr),
                    SubTotal = DatabaseHelper.GetMoney("SubTotal", dr),
                    Issued = DatabaseHelper.GetMoney("IssuedQuantity", dr),
                    Returned = DatabaseHelper.GetMoney("ReturnedQuantity", dr),
                };

                dl.Product = new ProductInfo()
                {
                    Id = DatabaseHelper.GetInt32("ProductID", dr),
                    ProductName = DatabaseHelper.GetString("ProductName", dr),
                    BasicUnit = DatabaseHelper.GetString("BasicUnit", dr),
                    PackSize = DatabaseHelper.GetInt32("PackSize", dr),
                    SerialNo = DatabaseHelper.GetString("SerialNo", dr),
                    TypeName = DatabaseHelper.GetString("TypeName", dr)
                };

                IInventoryLine iline;
                if (dl.OwnerDocumentType == DocumentType.GoodsDispatchNot)
                {
                    dl.OutgoingInventory = new InventoryOutgoingLine()
                    {
                        Id = DatabaseHelper.GetInt32("LineID", dr)
                    };
                    iline = dl.OutgoingInventory;
                }
                else
                {
                    dl.IncomingInventory = new InventoryIncomingLine()
                    {
                        Id = DatabaseHelper.GetInt32("LineID", dr)
                    };
                    iline = dl.IncomingInventory;
                }

                iline.DocumentLineId = DatabaseHelper.GetInt32("DocumentLineID", dr);
                iline.ProductId = DatabaseHelper.GetInt32("ProductID", dr);
                iline.DocumentDateTime = DatabaseHelper.GetDateTime("DocumentDateTime", dr);
                iline.Quantity = DatabaseHelper.GetMoney("il_qty", dr);
                iline.IsVoided = DatabaseHelper.GetBoolean("il_isvoid", dr);

                doc.DocumentLines.Add(dl);
            }
        }

        private void SetDocumentParameters(SqlCommand cm, Document doc)
        {
            DatabaseHelper.InsertBooleanParam("@IsVoided", cm, doc.IsVoided);
            DatabaseHelper.InsertStringNVarCharParam("@ReferenceNumber", cm, doc.ReferenceNumber);
            DatabaseHelper.InsertDateTimeParam("@DocumentDateTime", cm, doc.DocumentDateTime);
            DatabaseHelper.InsertStringNVarCharParam("@DocumentStatus", cm, doc.Status.ToString());
            DatabaseHelper.InsertStringNVarCharParam("@ModifiedBy", cm, doc.ModifiedBy);
            DatabaseHelper.InsertDateTimeParam("@ModifiedDateTime", cm, doc.ModifiedDateTime);
        }

        private void SetDocumentLine(SqlCommand cm, DocumentLine dl)
        {
            DatabaseHelper.InsertInt32Param("@ProductID", cm, dl.Product.Id);
            DatabaseHelper.InsertStringNVarCharParam("@Description", cm, dl.Description);
            DatabaseHelper.InsertMoneyParam("@UnitPrice", cm, dl.UnitPrice);
            DatabaseHelper.InsertMoneyParam("@SubTotal", cm, dl.SubTotal);
            DatabaseHelper.InsertMoneyParam("@Issued", cm, dl.Issued);
            DatabaseHelper.InsertMoneyParam("@Returned", cm, dl.Returned);
        }

        private void SetInventoryLine(SqlCommand cm, IInventoryLine il)
        {
            cm.Parameters.AddWithValue("@ProductID", il.ProductId);
            cm.Parameters.AddWithValue("@DocumentDateTime", il.DocumentDateTime);
            cm.Parameters.AddWithValue("@Quantity", il.Quantity);
            cm.Parameters.AddWithValue("@IsVoided", il.IsVoided);
        }

        public void Save(Document doc, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO Document ([SiteId],[RegionId],[IsVoided],	[DocumentType],	[ReferenceNumber], [DocumentStatus], "
                + "[DocumentDisplayNumber], [DispatchDocumentId], [AlphaPart], [IntegerPart], [DocumentDateTime], [ModifiedBy], [ModifiedDateTime], [SiteName], [RegionName])"
                + " VALUES (@SiteId, @RegionId, @IsVoided, @DocumentType, @ReferenceNumber, @DocumentStatus, @DocumentDisplayNumber,"
                + "@DispatchDocumentId,	@AlphaPart,	@IntegerPart, @DocumentDateTime, @ModifiedBy, @ModifiedDateTime, @SiteName, @RegionName) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@SiteId", cm, doc.SiteId);
                DatabaseHelper.InsertInt32Param("@RegionId", cm, doc.RegionId);
                DatabaseHelper.InsertInt32Param("@DocumentType", cm, (int)doc.DocumentType);
                DatabaseHelper.InsertStringNVarCharParam("@SiteName", cm, doc.SiteName);
                DatabaseHelper.InsertStringNVarCharParam("@RegionName", cm, doc.RegionName);
                DatabaseHelper.InsertStringNVarCharParam("@DocumentDisplayNumber", cm, doc.DocumentDisplayNumber);
                DatabaseHelper.InsertInt32Param("@DispatchDocumentId", cm, doc.DispatchDocumentId);
                DatabaseHelper.InsertStringNVarCharParam("@AlphaPart", cm, doc.AlphaPart);
                DatabaseHelper.InsertInt32Param("@IntegerPart", cm, doc.IntegerPart);

                SetDocumentParameters(cm, doc);
                doc.Id = int.Parse(cm.ExecuteScalar().ToString());

                foreach (DocumentLine dl in doc.DocumentLines)
                {
                    dl.DocumentId = doc.Id;
                    SaveDocumentLine(dl, sqltransaction);
                }
            }
        }

        private void SaveDocumentLine(DocumentLine dl, SqlTransaction sqltransaction)
        {
            string sql = "INSERT INTO DocumentLine ([DocumentID], [OwnerDocumentType], [ProductID], [Description], "
                + "[UnitPrice], [SubTotal], [IssuedQuantity], [ReturnedQuantity]) VALUES (@DocumentID, @OwnerDocumentType, @ProductID,"
                + "@Description,  @UnitPrice, @SubTotal, @Issued, @Returned) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@DocumentID", cm, dl.DocumentId);
                DatabaseHelper.InsertInt32Param("@OwnerDocumentType", cm, (int)dl.OwnerDocumentType);

                SetDocumentLine(cm, dl);
                dl.Id = int.Parse(cm.ExecuteScalar().ToString());

                //if (dl.OwnerDocumentType == DocumentType.GoodsDispatchNot)
                //{
                //    dl.OutgoingInventory.DocumentLineId = dl.Id;
                //    dl.OutgoingInventory.Id = SaveInventoryLine(dl.OutgoingInventory, "InventoryOutgoingLine", sqltransaction);
                //}
                //else
                //{
                //    dl.IncomingInventory.DocumentLineId = dl.Id;
                //    dl.IncomingInventory.Id = SaveInventoryLine(dl.IncomingInventory, "InventoryIncomingLine", sqltransaction);
                //}
            }
        }

        private int SaveInventoryLine(IInventoryLine il, string tbl, SqlTransaction sqltransaction)
        {
            string sql = String.Format("INSERT INTO {0} ([DocumentLineID], [ProductID], [DocumentDateTime], [Quantity], [IsVoided]) ", tbl);
            sql += "VALUES (@DocumentLineID, @ProductID, @DocumentDateTime, @Quantity, @IsVoided) SELECT @@identity";

            int id;
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                cm.Parameters.AddWithValue("@DocumentLineID", il.DocumentLineId);
                SetInventoryLine(cm, il);
                id = int.Parse(cm.ExecuteScalar().ToString());
            }
            return id;
        }

        public void Update(Document doc, SqlTransaction sqltransaction)
        {
            string sql = "UPDATE Document SET [IsVoided] = @IsVoided, [ReferenceNumber] = @ReferenceNumber, [DocumentDateTime] = @DocumentDateTime, "
                + " [DocumentStatus] = @DocumentStatus, [ModifiedBy] = @ModifiedBy, [ModifiedDateTime] = @ModifiedDateTime WHERE [DocumentID] = @DocumentID";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@DocumentID", cm, doc.Id);
                SetDocumentParameters(cm, doc);
                cm.ExecuteNonQuery();
                UpdateDocumentLine(doc, sqltransaction);
            }
        }

        private void UpdateDocumentLine(Document doc, SqlTransaction sqltransaction)
        {
            string sql = "UPDATE DocumentLine SET [ProductID] = @ProductID, [Description] = @Description, "
                + "[UnitPrice] = @UnitPrice, [SubTotal] = @SubTotal, [IssuedQuantity] = @Issued, [ReturnedQuantity] = @Returned "
                + "WHERE [DocumentLineID] = @DocumentLineID";

            string sqlDelete = "DELETE FROM DocumentLine WHERE [DocumentLineID] = @DocumentLineID";

            foreach (DocumentLine dl in doc.DocumentLines)
            {
                if (dl.IsDirty && dl.Id > 0)
                {
                    using (SqlCommand cm = new SqlCommand(sqlDelete, DefaultConnection, sqltransaction))
                    {
                        DatabaseHelper.InsertInt32Param("@DocumentLineID", cm, dl.Id);
                        cm.ExecuteNonQuery();
                    }
                }
                else if (!dl.IsDirty && !dl.IsNew())
                {
                    using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
                    {
                        DatabaseHelper.InsertInt32Param("@DocumentLineID", cm, dl.Id);
                        SetDocumentLine(cm, dl);
                        cm.ExecuteNonQuery();
                    }

                    if (doc.Status == DocumentStatus.Saved)
                    {
                        if (dl.OwnerDocumentType == DocumentType.GoodsDispatchNot)
                        {
                            if (dl.OutgoingInventory.IsNew())
                                SaveInventoryLine(dl.OutgoingInventory, "InventoryOutgoingLine", sqltransaction);
                            else
                                UpdateInventoryLine(dl.OutgoingInventory, "InventoryOutgoingLine", sqltransaction);
                        }
                        else
                        {
                            if (dl.IncomingInventory.IsNew())
                                SaveInventoryLine(dl.IncomingInventory, "InventoryIncomingLine", sqltransaction);
                            else
                                UpdateInventoryLine(dl.IncomingInventory, "InventoryIncomingLine", sqltransaction);
                        }
                    }
                }
                else if (!dl.IsDirty && dl.IsNew())
                {
                    dl.DocumentId = doc.Id;
                    SaveDocumentLine(dl, sqltransaction);

                    if (doc.Status == DocumentStatus.Saved)
                    {
                        if (dl.OwnerDocumentType == DocumentType.GoodsDispatchNot)
                        {
                            dl.OutgoingInventory.DocumentLineId = dl.Id;

                            if (dl.OutgoingInventory.IsNew())
                                SaveInventoryLine(dl.OutgoingInventory, "InventoryOutgoingLine", sqltransaction);
                            else
                                UpdateInventoryLine(dl.OutgoingInventory, "InventoryOutgoingLine", sqltransaction);
                        }
                        else
                        {
                            dl.IncomingInventory.DocumentLineId = dl.Id;
                            if (dl.IncomingInventory.IsNew())
                                SaveInventoryLine(dl.IncomingInventory, "InventoryIncomingLine", sqltransaction);
                            else
                                UpdateInventoryLine(dl.IncomingInventory, "InventoryIncomingLine", sqltransaction);
                        }
                    }
                }
            }
        }

        private void UpdateInventoryLine(IInventoryLine il, string tbl, SqlTransaction sqltransaction)
        {
            string sql = String.Format("UPDATE {0} SET [ProductID] = @ProductID, [DocumentDateTime] = @DocumentDateTime, [Quantity] = @Quantity,", tbl);
            sql += "[IsVoided] = @IsVoided [IsVoided] = @IsVoided WHERE [LineID] = @LineID";
            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection, sqltransaction))
            {
                DatabaseHelper.InsertInt32Param("@LineID", cm, il.LineId);
                SetInventoryLine(cm, il);
                cm.ExecuteNonQuery();
            }
        }

        public void Delete(Document doc)
        {
            string sql = "DELETE FROM Document  WHERE DocumentID = @DocumentID";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@DocumentID", cm, doc.Id);
                cm.ExecuteNonQuery();
            }
        }

        public IList<DocumentInfo> GetListOfDocumentInfo(string filiter)
        {
            string sql = "SELECT [SiteId], [RegionId], [IsVoided], [DocumentID], [DocumentType], [ReferenceNumber], [DocumentStatus], "
                + "[DocumentDisplayNumber], [DispatchDocumentId], [AlphaPart], [IntegerPart], [DocumentDateTime], [ModifiedBy], "
                + "[ModifiedDateTime], [SiteName], [RegionName] FROM Document ";
            if (filiter != null)
                sql += " WHERE " + filiter;
            sql += " order by DocumentDateTime desc";

            IList<DocumentInfo> lstDoc = new List<DocumentInfo>();

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
                                lstDoc.Add(ReadDocumentInfo(dr));
                            }
                        }
                    }
                }
            }

            return lstDoc;
        }

        private DocumentInfo ReadDocumentInfo(SqlDataReader dr)
        {
            DocumentInfo doc = new DocumentInfo()
            {
                SiteId = DatabaseHelper.GetInt32("SiteId", dr),
                RegionId = DatabaseHelper.GetInt32("RegionId", dr),
                IsVoided = DatabaseHelper.GetBoolean("IsVoided", dr),
                Id = DatabaseHelper.GetInt32("DocumentID", dr),
                ReferenceNumber = DatabaseHelper.GetString("ReferenceNumber", dr),
                Status = (DocumentStatus)Enum.Parse(typeof(DocumentStatus), DatabaseHelper.GetString("DocumentStatus", dr)),
                DocumentDisplayNumber = DatabaseHelper.GetString("DocumentDisplayNumber", dr),
                DispatchDocumentId = DatabaseHelper.GetInt32("DispatchDocumentId", dr),
                AlphaPart = DatabaseHelper.GetString("AlphaPart", dr),
                IntegerPart = DatabaseHelper.GetInt32("IntegerPart", dr),
                DocumentDateTime = DatabaseHelper.GetDateTime("DocumentDateTime", dr),
                SiteName = DatabaseHelper.GetString("SiteName", dr),
                RegionName = DatabaseHelper.GetString("RegionName", dr),
                ModifiedBy = DatabaseHelper.GetString("ModifiedBy", dr),
                ModifiedDateTime = DatabaseHelper.GetDateTime("ModifiedDateTime", dr)
            };

            doc.DocumentType = (DocumentType)Enum.ToObject(typeof(DocumentType), DatabaseHelper.GetInt32("DocumentType", dr));

            return doc;
        }

        public IList<InventoryMovement> RptInventoryMovemnet(DateTime fromdate, DateTime todate, string fliter)
        {
            string sql = "SELECT (select TypeName from ProductType where TypeId = iv.ProductTypeId) as TypeName, "
               + "(select ProductName from product where ProductId = iv.ProductID) as ProductName, "
               + "SUM(iv.Issued) as Issued, SUM(iv.Returned) as Returned "
               + "FROM dbo.fnGetInventoryMovement(@FromDate, @ToDate) AS iv ";

            sql += String.Format(" {0} group by iv.ProductTypeId, iv.ProductID ORDER BY TypeName, ProductName ", fliter);

            IList<InventoryMovement> result = new List<InventoryMovement>();

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertDateTimeParam("@FromDate", cm, fromdate);
                    DatabaseHelper.InsertDateTimeParam("@ToDate", cm, todate);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                result.Add(FetchInventoryMovement(dr));
                            }
                        }
                    }
                }
            }

            return result;
        }

        private InventoryMovement FetchInventoryMovement(SqlDataReader dr)
        {
            InventoryMovement iv = new InventoryMovement()
            {
                ProductName = DatabaseHelper.GetString("ProductName", dr),
                TypeName = DatabaseHelper.GetString("TypeName", dr),
                Issued = DatabaseHelper.GetMoney("Issued", dr),
                Returned = DatabaseHelper.GetMoney("Returned", dr)
            };

            return iv;
        }

        public void CreateNumberSequence(NumberSequence nsequence)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand("CreateNumberSequence", con))
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;

                    DatabaseHelper.InsertStringNVarCharParam("@typeName", cm, nsequence.TypeName.ToString());
                    DatabaseHelper.InsertStringNVarCharParam("@startValue", cm, nsequence.CurrentValue);
                    DatabaseHelper.InsertStringNVarCharParam("@startAlpha", cm, nsequence.AlphaPart);
                    DatabaseHelper.InsertInt32Param("@startInt", cm, nsequence.IntegerPart);
                    DatabaseHelper.InsertInt32Param("@numSeqID", cm, nsequence.Id);
                    cm.Parameters["@numSeqID"].Direction = ParameterDirection.Output;

                    cm.ExecuteNonQuery();
                    nsequence.Id = (int)cm.Parameters["@numSeqID"].Value;
                }
            }
        }

        public void SetCurrentDisplayValue(NumberSequence nsequence)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand("SetCurrentDisplayValue", con))
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;

                    DatabaseHelper.InsertStringNVarCharParam("@newValue", cm, nsequence.CurrentValue);
                    DatabaseHelper.InsertStringNVarCharParam("@AlphaPart", cm, nsequence.AlphaPart);
                    DatabaseHelper.InsertInt32Param("@Integerpart", cm, nsequence.IntegerPart);
                    DatabaseHelper.InsertInt32Param("@numSeqID", cm, nsequence.Id);

                    cm.Parameters["@newValue"].Direction = ParameterDirection.Output;
                    cm.Parameters["@Integerpart"].Direction = ParameterDirection.Output;
                    cm.Parameters["@AlphaPart"].Direction = ParameterDirection.Output;

                    cm.ExecuteNonQuery();

                    nsequence.IntegerPart = (int)cm.Parameters["@Integerpart"].Value;
                    nsequence.AlphaPart = cm.Parameters["@AlphaPart"].Value.ToString();
                    nsequence.CurrentValue = cm.Parameters["@newValue"].Value.ToString();
                }
            }
        }

        public NumberSequence GetNumberSequence(DocumentType doctype)
        {
            string sql = "SELECT NumberSequenceID, EntityName, CurrentValue, AlphaPart, IntegerPart FROM NumberSequence where EntityName = @DocType ";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand(sql, con))
                {
                    DatabaseHelper.InsertStringNVarCharParam("@DocType", cm, doctype.ToString());

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                return FetchNumberSequence(dr);
                            }
                        }
                    }
                }
            }

            return null;
        }

        private NumberSequence FetchNumberSequence(SqlDataReader dr)
        {
            NumberSequence np = new NumberSequence()
            {
                TypeName = (DocumentType)Enum.Parse(typeof(DocumentType), DatabaseHelper.GetString("EntityName", dr)),
                CurrentValue = DatabaseHelper.GetString("CurrentValue", dr),
                AlphaPart = DatabaseHelper.GetString("AlphaPart", dr),
                IntegerPart = DatabaseHelper.GetInt32("IntegerPart", dr),
                Id = DatabaseHelper.GetInt32("NumberSequenceID", dr)
            };

            return np;
        }

        public NumberSequence GetCurrentDisplayValue(NumberSequence sequence)
        {
            NumberSequence nseq = new NumberSequence();
            nseq.Id = sequence.Id;
            nseq.TypeName = sequence.TypeName;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                using (SqlCommand cm = new SqlCommand("GetCurrentDisplayValue", con))
                {
                    cm.CommandType = System.Data.CommandType.StoredProcedure;
                    DatabaseHelper.InsertInt32Param("@numSeqID", cm, nseq.Id);
                    
                    SqlParameter prmcurrentValue = cm.Parameters.Add("@currentValue", SqlDbType.NVarChar);                    
                    prmcurrentValue.Direction = ParameterDirection.Output;                    
                    prmcurrentValue.Size = 60;
                    
                    SqlParameter prmcurrentAlpha = cm.Parameters.Add("@currentAlpha", SqlDbType.NVarChar);
                    prmcurrentAlpha.Direction = ParameterDirection.Output;
                    prmcurrentAlpha.Size = 50;
                    
                    SqlParameter prmcurrentInt = cm.Parameters.Add("@currentInt", SqlDbType.Int);
                    prmcurrentInt.Direction = ParameterDirection.Output;
                    
                    cm.ExecuteNonQuery();

                    nseq.CurrentValue = prmcurrentValue.Value.ToString();
                    nseq.AlphaPart = prmcurrentAlpha.Value.ToString();
                    nseq.IntegerPart = (int)prmcurrentInt.Value;
                }
            }

            return nseq;
        }
    }
}
