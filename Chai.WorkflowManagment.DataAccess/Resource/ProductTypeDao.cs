
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Chai.ZADS.CoreDomain.Resource;

namespace Chai.ZADS.DataAccess.Resource
{
    public class ProductTypeDao : BaseDao
    {
        public ProductType GetProductTypeById(int producttypeId)
        {
            string sql = "SELECT * FROM ProductType where TypeID = @producttypeId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@producttypeId", cm, producttypeId);

                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr != null)
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            return GetProductType(dr);
                        }
                    }
                }
            }
            return null;
        }

        private static ProductType GetProductType(SqlDataReader dr)
        {
            ProductType producttype = new ProductType
            {
                Id = DatabaseHelper.GetInt32("TypeID", dr),
                TypeName = DatabaseHelper.GetString("TypeName", dr),
                Description = DatabaseHelper.GetString("Description", dr),

            };

            return producttype;
        }

        private static void SetProductType(SqlCommand cm, ProductType producttype)
        {
            DatabaseHelper.InsertStringNVarCharParam("@Name", cm, producttype.TypeName);
            DatabaseHelper.InsertStringNVarCharParam("@Description", cm, producttype.Description);

        }

        public void Save(ProductType producttype)
        {
            string sql = "INSERT INTO ProductType(TypeName, Description) VALUES (@Name, @Description) SELECT @@identity";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                SetProductType(cm, producttype);
                producttype.Id = int.Parse(cm.ExecuteScalar().ToString());
            }
        }

        public void Update(ProductType producttype)
        {
            string sql = "Update ProductType SET TypeName = @Name, Description = @Description  where TypeID = @Id";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@Id", cm, producttype.Id);
                SetProductType(cm, producttype);
                cm.ExecuteNonQuery();
            }
        }

        public void Delete(int producttypeId)
        {
            string sql = "Delete ProductType where TypeID = @producttypeId";

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                DatabaseHelper.InsertInt32Param("@producttypeId", cm, producttypeId);
                cm.ExecuteNonQuery();
            }
        }

        public IList<ProductType> GetListOfProductType()
        {
            string sql = "SELECT * FROM ProductType";

            IList<ProductType> lstProductType = new List<ProductType>();

            using (SqlCommand cm = new SqlCommand(sql, DefaultConnection))
            {
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            lstProductType.Add(GetProductType(dr));
                        }
                    }
                }
            }
            return lstProductType;
        }
    }
}
