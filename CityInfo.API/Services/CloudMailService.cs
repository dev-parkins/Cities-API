using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private const string _mailTo = "admin@testcompany.com";
        private const string _mailFrom = "noreply@testcompany.com";

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with CloudMailService");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}
