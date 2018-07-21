using System;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System.Configuration;
using System.Net.Mail;

namespace RazorEngine_Test
{
    public static class TemplatedErrorEmail
    {
        public static void SendExceptionEmail(ExceptionEmailClass exceptionEmailClass)
        {
            try
            {
                var config = new TemplateServiceConfiguration
                {
                    TemplateManager = new ResolvePathTemplateManager(new[] { "EmailTemplates" }),
                    DisableTempFileLocking = true
                };

                Engine.Razor = RazorEngineService.Create(config);
                var emailHtmlBody = Engine.Razor.RunCompile("ExceptionEmail.cshtml", null, exceptionEmailClass);

                SendFinal(emailHtmlBody, $"Exception occurred in Application {exceptionEmailClass.ExceptionSource}");
            }
            catch (Exception e)
            {
                Logger.Error($"Error Details: {e.Message}\r\nStack: {e.StackTrace}");
            }
        }

        public static void SendFinal(string emailHtmlBody, string subject)
        {
            var email = new MailMessage()
            {
                Body = emailHtmlBody,
                IsBodyHtml = true,
                Subject = subject,
            };

            string[] emails = ConfigurationManager.AppSettings["ToMail"].Split(',');
            foreach (string emailAddr in emails)
            {
                email.To.Add(new MailAddress(emailAddr));
            }

            var smtpClient = new SmtpClient();

            smtpClient.Send(email);
        }
    }
}
