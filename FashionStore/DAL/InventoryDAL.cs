using FashionStore.Code;
using FashionStore.Models;
using log4net;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FashionStore.DAL
{
    public class InventoryDAL
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(InventoryDAL));

        public List<Product> GetProducts()
        {
            var products = new List<Product>();
            try
            {
                string query = "select * from XXFS_PRODUCT";
                DataSet dataSet = OracleHelper.ExecuteDataset(Constants.Application.DBConnConnectionString, CommandType.Text, query);
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        products.Add(new Product
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

        public void RemoveProduct(int productId)
        {
            try
            {
                string query = "delete from XXFS_PRODUCT where prod_id = " + productId;
                DataSet dataSet = OracleHelper.ExecuteDataset(Constants.Application.DBConnConnectionString, CommandType.Text, query);                
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void SaveProduct(Product model)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(Constants.Application.DBConnConnectionString))
                {
                    using (OracleCommand oCmd = new OracleCommand())
                    {
                        oCmd.CommandText = "XXFS_INVENTORY_PKG.sp_insert_products";
                        oCmd.CommandType = CommandType.StoredProcedure;
                        oCmd.Connection = conn;

                        oCmd.Parameters.Add(new OracleParameter("ip_prod_id", model.ProductId));
                        oCmd.Parameters.Add(new OracleParameter("ip_prod_name", model.ProductName));
                        oCmd.Parameters.Add(new OracleParameter("ip_description", model.Description));
                        oCmd.Parameters.Add(new OracleParameter("ip_image", model.Image));
                        oCmd.Parameters.Add(new OracleParameter("ip_quantity", model.Quantity));
                        oCmd.Parameters.Add(new OracleParameter("ip_price", model.Price));
                        oCmd.Parameters.Add(new OracleParameter("ip_active", model.Active ? 1 : 0));
                        oCmd.Parameters.Add(new OracleParameter("ip_type", model.Type));

                        OracleParameter statusOut = new OracleParameter("op_status", OracleDbType.Int32, 100, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(statusOut);

                        OracleParameter messageOut = new OracleParameter("op_message", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(messageOut);

                        conn.Open();
                        oCmd.ExecuteNonQuery();
                        conn.Close();                        
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}