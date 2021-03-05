using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
namespace brimark_backend.Controllers.POST
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionInitiationController : ControllerBase
    {

        private readonly ILogger<TransactionInitiationController> _logger;
        private static readonly MySqlCommand makePurchase = new MySqlCommand();
        private static readonly HttpClient client = new HttpClient();
        private static readonly Dictionary<string, string> paypalDetails = Utils.Data.GetPayPal();

        public TransactionInitiationController(ILogger<TransactionInitiationController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public StatusCodeResult Post(String item, String buyer)
        {

            client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com/v1/payments/payment");
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", paypalDetails["secret"]);





            return StatusCode(200);

        }

        public static void SetConnection(MySqlConnection connection)
        {
            makePurchase.Connection = connection;
        }
    }
}
