
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain.Resource;
using Chai.ZADS.CoreDomain;
namespace Chai.ZADS.DataAccess.Resource
{
    public class ProductDao : BaseDao
    {
        public Product GetProductById(int groupId)
        {
            string sql = "SELECT [ProductTypeId], [ProductID], [ProductName], [SerialNo], [BasicUnit], [PackSize], [UnitPrice], Product.[Description], [TypeName] "
                + "FROM Product INNER JOIN ProductType ON ProductType.TypeID = Product.ProductTypeId Where ProductId = @productId ";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cm = new SqlCommand(sql, conn))
                {
                    DatabaseHelper.InsertInt32Param("@productId", cm, groupId);

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr != null)
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                return GetProduct(dr);
                            }
                        }
                    }
                }
            }
            return null;
        }
               

        private static Product GetProduct(SqlDataReader dr)
        {
            Product product = new Product
            {
                Id = DatabaseHelper.GetInt32("ProductId", dr),                
                ProductName = DatabaseHelper.GetString("ProductName", dr),
                PackSize = DatabaseHelper.GetInt32("PackSize", dr),
                BasicUnit = DatabaseHelper.GetString("BasicUnit", dr),
                SerialNo = DatabaseHelper.GetString("SerialNo", dr),
                UnitPrice = DatabaseHelper.GetMoney("UnitPrice", dr),
                Description = DatabaseHelper.GetString("Description", dr)                
            };
            
            product.ProductType = new ProductType()
            {
                Id = DatabaseHelper.GetInt32("ProductTypeId", dr),
                TypeName = DatabaseHelper.GetString("TypeName", dr)
            };

            return product;
        }

        private static void SetProduct(SqlCommand cm, Product product)
        {
            DatabaseHelper.InsertInt32Param("@ProductTypeId", cm, product.ProductType.Id);
            DatabaseHelper.InsertStringNVarCharParam("@Description", cm, product.Description);
            DatabaseHelper.InsertStringNVarCharParam("@ProductName", cm,product.ProductName);
            DatabaseHelper.InsertInt32Param("@PackSize", cm,product.PackSize);
            DatabaseHelper.InsertStringNVarCharParam("@BasicUnit", cm,product.BasicUnit);
            DatabaseHelper.InsertStringNVarCharParam("@SerialNo", cm,product.SerialNo);
            DatabaseHelper.InsertMoneyParam("@UnitPrice", cm, product.UnitPrice);
            
        }

        public void Save(Product product, SqlTransaction tr)
        {
            string sql = "INSERT INTO Product(ProductTypeId,ProductName,PackSize,BasicUnit,SerialNo,UnitPrice,Description) "
            + "VALUES (@ProductTypeId, @ProductName,@PackSize,@BasicUnit,@SerialNo,@UnitPrice, @Description) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection,tr))
            {
                SetProduct(cm, product);
                product.Id = int.Parse(cm.ExecuteScalar().ToString());               
            }
        }
       
        public void Update(Product product,SqlTransaction tr)
        {
            string sql = "Update Product SET ProductTypeId = @ProductTypeId, ProductName = @ProductName, PackSize = @PackSize, BasicUnit = @BasicUnit,"
                       + "SerialNo = @SerialNo, UnitPrice=@UnitPrice ,Description=@Description  where ProductId = @productId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection,tr))
            {
                DatabaseHelper.InsertInt32Param("@productId", cm, product.Id);
                SetProduct(cm, product);
                cm.ExecuteNonQuery();
            }
        }

        public void Delete(Product product)
        {
            string sql = "Delete Product where ProductId = @productId"; 

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@productId", cm, product.Id);
                cm.ExecuteNonQuery();
            }
        }
               
        public IList<Product> GetNotAssignedProductsforTest(int TestId)
        {
            string sql;
            sql = "SELECT Product.*,ProductType.TypeName as ProductTypeName FROM Product LEFT JOIN ProductType ON ProductType.Id = Product.ProductTypeId WHERE Product.ProductId Not In (Select ProductUsage.ProductId From ProductUsage where ProductUsage.TestId = @TestId)";

            IList<Product> lstProduct = new List<Product>();

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {

                DatabaseHelper.InsertInt32Param("@TestId", cm, TestId);


                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            lstProduct.Add(GetProduct(dr));
                        }
                    }
                }
            }
            return lstProduct;
        }
       
        private ProductInfo ReadProductInfo(SqlDataReader dr)
        {
            ProductInfo product = new ProductInfo
            {
                Id = DatabaseHelper.GetInt32("ProductId", dr),                
                ProductName = DatabaseHelper.GetString("ProductName", dr),
                SerialNo = DatabaseHelper.GetString("SerialNo", dr),                
                BasicUnit = DatabaseHelper.GetString("BasicUnit", dr),
                PackSize = DatabaseHelper.GetInt32("PackSize", dr),                
                UnitPrice = DatabaseHelper.GetMoney("UnitPrice", dr),
                Description = DatabaseHelper.GetString("Description", dr),
                TypeName = DatabaseHelper.GetString("TypeName", dr)
            };

            return product;
        }

        public IList<ProductInfo> GetListOfProductInfo(string filiter)
        {
            string sql = "SELECT [ProductID], [ProductName], [SerialNo], [BasicUnit], [PackSize], [UnitPrice], Product.[Description], [TypeName] " 
                + "FROM Product INNER JOIN ProductType ON ProductType.TypeID = Product.ProductTypeId";

            if (filiter != null)
                sql += " WHERE " + filiter;
            sql += " order by Product.ProductName";

            IList<ProductInfo> lstproduct = new List<ProductInfo>();

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
                                lstproduct.Add(ReadProductInfo(dr));
                            }
                        }
                    }
                }
            }
            return lstproduct;
        }
    }
}
