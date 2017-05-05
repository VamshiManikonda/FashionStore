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
    public class CustomerDAL
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(CustomerDAL));

        public Login Register(Login model)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(Constants.Application.DBConnConnectionString))
                {
                    using (OracleCommand oCmd = new OracleCommand())
                    {
                        oCmd.CommandText = "xxfs_customer_pkg.sp_register";
                        oCmd.CommandType = CommandType.StoredProcedure;
                        oCmd.Connection = conn;

                        oCmd.Parameters.Add(new OracleParameter("ip_username", model.UserName));
                        oCmd.Parameters.Add(new OracleParameter("ip_password", model.Password));
                        oCmd.Parameters.Add(new OracleParameter("ip_firstname", model.FirstName));
                        oCmd.Parameters.Add(new OracleParameter("ip_lastname", model.LastName));
                        oCmd.Parameters.Add(new OracleParameter("ip_address", model.Address));
                        oCmd.Parameters.Add(new OracleParameter("ip_city", model.City));
                        oCmd.Parameters.Add(new OracleParameter("ip_state", model.State));
                        oCmd.Parameters.Add(new OracleParameter("ip_country", model.Country));
                        oCmd.Parameters.Add(new OracleParameter("ip_zipcode", model.ZipCode));
                        oCmd.Parameters.Add(new OracleParameter("ip_phone", model.Phone));
                        oCmd.Parameters.Add(new OracleParameter("ip_email", model.Email));
                        oCmd.Parameters.Add(new OracleParameter("ip_admin_user", model.AdminUser ? "Y" : "N"));

                        OracleParameter statusOut = new OracleParameter("op_status", OracleDbType.Int32, 100, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(statusOut);

                        OracleParameter messageOut = new OracleParameter("op_message", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(messageOut);

                        conn.Open();
                        oCmd.ExecuteNonQuery();
                        conn.Close();

                        model.Status = (statusOut.Value.ToString() != "null") ? Convert.ToInt32(statusOut.Value.ToString()) : 0;
                        model.Message = (messageOut.Value.ToString() != "null") ? messageOut.Value.ToString() : "";
                    }
                }
            }
            catch (Exception ex)
            {
                model.Status = 1;
                model.Message = "Please enter required fields.";
                logger.Error(ex);
            }
            return model;
        }

        public User Update(User model)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(Constants.Application.DBConnConnectionString))
                {
                    using (OracleCommand oCmd = new OracleCommand())
                    {
                        oCmd.CommandText = "xxfs_customer_pkg.sp_update";
                        oCmd.CommandType = CommandType.StoredProcedure;
                        oCmd.Connection = conn;

                        oCmd.Parameters.Add(new OracleParameter("ip_id", model.Id));
                        oCmd.Parameters.Add(new OracleParameter("ip_firstname", model.FirstName));
                        oCmd.Parameters.Add(new OracleParameter("ip_lastname", model.LastName));
                        oCmd.Parameters.Add(new OracleParameter("ip_address", model.Address));
                        oCmd.Parameters.Add(new OracleParameter("ip_city", model.City));
                        oCmd.Parameters.Add(new OracleParameter("ip_state", model.State));
                        oCmd.Parameters.Add(new OracleParameter("ip_country", model.Country));
                        oCmd.Parameters.Add(new OracleParameter("ip_zipcode", model.ZipCode));
                        oCmd.Parameters.Add(new OracleParameter("ip_phone", model.Phone));
                        oCmd.Parameters.Add(new OracleParameter("ip_email", model.Email));

                        OracleParameter statusOut = new OracleParameter("op_status", OracleDbType.Int32, 100, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(statusOut);

                        OracleParameter messageOut = new OracleParameter("op_message", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(messageOut);

                        conn.Open();
                        oCmd.ExecuteNonQuery();
                        conn.Close();

                        model.Status = (statusOut.Value.ToString() != "null") ? Convert.ToInt32(statusOut.Value.ToString()) : 0;
                        model.Message = (messageOut.Value.ToString() != "null") ? messageOut.Value.ToString() : "";
                    }
                }
            }
            catch (Exception ex)
            {
                model.Status = 1;
                model.Message = "Please enter required fields.";
                logger.Error(ex);
            }
            return model;
        }

        public Login Login(Login model)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(Constants.Application.DBConnConnectionString))
                {
                    using (OracleCommand oCmd = new OracleCommand())
                    {
                        oCmd.CommandText = "xxfs_customer_pkg.sp_login";
                        oCmd.CommandType = CommandType.StoredProcedure;
                        oCmd.Connection = conn;

                        oCmd.Parameters.Add(new OracleParameter("ip_username", model.UserName));
                        oCmd.Parameters.Add(new OracleParameter("ip_password", model.Password));

                        OracleParameter idOut = new OracleParameter("op_id", OracleDbType.Int32, 100, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(idOut);

                        OracleParameter firstOut = new OracleParameter("op_firstname", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(firstOut);

                        OracleParameter lastOut = new OracleParameter("op_lastname", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(lastOut);

                        OracleParameter addressOut = new OracleParameter("op_address", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(addressOut);

                        OracleParameter cityOut = new OracleParameter("op_city", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(cityOut);

                        OracleParameter stateOut = new OracleParameter("op_state", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(stateOut);

                        OracleParameter countryOut = new OracleParameter("op_country", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(countryOut);

                        OracleParameter zipOut = new OracleParameter("op_zipcode", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(zipOut);

                        OracleParameter phoneOut = new OracleParameter("op_phone", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(phoneOut);

                        OracleParameter emailOut = new OracleParameter("op_email", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(emailOut);

                        OracleParameter adminOut = new OracleParameter("op_admin_user", OracleDbType.Int32, 100, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(adminOut);

                        OracleParameter statusOut = new OracleParameter("op_status", OracleDbType.Int32, 100, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(statusOut);

                        OracleParameter messageOut = new OracleParameter("op_message", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(messageOut);

                        conn.Open();
                        oCmd.ExecuteNonQuery();

                        model.User = new User
                        {
                            Id = (idOut.Value.ToString() != "null") ? Convert.ToInt32(idOut.Value.ToString()) : 0,
                            UserName = model.UserName,
                            Password = model.Password,
                            FirstName = (firstOut.Value.ToString() != "null") ? firstOut.Value.ToString() : "",
                            LastName = (lastOut.Value.ToString() != "null") ? lastOut.Value.ToString() : "",
                            Address = (addressOut.Value.ToString() != "null") ? addressOut.Value.ToString() : "",
                            City = (cityOut.Value.ToString() != "null") ? cityOut.Value.ToString() : "",
                            State = (stateOut.Value.ToString() != "null") ? stateOut.Value.ToString() : "",
                            Country = (countryOut.Value.ToString() != "null") ? countryOut.Value.ToString() : "",
                            ZipCode = (zipOut.Value.ToString() != "null") ? zipOut.Value.ToString() : "",
                            Phone = (phoneOut.Value.ToString() != "null") ? phoneOut.Value.ToString() : "",
                            Email = (emailOut.Value.ToString() != "null") ? emailOut.Value.ToString() : "",
                            AdminUser = (adminOut.Value.ToString() != "null" && adminOut.Value.ToString() == "1") ? true : false
                        };

                        model.Status = (statusOut.Value.ToString() != "null") ? Convert.ToInt32(statusOut.Value.ToString()) : 0;
                        model.Message = (messageOut.Value.ToString() != "null") ? messageOut.Value.ToString() : "";
                    }
                }
            }
            catch (Exception ex)
            {
                model.Status = 1;
                model.Message = "Please enter required fields.";
                logger.Error(ex);
            }
            return model;
        }

        public Login Reset(Login model)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(Constants.Application.DBConnConnectionString))
                {
                    using (OracleCommand oCmd = new OracleCommand())
                    {
                        oCmd.CommandText = "xxfs_customer_pkg.sp_reset";
                        oCmd.CommandType = CommandType.StoredProcedure;
                        oCmd.Connection = conn;

                        oCmd.Parameters.Add(new OracleParameter("ip_username", model.UserName));
                        oCmd.Parameters.Add(new OracleParameter("ip_password", model.Password));

                        OracleParameter statusOut = new OracleParameter("op_status", OracleDbType.Int32, 100, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(statusOut);

                        OracleParameter messageOut = new OracleParameter("op_message", OracleDbType.Varchar2, 2000, null, ParameterDirection.Output);
                        oCmd.Parameters.Add(messageOut);

                        conn.Open();
                        oCmd.ExecuteNonQuery();
                        conn.Close();

                        model.Status = (statusOut.Value.ToString() != "null") ? Convert.ToInt32(statusOut.Value.ToString()) : 0;
                        model.Message = (messageOut.Value.ToString() != "null") ? messageOut.Value.ToString() : "";
                    }
                }
            }
            catch (Exception ex)
            {
                model.Status = 1;
                model.Message = "Please enter required fields.";
                logger.Error(ex);
            }
            return model;
        }
    }
}