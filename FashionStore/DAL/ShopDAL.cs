using FashionStore.Code;
using FashionStore.Models;
using log4net;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FashionStore.DAL
{
    public class ShopDAL
    {

        private static readonly ILog logger = LogManager.GetLogger(typeof(ShopDAL));

        public List<Product> WomenProducts()
        {
            var products = new List<Product>();
            try
            {
                using (OracleConnection conn = new OracleConnection(Constants.Application.DBConnConnectionString))
                {
                    using (OracleCommand oCmd = new OracleCommand())
                    {
                        oCmd.CommandText = "XXFS_INVENTORY_PKG.sp_women_products";
                        oCmd.CommandType = CommandType.StoredProcedure;
                        oCmd.Connection = conn;

                        OracleParameter productsOut = new OracleParameter("op_products", OracleDbType.RefCursor, ParameterDirection.Output);
                        oCmd.Parameters.Add(productsOut);

                        conn.Open();
                        oCmd.ExecuteNonQuery();

                        if (!((OracleRefCursor)productsOut.Value).IsNull)
                        {
                            OracleDataReader dr = ((OracleRefCursor)productsOut.Value).GetDataReader();
                            while (dr.Read())
                            {
                                products.Add(new Product
                                {
                                    ProductId = dr["prod_id"].ToString() != "null" ? Convert.ToInt32(dr["prod_id"].ToString()) : 0,
                                    ProductName = dr["PRODUCT_NAME"].ToString() != "null" ? dr["PRODUCT_NAME"].ToString() : "",
                                    Description = dr["Description"].ToString() != "null" ? dr["Description"].ToString() : "",
                                    Image = dr["Product_Image"].ToString() != "null" ? dr["Product_Image"].ToString() : "",
                                    Quantity = dr["Quantity"].ToString() != "null" ? Convert.ToInt32(dr["Quantity"].ToString()) : 0,
                                    Price = dr["Price"].ToString() != "null" ? Convert.ToDecimal(dr["Price"].ToString()) : 0,
                                    StartDate = dr["Start_date"].ToString() != "null" ? Convert.ToDateTime(dr["Start_date"].ToString()).ToString("dd MMM yyyy") : "",
                                    EndDate = dr["End_date"].ToString() != "null" ? dr["End_date"].ToString() : "",
                                    Type = dr["Type"].ToString() != "null" ? dr["Type"].ToString() : "",
                                    Active = (!string.IsNullOrEmpty(dr["Active"].ToString()) && dr["Active"].ToString() == "1") ? true : false
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return products;
        }

        public List<Product> MenProducts()
        {
            var products = new List<Product>();
            try
            {
                using (OracleConnection conn = new OracleConnection(Constants.Application.DBConnConnectionString))
                {
                    using (OracleCommand oCmd = new OracleCommand())
                    {
                        oCmd.CommandText = "XXFS_INVENTORY_PKG.sp_men_products";
                        oCmd.CommandType = CommandType.StoredProcedure;
                        oCmd.Connection = conn;

                        OracleParameter productsOut = new OracleParameter("op_products", OracleDbType.RefCursor, ParameterDirection.Output);
                        oCmd.Parameters.Add(productsOut);

                        conn.Open();
                        oCmd.ExecuteNonQuery();

                        if (!((OracleRefCursor)productsOut.Value).IsNull)
                        {
                            OracleDataReader dr = ((OracleRefCursor)productsOut.Value).GetDataReader();
                            while (dr.Read())
                            {
                                products.Add(new Product
                                {
                                    ProductId = dr["prod_id"].ToString() != "null" ? Convert.ToInt32(dr["prod_id"].ToString()) : 0,
                                    ProductName = dr["PRODUCT_NAME"].ToString() != "null" ? dr["PRODUCT_NAME"].ToString() : "",
                                    Description = dr["Description"].ToString() != "null" ? dr["Description"].ToString() : "",
                                    Image = dr["Product_Image"].ToString() != "null" ? dr["Product_Image"].ToString() : "",
                                    Quantity = dr["Quantity"].ToString() != "null" ? Convert.ToInt32(dr["Quantity"].ToString()) : 0,
                                    Price = dr["Price"].ToString() != "null" ? Convert.ToDecimal(dr["Price"].ToString()) : 0,
                                    StartDate = dr["Start_date"].ToString() != "null" ? Convert.ToDateTime(dr["Start_date"].ToString()).ToString("dd MMM yyyy") : "",
                                    EndDate = dr["End_date"].ToString() != "null" ? dr["End_date"].ToString() : "",
                                    Type = dr["Type"].ToString() != "null" ? dr["Type"].ToString() : "",
                                    Active = (!string.IsNullOrEmpty(dr["Active"].ToString()) && dr["Active"].ToString() == "1") ? true : false
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return products;
        }

        public Product Product(int productId)
        {
            var product = new Product();
            try
            {
                string query = "select * from XXFS_PRODUCT where prod_id = " + productId;
                DataSet dataSet = OracleHelper.ExecuteDataset(Constants.Application.DBConnConnectionString, CommandType.Text, query);
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        product = new Product
                        {
                            ProductId = row["prod_id"].ToString() != "null" ? Convert.ToInt32(row["prod_id"].ToString()) : 0,
                            ProductName = row["PRODUCT_NAME"].ToString() != "null" ? row["PRODUCT_NAME"].ToString() + " (Id: " + Convert.ToInt32(row["prod_id"].ToString()) + ")" : "",
                            Description = row["Description"].ToString() != "null" ? row["Description"].ToString() : "",
                            Image = row["Product_Image"].ToString() != "null" ? row["Product_Image"].ToString() : "",
                            Quantity = row["Quantity"].ToString() != "null" ? Convert.ToInt32(row["Quantity"].ToString()) : 0,
                            Price = row["Price"].ToString() != "null" ? Convert.ToDecimal(row["Price"].ToString()) : 0,
                            StartDate = row["Start_date"].ToString() != "null" ? Convert.ToDateTime(row["Start_date"].ToString()).ToString("dd MMM yyyy") : "",
                            EndDate = row["End_date"].ToString() != "null" ? row["End_date"].ToString() : "",
                            Type = row["Type"].ToString() != "null" ? row["Type"].ToString() : "",
                            Active = (!string.IsNullOrEmpty(row["Active"].ToString()) && row["Active"].ToString() == "1") ? true : false
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return product;
        }

        public List<Product> GetProducts(List<int> productIds)
        {
            var products = new List<Product>();
            try
            {
                string query = "select * from XXFS_PRODUCT where prod_id in (" + string.Join(",", productIds.ToArray()) + ")";
                DataSet dataSet = OracleHelper.ExecuteDataset(Constants.Application.DBConnConnectionString, CommandType.Text, query);
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        products.Add(new Product
                        {
                            ProductId = row["prod_id"].ToString() != "null" ? Convert.ToInt32(row["prod_id"].ToString()) : 0,
                            ProductName = row["PRODUCT_NAME"].ToString() != "null" ? row["PRODUCT_NAME"].ToString() : "",
                            Description = row["Description"].ToString() != "null" ? row["Description"].ToString() : "",
                            Image = row["Product_Image"].ToString() != "null" ? row["Product_Image"].ToString() : "",
                            Quantity = row["Quantity"].ToString() != "null" ? Convert.ToInt32(row["Quantity"].ToString()) : 0,
                            Price = row["Price"].ToString() != "null" ? Convert.ToDecimal(row["Price"].ToString()) : 0,
                            StartDate = row["Start_date"].ToString() != "null" ? Convert.ToDateTime(row["Start_date"].ToString()).ToString("dd MMM yyyy") : "",
                            EndDate = row["End_date"].ToString() != "null" ? row["End_date"].ToString() : "",
                            Type = row["Type"].ToString() != "null" ? row["Type"].ToString() : "",
                            Active = (!string.IsNullOrEmpty(row["Active"].ToString()) && row["Active"].ToString() == "1") ? true : false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return products;
        }
    }
}