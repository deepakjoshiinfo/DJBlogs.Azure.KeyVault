using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DJBlogs.Azure.KeyVault.Data;
using DJBlogs.Azure.KeyVault.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DJBlogs.Azure.KeyVault.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SchoolContext _context;
        public HomeController(ILogger<HomeController> logger, SchoolContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["mysecrets"] = GetSecret();
            return View();
        }
        private string GetSecret()
        {
            var keyvault = "djsecrets";
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
        {
            Delay= TimeSpan.FromSeconds(2),
            MaxDelay = TimeSpan.FromSeconds(16),
            MaxRetries = 5,
            Mode = RetryMode.Exponential
         }
            };
            var client = new SecretClient(new Uri(string.Format("https://{0}.vault.azure.net/", keyvault)), new DefaultAzureCredential(), options);

            KeyVaultSecret secret = client.GetSecret("mysecrets");

            string secretValue = secret.Value;
            return secretValue;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
