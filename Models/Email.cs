//using Microsoft.AspNetCore.Identity.UI.Services;
//using SendGrid.Helpers.Mail;
//using SendGrid;
//using System.ComponentModel.DataAnnotations;
//using System.Net.Mail;
//using Microsoft.Graph.Models;
//using Microsoft.Extensions.Options;
//using Microsoft.Graph.Models.Security;

//namespace Task11_crud_.Models
//{
//    public class Email : IEmailSender
//    {
//        private readonly ILogger _logger;
//        private readonly IEmailSender _emailSender;

//        public Email(IOptions<AuthMessageSenderOptions> optionsAccessor,
//                           ILogger<EmailSender> logger,IEmailSender emailSender)
//        {
//            Options = optionsAccessor.Value;
//            _logger = logger;
//            _emailSender = emailSender;
//        }

//        public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

//        public async Task SendEmailAsync(string toEmail, string subject, string message)
//        {
//            if (string.IsNullOrEmpty(Options.SendGridKey))
//            {
//                throw new Exception("Null SendGridKey");
//            }
//            await Execute(Options.SendGridKey, subject, message, toEmail);
//        }

//        public async Task Execute(string apiKey, string subject, string message, string toEmail)
//        {
//            var client = new SendGridClient(apiKey);
//            var msg = new SendGridMessage()
//            {
//                From = new EmailAddress("divyaevoke@outlook.com", "Email Confirmation"),
//                Subject = subject,
//                PlainTextContent = message,
//                HtmlContent = message
//            };
//            msg.AddTo(new EmailAddress(toEmail,"cofirm"));
            
//            //// Disable click tracking.
//            //// See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
//            //msg.SetClickTracking(false, false);
//            //var response = await client.SendEmailAsync(msg);
//            //_logger.LogInformation(response.IsSuccessStatusCode
//            //                       ? $"Email to {toEmail} queued successfully!"
//            //                       : $"Failure Email to {toEmail}");
//        }
//        //    public readonly ILogger _logger;
//        //    public Seceretkey seceretkey = new Seceretkey();

//        //    public async Task SendEmailAsync(string email, string subject, string Message)
//        //    {
//        //        if (string.IsNullOrEmpty(seceretkey.SendGridKey))
//        //        {
//        //            throw new NotImplementedException();
//        //        }
//        //        await SendEmail(seceretkey.SendGridKey, subject, Message, email);
//        //    }
//        //    public async Task SendEmail(string sendGridKey,string subject,string Message,string email)
//        //    {
//        //        var sendGridClient = new SendGridClient(sendGridKey);
//        //        var conformationMsg = new SendGridMessage()
//        //        {
//        //            From = new EmailAddress("divyaevoke@outlook.com", "Confiramation Account"),
//        //            //To = new EmailAddress("divyaevoke@outlook.com"),
//        //            Subject = subject,
//        //            PlainTextContent = Message,
//        //            HtmlContent = Message,
//        //        };
//        //        conformationMsg.AddTo(new EmailAddress(email));
//        //        conformationMsg.SetClickTracking(false, false);
//        //        var response = await sendGridClient.SendEmailAsync(conformationMsg);
//        //        if(response.IsSuccessStatusCode)
//        //        {

//        //            _logger.LogInformation($"Email send to the {email} successfully");
//        //        }
//        //        else
//        //        {
//        //            _logger.LogInformation($"{response.ToJson}");
//        //            _logger.LogInformation($"Email send to the {response.StatusCode} not successfull");
//        //        }

//        //    }
//        //}
//    }

//    internal class SendGridMessage
//    {
//        public SendGridMessage()
//        {
//        }
       
//        public EmailAddress From { get; set; }
//        public string Subject { get; set; }
//        public string PlainTextContent { get; set; }
//        public string HtmlContent { get; set; }

//        internal void AddTo(EmailAddress emailAddress)
//        {
//            throw new NotImplementedException();
//        }

//        internal void SetClickTracking(bool v1, bool v2)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
