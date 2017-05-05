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
using System.Web.Mvc;

namespace FashionStore.DAL
{
    public class OrderDAL
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(OrderDAL));
        public Order GetCart(List<OrderCart> orderCarts)
        {
            var order = new Order();
            try
            {
                foreach (var orderCart in orderCarts)
                {
                    string query = "select * from XXFS_PRODUCT where prod_id = " + orderCart.ProductId;
                    DataSet dataSet = OracleHelper.ExecuteDataset(Constants.Application.DBConnConnectionString, CommandType.Text, query);
                    if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in dataSet.Tables[0].Rows)
                        {
                            var cart = new OrderCart
                            {
                                ProductId = row["prod_id"].ToString() != "null" ? Convert.ToInt32(row["prod_id"].ToString()) : 0,
                                ProductName = row["PRODUCT_NAME"].ToString() != "null" ? row["PRODUCT_NAME"].ToString() + " (Id: " + Convert.ToInt32(row["prod_id"].ToString()) + ")" : "",
                                ProductImage = row["Product_Image"].ToString() != "null" ? row["Product_Image"].ToString() : "",
                                Quantity = orderCart.Quantity,
                                ItemQuantity = orderCart.Quantity.ToString(),
                                Price = row["Price"].ToString() != "null" ? Convert.ToDecimal(row["Price"].ToString()) : 0,
                                TotalPrice = orderCart.Quantity * Convert.ToDecimal(row["Price"].ToString()),
                            };
                            var totalQuantity = row["Quantity"].ToString() != "null" ? Convert.ToInt32(row["Quantity"].ToString()) : 0;
                            var quantities = new List<SelectListItem>();
                            for(int i = 1; i <= totalQuantity; i++)
                            {
                                quantities.Add(new SelectListItem
                                {
                                    Value = i.ToString(),
                                    Text = i.ToString()
                                });
                            }                            
                            cart.Quantities = quantities;
                            order.OrderCarts.Add(cart);
                        }
                    }    
                    
                }
                if (order.OrderCarts != null && order.OrderCarts.Count > 0)
                {
                    order.SubTotal = order.OrderCarts.Sum(x => x.TotalPrice);
                    order.Total = order.OrderCarts.Sum(x => x.TotalPrice);
                }                
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return order;
        }

        public void GetCartDetails(Order order)
        {
            var orderCartList = new List<OrderCart>();
            try
            {
                foreach (var orderCart in order.OrderCarts)
                {
                    string query = "select * from XXFS_PRODUCT where prod_id = " + orderCart.ProductId;
                    DataSet dataSet = OracleHelper.ExecuteDataset(Constants.Application.DBConnConnectionString, CommandType.Text, query);
                    if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in dataSet.Tables[0].Rows)
                        {
                            orderCartList.Add(new OrderCart
                            {
                                ProductId = row["prod_id"].ToString() != "null" ? Convert.ToInt32(row["prod_id"].ToString()) : 0,
                                ProductName = row["PRODUCT_NAME"].ToString() != "null" ? row["PRODUCT_NAME"].ToString() + " (Id: " + Convert.ToInt32(row["prod_id"].ToString()) + ")" : "",
                                ProductImage = row["Product_Image"].ToString() != "null" ? row["Product_Image"].ToString() : "",
                                Quantity = orderCart.Quantity,
                                CartId = orderCart.CartId,
                                OrderId = orderCart.OrderId,
                                ItemQuantity = orderCart.Quantity.ToString(),
                                Price = row["Price"].ToString() != "null" ? Convert.ToDecimal(row["Price"].ToString()) : 0,
                                TotalPrice = orderCart.Quantity * Convert.ToDecimal(row["Price"].ToString()),
                            });                         
                        }
                    }

                }
                order.OrderCarts = orderCartList;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public OrderManager Confirm(Order model)
        {
            var order = new OrderManager();
            try
            {
                using (OracleConnection conn = new OracleConnection(Constants.Application.DBConnConnectionString))
                {
                    using (OracleCommand oCmd = new OracleCommand())
                    {
                        oCmd.CommandText = "XXFS_ORDER_PKG.sp_insert_order";
                        oCmd.CommandType = CommandType.StoredProcedure;
                        oCmd.Connection = conn;

                        oCmd.Parameters.Add(new OracleParameter("ip_quantity", model.Quantity));
                        oCmd.Parameters.Add(new OracleParameter("ip_customer_id", Constants.Application.User.Id));
                        oCmd.Parameters.Add(new OracleParameter("ip_sub_total", model.SubTotal));
                        oCmd.Parameters.Add(new OracleParameter("ip_total", model.Total));
                        oCmd.Parameters.Add(new OracleParameter("ip_firstname", model.FirstName));
                        oCmd.Parameters.Add(new OracleParameter("ip_lastname", model.LastName));
                        oCmd.Parameters.Add(new OracleParameter("ip_address", model.Address));
                        oCmd.Parameters.Add(new OracleParameter("ip_city", model.City));
                        oCmd.Parameters.Add(new OracleParameter("ip_state", model.State));
                        oCmd.Parameters.Add(new OracleParameter("ip_country", model.Country));
                        oCmd.Parameters.Add(new OracleParameter("ip_zipcode", model.ZipCode));
                        oCmd.Parameters.Add(new OracleParameter("ip_phone", model.Phone));
                        oCmd.Parameters.Add(new OracleParameter("ip_email", model.Email));
                        oCmd.Parameters.Add(new OracleParameter("ip_payment_type", model.Paypal ? "Paypal" : "Cash"));

                        OracleParameter orderOut = new OracleParameter("op_order_no", OracleDbType.Int32, 100, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(orderOut);

                        OracleParameter ordersOut = new OracleParameter("op_orders", OracleDbType.RefCursor, ParameterDirection.Output);
                        oCmd.Parameters.Add(ordersOut);

                        OracleParameter statusOut = new OracleParameter("op_status", OracleDbType.Int32, 100, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(statusOut);

                        OracleParameter messageOut = new OracleParameter("op_message", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(messageOut);

                        conn.Open();
                        oCmd.ExecuteNonQuery();

                        model.OrderId = (orderOut.Value.ToString() != "null") ? Convert.ToInt32(orderOut.Value.ToString()) : 0;
                        model.Status = (statusOut.Value.ToString() != "null") ? Convert.ToInt32(statusOut.Value.ToString()) : 0;
                        model.Message = (messageOut.Value.ToString() != "null") ? messageOut.Value.ToString() : "";

                        if (!((OracleRefCursor)ordersOut.Value).IsNull)
                        {
                            OracleDataReader dr = ((OracleRefCursor)ordersOut.Value).GetDataReader();
                            while (dr.Read())
                            {
                                if(dr["STATUS"].ToString() != "null" && dr["STATUS"].ToString() == "In process")
                                {
                                    order.CurrentOrders.Add(new Order
                                    {
                                        OrderId = dr["ORDER_NO"].ToString() != "null" ? Convert.ToInt32(dr["ORDER_NO"].ToString()) : 0,
                                        OrderStatus = dr["STATUS"].ToString() != "null" ? dr["STATUS"].ToString() : "",
                                        SubTotal = dr["SUB_TOTAL"].ToString() != "null" ? Convert.ToDecimal(dr["SUB_TOTAL"].ToString()) : 0,
                                        Total = dr["TOTAL"].ToString() != "null" ? Convert.ToDecimal(dr["TOTAL"].ToString()) : 0,
                                        CreatedDate = dr["CREATED_DATE"].ToString() != "null" ? Convert.ToDateTime(dr["CREATED_DATE"].ToString()).ToString("dd MMM yyyy") : "",
                                        Quantity = dr["Quantity"].ToString() != "null" ? Convert.ToInt32(dr["Quantity"].ToString()) : 0,
                                        FirstName = dr["SHIP_FIRST_NAME"].ToString() != "null" ? dr["SHIP_FIRST_NAME"].ToString() : "",
                                        LastName = dr["SHIP_LAST_NAME"].ToString() != "null" ? dr["SHIP_LAST_NAME"].ToString() : "",
                                        Address = dr["SHIP_ADDRESS"].ToString() != "null" ? dr["SHIP_ADDRESS"].ToString() : "",
                                        City = dr["SHIP_CITY"].ToString() != "null" ? dr["SHIP_CITY"].ToString() : "",
                                        State = dr["SHIP_STATE"].ToString() != "null" ? dr["SHIP_STATE"].ToString() : "",
                                        Country = dr["SHIP_COUNTRY"].ToString() != "null" ? dr["SHIP_COUNTRY"].ToString() : "",
                                        ZipCode = dr["SHIP_ZIPCODE"].ToString() != "null" ? dr["SHIP_ZIPCODE"].ToString() : "",
                                        Phone = dr["SHIP_PHONE"].ToString() != "null" ? dr["SHIP_PHONE"].ToString() : "",
                                        Email = dr["SHIP_EMAIL"].ToString() != "null" ? dr["SHIP_EMAIL"].ToString() : "",
                                        CardType = dr["CARD_TYPE"].ToString() != "null" && dr["CARD_TYPE"].ToString() == "Paypal" ? "Paypal" : "Cash"
                                    });
                                }
                                else
                                {
                                    order.ProcessedOrders.Add(new Order
                                    {
                                        OrderId = dr["ORDER_NO"].ToString() != "null" ? Convert.ToInt32(dr["ORDER_NO"].ToString()) : 0,
                                        OrderStatus = dr["STATUS"].ToString() != "null" ? dr["STATUS"].ToString() : "",
                                        SubTotal = dr["SUB_TOTAL"].ToString() != "null" ? Convert.ToDecimal(dr["SUB_TOTAL"].ToString()) : 0,
                                        Total = dr["TOTAL"].ToString() != "null" ? Convert.ToDecimal(dr["TOTAL"].ToString()) : 0,
                                        CreatedDate = dr["CREATED_DATE"].ToString() != "null" ? Convert.ToDateTime(dr["CREATED_DATE"].ToString()).ToString("dd MMM yyyy") : "",
                                        Quantity = dr["Quantity"].ToString() != "null" ? Convert.ToInt32(dr["Quantity"].ToString()) : 0,
                                        FirstName = dr["SHIP_FIRST_NAME"].ToString() != "null" ? dr["SHIP_FIRST_NAME"].ToString() : "",
                                        LastName = dr["SHIP_LAST_NAME"].ToString() != "null" ? dr["SHIP_LAST_NAME"].ToString() : "",
                                        Address = dr["SHIP_ADDRESS"].ToString() != "null" ? dr["SHIP_ADDRESS"].ToString() : "",
                                        City = dr["SHIP_CITY"].ToString() != "null" ? dr["SHIP_CITY"].ToString() : "",
                                        State = dr["SHIP_STATE"].ToString() != "null" ? dr["SHIP_STATE"].ToString() : "",
                                        Country = dr["SHIP_COUNTRY"].ToString() != "null" ? dr["SHIP_COUNTRY"].ToString() : "",
                                        ZipCode = dr["SHIP_ZIPCODE"].ToString() != "null" ? dr["SHIP_ZIPCODE"].ToString() : "",
                                        Phone = dr["SHIP_PHONE"].ToString() != "null" ? dr["SHIP_PHONE"].ToString() : "",
                                        Email = dr["SHIP_EMAIL"].ToString() != "null" ? dr["SHIP_EMAIL"].ToString() : "",
                                        CardType = dr["CARD_TYPE"].ToString() != "null" && dr["CARD_TYPE"].ToString() == "Paypal" ? "Paypal" : "Cash"
                                    });
                                }
                            }
                        }

                        InsertOrderCart(model);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return order;
        }

        public Order GetOrder(int orderId)
        {
            var order = new Order();
            try
            {
                using (OracleConnection conn = new OracleConnection(Constants.Application.DBConnConnectionString))
                {
                    using (OracleCommand oCmd = new OracleCommand())
                    {
                        oCmd.CommandText = "XXFS_ORDER_PKG.sp_get_order";
                        oCmd.CommandType = CommandType.StoredProcedure;
                        oCmd.Connection = conn;

                        oCmd.Parameters.Add(new OracleParameter("ip_order_no", orderId));

                        OracleParameter nameOut = new OracleParameter("op_customer_name", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(nameOut);

                        OracleParameter emailOut = new OracleParameter("op_email", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(emailOut);

                        OracleParameter ordersOut = new OracleParameter("op_orders", OracleDbType.RefCursor, ParameterDirection.Output);
                        oCmd.Parameters.Add(ordersOut);

                        OracleParameter orderCartOut = new OracleParameter("op_order_items", OracleDbType.RefCursor, ParameterDirection.Output);
                        oCmd.Parameters.Add(orderCartOut);

                        OracleParameter statusOut = new OracleParameter("op_status", OracleDbType.Int32, 100, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(statusOut);

                        OracleParameter messageOut = new OracleParameter("op_message", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(messageOut);

                        conn.Open();
                        oCmd.ExecuteNonQuery();

                        order.CustomerName = (nameOut.Value.ToString() != "null") ? nameOut.Value.ToString() : "";
                        order.CustomerEmail = (emailOut.Value.ToString() != "null") ? emailOut.Value.ToString() : "";
                        order.Status = (statusOut.Value.ToString() != "null") ? Convert.ToInt32(statusOut.Value.ToString()) : 0;
                        order.Message = (messageOut.Value.ToString() != "null") ? messageOut.Value.ToString() : "";

                        if (!((OracleRefCursor)orderCartOut.Value).IsNull)
                        {
                            OracleDataReader dr = ((OracleRefCursor)orderCartOut.Value).GetDataReader();
                            while (dr.Read())
                            {
                                order.OrderCarts.Add(new OrderCart
                                {
                                    CartId = dr["ORDER_ITEM_ID"].ToString() != "null" ? Convert.ToInt32(dr["ORDER_ITEM_ID"].ToString()) : 0,
                                    ProductId = dr["PROD_ID"].ToString() != "null" ? Convert.ToInt32(dr["PROD_ID"].ToString()) : 0,
                                    Quantity = dr["QUANTITY"].ToString() != "null" ? Convert.ToInt32(dr["QUANTITY"].ToString()) : 0,
                                    OrderId = dr["ORDER_NO"].ToString() != "null" ? Convert.ToInt32(dr["ORDER_NO"].ToString()) : 0,
                                });
                            }
                        }
                        GetCartDetails(order);

                        if (!((OracleRefCursor)ordersOut.Value).IsNull)
                        {
                            OracleDataReader dr = ((OracleRefCursor)ordersOut.Value).GetDataReader();
                            while (dr.Read())
                            {
                                order.OrderId = dr["ORDER_NO"].ToString() != "null" ? Convert.ToInt32(dr["ORDER_NO"].ToString()) : 0;
                                order.OrderStatus = dr["STATUS"].ToString() != "null" ? dr["STATUS"].ToString() : "";
                                order.SubTotal = dr["SUB_TOTAL"].ToString() != "null" ? Convert.ToDecimal(dr["SUB_TOTAL"].ToString()) : 0;
                                order.Total = dr["TOTAL"].ToString() != "null" ? Convert.ToDecimal(dr["TOTAL"].ToString()) : 0;
                                order.CreatedDate = dr["CREATED_DATE"].ToString() != "null" ? Convert.ToDateTime(dr["CREATED_DATE"].ToString()).ToString("dd MMM yyyy") : "";
                                order.Quantity = dr["Quantity"].ToString() != "null" ? Convert.ToInt32(dr["Quantity"].ToString()) : 0;
                                order.FirstName = dr["SHIP_FIRST_NAME"].ToString() != "null" ? dr["SHIP_FIRST_NAME"].ToString() : "";
                                order.LastName = dr["SHIP_LAST_NAME"].ToString() != "null" ? dr["SHIP_LAST_NAME"].ToString() : "";
                                order.Address = dr["SHIP_ADDRESS"].ToString() != "null" ? dr["SHIP_ADDRESS"].ToString() : "";
                                order.City = dr["SHIP_CITY"].ToString() != "null" ? dr["SHIP_CITY"].ToString() : "";
                                order.State = dr["SHIP_STATE"].ToString() != "null" ? dr["SHIP_STATE"].ToString() : "";
                                order.Country = dr["SHIP_COUNTRY"].ToString() != "null" ? dr["SHIP_COUNTRY"].ToString() : "";
                                order.ZipCode = dr["SHIP_ZIPCODE"].ToString() != "null" ? dr["SHIP_ZIPCODE"].ToString() : "";
                                order.Phone = dr["SHIP_PHONE"].ToString() != "null" ? dr["SHIP_PHONE"].ToString() : "";
                                order.Email = dr["SHIP_EMAIL"].ToString() != "null" ? dr["SHIP_EMAIL"].ToString() : "";
                                order.CardType = dr["CARD_TYPE"].ToString() != "null" && dr["CARD_TYPE"].ToString() == "Paypal" ? "Paypal" : "Cash";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return order;
        }

        public OrderManager MyOrders()
        {
            var order = new OrderManager();
            try
            {
                string query = "select * from xxfs_order where customer_id = " + Constants.Application.User.Id;
                DataSet dataSet = OracleHelper.ExecuteDataset(Constants.Application.DBConnConnectionString, CommandType.Text, query);
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        if (row["STATUS"].ToString() != "null" && row["STATUS"].ToString() == "In process")
                        {
                            order.CurrentOrders.Add(new Order
                            {
                                OrderId = row["ORDER_NO"].ToString() != "null" ? Convert.ToInt32(row["ORDER_NO"].ToString()) : 0,
                                OrderStatus = row["STATUS"].ToString() != "null" ? row["STATUS"].ToString() : "",
                                SubTotal = row["SUB_TOTAL"].ToString() != "null" ? Convert.ToDecimal(row["SUB_TOTAL"].ToString()) : 0,
                                Total = row["TOTAL"].ToString() != "null" ? Convert.ToDecimal(row["TOTAL"].ToString()) : 0,
                                CreatedDate = row["CREATED_DATE"].ToString() != "null" ? Convert.ToDateTime(row["CREATED_DATE"].ToString()).ToString("dd MMM yyyy") : "",
                                Quantity = row["Quantity"].ToString() != "null" ? Convert.ToInt32(row["Quantity"].ToString()) : 0,
                                FirstName = row["SHIP_FIRST_NAME"].ToString() != "null" ? row["SHIP_FIRST_NAME"].ToString() : "",
                                LastName = row["SHIP_LAST_NAME"].ToString() != "null" ? row["SHIP_LAST_NAME"].ToString() : "",
                                Address = row["SHIP_ADDRESS"].ToString() != "null" ? row["SHIP_ADDRESS"].ToString() : "",
                                City = row["SHIP_CITY"].ToString() != "null" ? row["SHIP_CITY"].ToString() : "",
                                State = row["SHIP_STATE"].ToString() != "null" ? row["SHIP_STATE"].ToString() : "",
                                Country = row["SHIP_COUNTRY"].ToString() != "null" ? row["SHIP_COUNTRY"].ToString() : "",
                                ZipCode = row["SHIP_ZIPCODE"].ToString() != "null" ? row["SHIP_ZIPCODE"].ToString() : "",
                                Phone = row["SHIP_PHONE"].ToString() != "null" ? row["SHIP_PHONE"].ToString() : "",
                                Email = row["SHIP_EMAIL"].ToString() != "null" ? row["SHIP_EMAIL"].ToString() : "",
                                CardType = row["CARD_TYPE"].ToString() != "null" && row["CARD_TYPE"].ToString() == "Paypal" ? "Paypal" : "Cash"
                            });
                        }
                        else
                        {
                            order.ProcessedOrders.Add(new Order
                            {
                                OrderId = row["ORDER_NO"].ToString() != "null" ? Convert.ToInt32(row["ORDER_NO"].ToString()) : 0,
                                OrderStatus = row["STATUS"].ToString() != "null" ? row["STATUS"].ToString() : "",
                                SubTotal = row["SUB_TOTAL"].ToString() != "null" ? Convert.ToDecimal(row["SUB_TOTAL"].ToString()) : 0,
                                Total = row["TOTAL"].ToString() != "null" ? Convert.ToDecimal(row["TOTAL"].ToString()) : 0,
                                CreatedDate = row["CREATED_DATE"].ToString() != "null" ? Convert.ToDateTime(row["CREATED_DATE"].ToString()).ToString("dd MMM yyyy") : "",
                                Quantity = row["Quantity"].ToString() != "null" ? Convert.ToInt32(row["Quantity"].ToString()) : 0,
                                FirstName = row["SHIP_FIRST_NAME"].ToString() != "null" ? row["SHIP_FIRST_NAME"].ToString() : "",
                                LastName = row["SHIP_LAST_NAME"].ToString() != "null" ? row["SHIP_LAST_NAME"].ToString() : "",
                                Address = row["SHIP_ADDRESS"].ToString() != "null" ? row["SHIP_ADDRESS"].ToString() : "",
                                City = row["SHIP_CITY"].ToString() != "null" ? row["SHIP_CITY"].ToString() : "",
                                State = row["SHIP_STATE"].ToString() != "null" ? row["SHIP_STATE"].ToString() : "",
                                Country = row["SHIP_COUNTRY"].ToString() != "null" ? row["SHIP_COUNTRY"].ToString() : "",
                                ZipCode = row["SHIP_ZIPCODE"].ToString() != "null" ? row["SHIP_ZIPCODE"].ToString() : "",
                                Phone = row["SHIP_PHONE"].ToString() != "null" ? row["SHIP_PHONE"].ToString() : "",
                                Email = row["SHIP_EMAIL"].ToString() != "null" ? row["SHIP_EMAIL"].ToString() : "",
                                CardType = row["CARD_TYPE"].ToString() != "null" && row["CARD_TYPE"].ToString() == "Paypal" ? "Paypal" : "Cash"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return order;
        }

        public OrderManager GetOrders()
        {
            var order = new OrderManager();
            try
            {
                string query = "select * from xxfs_order";
                DataSet dataSet = OracleHelper.ExecuteDataset(Constants.Application.DBConnConnectionString, CommandType.Text, query);
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        if (row["STATUS"].ToString() != "null" && row["STATUS"].ToString() == "In process")
                        {
                            order.CurrentOrders.Add(new Order
                            {
                                OrderId = row["ORDER_NO"].ToString() != "null" ? Convert.ToInt32(row["ORDER_NO"].ToString()) : 0,
                                OrderStatus = row["STATUS"].ToString() != "null" ? row["STATUS"].ToString() : "",
                                CustomerId = row["CUSTOMER_ID"].ToString() != "null" ? Convert.ToInt32(row["CUSTOMER_ID"].ToString()) : 0,
                                SubTotal = row["SUB_TOTAL"].ToString() != "null" ? Convert.ToDecimal(row["SUB_TOTAL"].ToString()) : 0,
                                Total = row["TOTAL"].ToString() != "null" ? Convert.ToDecimal(row["TOTAL"].ToString()) : 0,
                                CreatedDate = row["CREATED_DATE"].ToString() != "null" ? Convert.ToDateTime(row["CREATED_DATE"].ToString()).ToString("dd MMM yyyy") : "",
                                Quantity = row["Quantity"].ToString() != "null" ? Convert.ToInt32(row["Quantity"].ToString()) : 0,
                                FirstName = row["SHIP_FIRST_NAME"].ToString() != "null" ? row["SHIP_FIRST_NAME"].ToString() : "",
                                LastName = row["SHIP_LAST_NAME"].ToString() != "null" ? row["SHIP_LAST_NAME"].ToString() : "",
                                Address = row["SHIP_ADDRESS"].ToString() != "null" ? row["SHIP_ADDRESS"].ToString() : "",
                                City = row["SHIP_CITY"].ToString() != "null" ? row["SHIP_CITY"].ToString() : "",
                                State = row["SHIP_STATE"].ToString() != "null" ? row["SHIP_STATE"].ToString() : "",
                                Country = row["SHIP_COUNTRY"].ToString() != "null" ? row["SHIP_COUNTRY"].ToString() : "",
                                ZipCode = row["SHIP_ZIPCODE"].ToString() != "null" ? row["SHIP_ZIPCODE"].ToString() : "",
                                Phone = row["SHIP_PHONE"].ToString() != "null" ? row["SHIP_PHONE"].ToString() : "",
                                Email = row["SHIP_EMAIL"].ToString() != "null" ? row["SHIP_EMAIL"].ToString() : "",
                                CardType = row["CARD_TYPE"].ToString() != "null" && row["CARD_TYPE"].ToString() == "Paypal" ? "Paypal" : "Cash"
                            });
                        }
                        else
                        {
                            order.ProcessedOrders.Add(new Order
                            {
                                OrderId = row["ORDER_NO"].ToString() != "null" ? Convert.ToInt32(row["ORDER_NO"].ToString()) : 0,
                                OrderStatus = row["STATUS"].ToString() != "null" ? row["STATUS"].ToString() : "",
                                CustomerId = row["CUSTOMER_ID"].ToString() != "null" ? Convert.ToInt32(row["CUSTOMER_ID"].ToString()) : 0,
                                SubTotal = row["SUB_TOTAL"].ToString() != "null" ? Convert.ToDecimal(row["SUB_TOTAL"].ToString()) : 0,
                                Total = row["TOTAL"].ToString() != "null" ? Convert.ToDecimal(row["TOTAL"].ToString()) : 0,
                                CreatedDate = row["CREATED_DATE"].ToString() != "null" ? Convert.ToDateTime(row["CREATED_DATE"].ToString()).ToString("dd MMM yyyy") : "",
                                Quantity = row["Quantity"].ToString() != "null" ? Convert.ToInt32(row["Quantity"].ToString()) : 0,
                                FirstName = row["SHIP_FIRST_NAME"].ToString() != "null" ? row["SHIP_FIRST_NAME"].ToString() : "",
                                LastName = row["SHIP_LAST_NAME"].ToString() != "null" ? row["SHIP_LAST_NAME"].ToString() : "",
                                Address = row["SHIP_ADDRESS"].ToString() != "null" ? row["SHIP_ADDRESS"].ToString() : "",
                                City = row["SHIP_CITY"].ToString() != "null" ? row["SHIP_CITY"].ToString() : "",
                                State = row["SHIP_STATE"].ToString() != "null" ? row["SHIP_STATE"].ToString() : "",
                                Country = row["SHIP_COUNTRY"].ToString() != "null" ? row["SHIP_COUNTRY"].ToString() : "",
                                ZipCode = row["SHIP_ZIPCODE"].ToString() != "null" ? row["SHIP_ZIPCODE"].ToString() : "",
                                Phone = row["SHIP_PHONE"].ToString() != "null" ? row["SHIP_PHONE"].ToString() : "",
                                Email = row["SHIP_EMAIL"].ToString() != "null" ? row["SHIP_EMAIL"].ToString() : "",
                                CardType = row["CARD_TYPE"].ToString() != "null" && row["CARD_TYPE"].ToString() == "Paypal" ? "Paypal" : "Cash"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return order;
        }

        public void CancelOrder(int orderId)
        {
            try
            {
                string query = "update xxfs_order set status = 'Cancelled' where ORDER_NO = " + orderId;
                DataSet dataSet = OracleHelper.ExecuteDataset(Constants.Application.DBConnConnectionString, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void ProcessOrder(int orderId)
        {
            try
            {
                string query = "update xxfs_order set status = 'Processed' where ORDER_NO = " + orderId;
                DataSet dataSet = OracleHelper.ExecuteDataset(Constants.Application.DBConnConnectionString, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void InsertOrderCart(Order model)
        {
            try
            {
                foreach(var orderCart in model.OrderCarts)
                {
                    using (OracleConnection conn = new OracleConnection(Constants.Application.DBConnConnectionString))
                    {
                        using (OracleCommand oCmd = new OracleCommand())
                        {
                            oCmd.CommandText = "XXFS_ORDER_PKG.sp_insert_order_item";
                            oCmd.CommandType = CommandType.StoredProcedure;
                            oCmd.Connection = conn;

                            oCmd.Parameters.Add(new OracleParameter("ip_prod_id", orderCart.ProductId));
                            oCmd.Parameters.Add(new OracleParameter("ip_price", orderCart.Price));
                            oCmd.Parameters.Add(new OracleParameter("ip_quantity", orderCart.Quantity));
                            oCmd.Parameters.Add(new OracleParameter("ip_order_no", model.OrderId));

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
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}