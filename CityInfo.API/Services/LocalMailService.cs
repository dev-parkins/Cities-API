using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private const string _mailTo = "admin@testcompany.com"; //TODO: add to config file
        private const string _mailFrom = "noreply@testcompany.com"; //TODO: add to config file

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with LocalMailService");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}
