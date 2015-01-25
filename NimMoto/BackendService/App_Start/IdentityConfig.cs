using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

using System.Configuration;
using System.Net;

using SendGrid;

using BackendService.DAL;
using BackendService.Models;

namespace BackendService
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return configSendGridAsync(message);
        }

        private Task configSendGridAsync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new System.Net.Mail.MailAddress("admin@nimmoto.com", "NimMoto");
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential
            (
                ConfigurationManager.AppSettings["SENDGRID_USERNAME"],
                ConfigurationManager.AppSettings["SENDGRID_PASSWORD"]
            );

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            if (transportWeb != null)
            {
                return transportWeb.DeliverAsync(myMessage);
            }
            else
            {
                return Task.FromResult(0);
            }
        }
    }

    public class RiderManager : UserManager<Rider>
    {
        public RiderManager(IUserStore<Rider> store) : base(store)
        {
        }

        public override Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            Rider rider = this.FindById(userId);
            if (rider == null)
                return Task.FromResult(String.Empty);

            Random rand = new Random();
            rider.CustomEmailConfirmationToken = rand.Next(1000000).ToString("D6");

            IdentityResult updateResult = this.Update(rider);
            if(updateResult.Succeeded == false)
            {
                return Task.FromResult(String.Join(",", updateResult.Errors));
            }

            return Task.FromResult(rider.CustomEmailConfirmationToken);
        }

        public override Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            Rider rider = this.FindById(userId);
            if (rider == null)
            { 
                return Task.FromResult
                        (
                            new IdentityResult
                            (
                                new [] {String.Format("Could not find user '{0}'.", userId)}
                            )
                        );
            }

            if(String.IsNullOrEmpty(token))
            {
                return Task.FromResult
                        (
                            new IdentityResult
                            (
                                new[] { "Provided token is not valid." }
                            )
                        );
            }

            //Should they be trying to confirm when the field is empty?
            //If the account is already confirmed, then just give it to them.
            if(String.IsNullOrEmpty(rider.CustomEmailConfirmationToken) && rider.EmailConfirmed)
            {
                return Task.FromResult(IdentityResult.Success);
            }

            if(token.Equals(rider.CustomEmailConfirmationToken))
            {
                rider.EmailConfirmed = true;
                rider.CustomEmailConfirmationToken = null;

                IdentityResult updateResult = this.Update(rider);
                if (updateResult.Succeeded == false)
                    return Task.FromResult(updateResult);

                return Task.FromResult(IdentityResult.Success);
            }

            return Task.FromResult
                        (
                            new IdentityResult
                            (
                                new[] { "Provided token is incorrect." }
                            )
                        );
        }

        public static RiderManager Create(IdentityFactoryOptions<RiderManager> options, IOwinContext context)
        {
            var manager = new RiderManager(new UserStore<Rider>(context.Get<NimMotoContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<Rider>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            //manager.RegisterTwoFactorProvider
            //(
            //    "Phone Code", 
            //    new PhoneNumberTokenProvider<Rider>
            //    {
            //        MessageFormat = "Your security code is {0}"
            //    }
            //);
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<Rider>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<Rider>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}
