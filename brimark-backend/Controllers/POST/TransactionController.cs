using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Text;
namespace brimark_backend.Controllers.POST
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {

        private readonly ILogger<TransactionController> _logger;
        private static readonly MySqlCommand makePurchase = new MySqlCommand();

        public TransactionController(ILogger<TransactionController> logger)
        {
            _logger = logger;
        }

        // Assume paypal should give us like tokens to validate

        // Pass the request to Brimark API, initiate transaction with paypal
        // Confirm back to front end, front end redirects
        // Paypal confirms payment / callsback
        // Brimark API then confirms the transaction and makes the database request

        [HttpPost]
        public StatusCodeResult Post(String item, String buyer)
        {

            return StatusCode(501);

        }

        public static void SetConnection(MySqlConnection connection)
        {
            makePurchase.Connection = connection;
        }
    }
}
