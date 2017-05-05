using FashionStore.Code;
using FashionStore.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace FashionStore.Common
{
    public class Notification
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Notification));

        public void SendWelcomeEmail(Login user)
        {
            try
            {
                string baseURL = Constants.Application.BaseURL.ToString();
                string templatesPath = HttpContext.Current.Server.MapPath("~/Templates");
                var strTemplate = System.IO.File.ReadAllText(templatesPath + @"\WelcomeEmailTemplate"); ;

                var strUserLis = new StringBuilder();

                strTemplate = strTemplate.Replace("{GlobalURL}", baseURL);
                strTemplate = strTemplate.Replace("{ReceiptDate}", DateTime.Now.ToString("dd MMM yyyy"));
                strTemplate = strTemplate.Replace("{USERNAME}", user.FirstName + " " + user.LastName);

                strUserLis.Append("<p>User Name: " + user.UserName + "</p>");
                strUserLis.Append("<p>Password: " + user.Password + "</p>");

                strTemplate = strTemplate.Replace("{DETAILS}", strUserLis.ToString());

                try
                {
                    strTemplate = strTemplate.Replace("{IMAGE}", baseURL + "/Images/logo2.png");
                }
                catch (System.Exception ex)
                {
                    logger.Error(ex);
                }

                string strStatus = string.Empty;

                string strsmtpAddress = System.Configuration.ConfigurationManager.AppSettings["smtpserver"];

                string strsmtpPort = System.Configuration.ConfigurationManager.AppSettings["smtpport"];

                MailMessage message = new MailMessage("fashionstorecustomercare@gmail.com", user.Email);

                message.Subject = "Welcome to Fashion Store";

                message.Body = strTemplate;
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                System.Net.NetworkCredential auth = new System.Net.NetworkCredential("fashionstorecustomercare@gmail.com", "sweetie1989");
                client.Host = strsmtpAddress;
                client.Port = Convert.ToInt32(strsmtpPort);
                client.UseDefaultCredentials = false;
                client.Credentials = auth;
                client.EnableSsl = true;

                client.Send(message);
                message.Dispose();
                client.Dispose();
                strStatus = "Success";
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void SendOrderEmail(Order order)
        {
            try
            {
                string baseURL = Constants.Application.BaseURL.ToString();
                string templatesPath = HttpContext.Current.Server.MapPath("~/Templates");
                var strTemplate = System.IO.File.ReadAllText(templatesPath + @"\OrderEmailTemplate"); ;

                var strOrderLis = new StringBuilder();

                strTemplate = strTemplate.Replace("{GlobalURL}", baseURL);
                strTemplate = strTemplate.Replace("{ReceiptDate}", DateTime.Now.ToString("dd MMM yyyy"));
                strTemplate = strTemplate.Replace("{USERNAME}", Constants.Application.User.FirstName + " " + Constants.Application.User.LastName);

                strOrderLis.Append(string.Format("<p>Your order no. {0} is '{1}'.</p>", order.OrderId, order.OrderStatus));
                //strOrderLis.Append("<br/>");

                strTemplate = strTemplate.Replace("{DETAILS}", strOrderLis.ToString());

                try
                {
                    strTemplate = strTemplate.Replace("{IMAGE}", baseURL + "/Images/logo2.png");
                }
                catch (System.Exception ex)
                {
                    logger.Error(ex);
                }

                string strStatus = string.Empty;

                string strsmtpAddress = System.Configuration.ConfigurationManager.AppSettings["smtpserver"];

                string strsmtpPort = System.Configuration.ConfigurationManager.AppSettings["smtpport"];

                MailMessage message = new MailMessage("fashionstorecustomercare@gmail.com", Constants.Application.User.Email);

                message.Subject = string.Format("Fashion Store - Notification on your order no. {0}", order.OrderId);

                message.Body = strTemplate;
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                System.Net.NetworkCredential auth = new System.Net.NetworkCredential("fashionstorecustomercare@gmail.com", "sweetie1989");
                client.Host = strsmtpAddress;
                client.Port = Convert.ToInt32(strsmtpPort);
                client.UseDefaultCredentials = false;
                client.Credentials = auth;
                client.EnableSsl = true;

                client.Send(message);
                message.Dispose();
                client.Dispose();
                strStatus = "Success";
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void SendOrderStatusEmail(Order order)
        {
            try
            {
                string baseURL = Constants.Application.BaseURL.ToString();
                string templatesPath = HttpContext.Current.Server.MapPath("~/Templates");
                var strTemplate = System.IO.File.ReadAllText(templatesPath + @"\OrderEmailTemplate"); ;

                var strOrderLis = new StringBuilder();

                strTemplate = strTemplate.Replace("{GlobalURL}", baseURL);
                strTemplate = strTemplate.Replace("{ReceiptDate}", DateTime.Now.ToString("dd MMM yyyy"));
                strTemplate = strTemplate.Replace("{USERNAME}", order.CustomerName);

                strOrderLis.Append(string.Format("<p>Your order no. {0} is '{1}'.</p>", order.OrderId, order.OrderStatus));
                //strOrderLis.Append("<br/>");

                strTemplate = strTemplate.Replace("{DETAILS}", strOrderLis.ToString());

                try
                {
                    strTemplate = strTemplate.Replace("{IMAGE}", baseURL + "/Images/logo2.png");
                }
                catch (System.Exception ex)
                {
                    logger.Error(ex);
                }

                string strStatus = string.Empty;

                string strsmtpAddress = System.Configuration.ConfigurationManager.AppSettings["smtpserver"];

                string strsmtpPort = System.Configuration.ConfigurationManager.AppSettings["smtpport"];

                MailMessage message = new MailMessage("fashionstorecustomercare@gmail.com", order.CustomerEmail);

                message.Subject = string.Format("Fashion Store - Notification on your order no. {0}", order.OrderId);

                message.Body = strTemplate;
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                System.Net.NetworkCredential auth = new System.Net.NetworkCredential("fashionstorecustomercare@gmail.com", "sweetie1989");
                client.Host = strsmtpAddress;
                client.Port = Convert.ToInt32(strsmtpPort);
                client.UseDefaultCredentials = false;
                client.Credentials = auth;
                client.EnableSsl = true;

                client.Send(message);
                message.Dispose();
                client.Dispose();
                strStatus = "Success";
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}