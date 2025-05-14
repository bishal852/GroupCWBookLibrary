using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendOrderConfirmationEmailAsync(string email, string name, int orderId, string claimCode, decimal total, string shippingAddress)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var host = smtpSettings["Host"];
            var port = int.Parse(smtpSettings["Port"]);
            var username = smtpSettings["Username"];
            var password = smtpSettings["Password"];
            var fromEmail = smtpSettings["FromEmail"];
            var fromName = smtpSettings["FromName"];

            var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = $"Order Confirmation - Order #{orderId}",
                Body = BuildOrderConfirmationEmail(name, orderId, claimCode, total, shippingAddress),
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(email, name));

            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Log the error but don't throw it to prevent order process from failing
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public async Task SendOrderProcessedEmailAsync(string email, string name, int orderId, decimal total, string shippingAddress, DateTime processedDate)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var host = smtpSettings["Host"];
            var port = int.Parse(smtpSettings["Port"]);
            var username = smtpSettings["Username"];
            var password = smtpSettings["Password"];
            var fromEmail = smtpSettings["FromEmail"];
            var fromName = smtpSettings["FromName"];

            var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = $"Your Order #{orderId} Has Been Processed",
                Body = BuildOrderProcessedEmail(name, orderId, total, shippingAddress, processedDate),
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(email, name));

            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Log the error but don't throw it to prevent order process from failing
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        private string BuildOrderConfirmationEmail(string name, int orderId, string claimCode, decimal total, string shippingAddress)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }");
            sb.AppendLine(".container { max-width: 600px; margin: 0 auto; padding: 20px; }");
            sb.AppendLine(".header { text-align: center; padding: 20px; background-color: #f8f9fa; margin-bottom: 20px; }");
            sb.AppendLine(".content { padding: 20px; }");
            sb.AppendLine(".order-details { margin: 20px 0; padding: 15px; background-color: #f8f9fa; border-radius: 5px; }");
            sb.AppendLine(".claim-code { font-size: 24px; font-weight: bold; color: #007bff; text-align: center; padding: 15px; background-color: #f8f9fa; border-radius: 5px; margin: 20px 0; }");
            sb.AppendLine(".footer { text-align: center; padding: 20px; font-size: 12px; color: #666; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<div class='container'>");
            sb.AppendLine("<div class='header'>");
            sb.AppendLine("<h1>Order Confirmation</h1>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class='content'>");
            sb.AppendLine($"<p>Dear {name},</p>");
            sb.AppendLine($"<p>Thank you for your order! We're pleased to confirm that your order #{orderId} has been received and is being processed.</p>");
            sb.AppendLine("<div class='order-details'>");
            sb.AppendLine($"<p><strong>Order Number:</strong> #{orderId}</p>");
            sb.AppendLine($"<p><strong>Total Amount:</strong> ${total.ToString("0.00")}</p>");
            sb.AppendLine($"<p><strong>Shipping Address:</strong> {shippingAddress}</p>");
            sb.AppendLine("</div>");
            sb.AppendLine("<p>Your claim code for this order is:</p>");
            sb.AppendLine($"<div class='claim-code'>{claimCode}</div>");
            sb.AppendLine("<p>Please keep this code handy when you pick up your order.</p>");
            sb.AppendLine("<p>If you have any questions about your order, please contact our customer service team.</p>");
            sb.AppendLine("<p>Thank you for shopping with us!</p>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class='footer'>");
            sb.AppendLine("<p>This is an automated email. Please do not reply to this message.</p>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        private string BuildOrderProcessedEmail(string name, int orderId, decimal total, string shippingAddress, DateTime processedDate)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }");
            sb.AppendLine(".container { max-width: 600px; margin: 0 auto; padding: 20px; }");
            sb.AppendLine(".header { text-align: center; padding: 20px; background-color: #f8f9fa; margin-bottom: 20px; }");
            sb.AppendLine(".content { padding: 20px; }");
            sb.AppendLine(".order-details { margin: 20px 0; padding: 15px; background-color: #f8f9fa; border-radius: 5px; }");
            sb.AppendLine(".status { font-size: 24px; font-weight: bold; color: #28a745; text-align: center; padding: 15px; background-color: #f8f9fa; border-radius: 5px; margin: 20px 0; }");
            sb.AppendLine(".footer { text-align: center; padding: 20px; font-size: 12px; color: #666; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<div class='container'>");
            sb.AppendLine("<div class='header'>");
            sb.AppendLine("<h1>Order Processed</h1>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class='content'>");
            sb.AppendLine($"<p>Dear {name},</p>");
            sb.AppendLine($"<p>Great news! Your order #{orderId} has been processed and is ready for pickup.</p>");
            sb.AppendLine("<div class='status'>PROCESSED</div>");
            sb.AppendLine("<div class='order-details'>");
            sb.AppendLine($"<p><strong>Order Number:</strong> #{orderId}</p>");
            sb.AppendLine($"<p><strong>Total Amount:</strong> ${total.ToString("0.00")}</p>");
            sb.AppendLine($"<p><strong>Shipping Address:</strong> {shippingAddress}</p>");
            sb.AppendLine($"<p><strong>Processed Date:</strong> {processedDate.ToString("g")}</p>");
            sb.AppendLine("</div>");
            sb.AppendLine("<p>You can pick up your order at our store during business hours. Please bring a valid ID for verification.</p>");
            sb.AppendLine("<p>If you have any questions about your order, please contact our customer service team.</p>");
            sb.AppendLine("<p>Thank you for shopping with us!</p>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div class='footer'>");
            sb.AppendLine("<p>This is an automated email. Please do not reply to this message.</p>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}
