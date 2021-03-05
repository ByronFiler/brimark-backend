using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

// TODO: Determine Valid IPs or however we want to do that
// TODO: Validate and connect to database
namespace brimark_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {

        private static readonly MySqlCommand searchQuery = new MySqlCommand();

        private static readonly MySqlParameter queryParameter = new MySqlParameter("@query", MySqlDbType.VarChar, 64);
        private static readonly MySqlParameter brandParameter = new MySqlParameter("@brand", MySqlDbType.VarChar, 64);
        private static readonly MySqlParameter sizingParameter = new MySqlParameter("@sizing", MySqlDbType.VarChar, 32);
        private static readonly MySqlParameter minimumPriceParameter = new MySqlParameter("@minimumPrice", MySqlDbType.Float);
        private static readonly MySqlParameter maximumPriceParameter = new MySqlParameter("@maximumPrice", MySqlDbType.Float);

        private readonly ILogger<SearchController> _logger;

        public SearchController(ILogger<SearchController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Search> Get(String query, String brand, String sizing, float minimumPrice, float maximumPrice)
        {

            StringBuilder sql = new StringBuilder("SELECT * FROM `items` WHERE ");

            if (
                !string.IsNullOrWhiteSpace(query)
                || !string.IsNullOrWhiteSpace(brand)
                || !string.IsNullOrWhiteSpace(sizing)
                || (minimumPrice > 0)
                || (maximumPrice > 0)
                )
            {
                // This is not the pretties so feel free to change this
                searchQuery.Parameters.Clear();
                if (!string.IsNullOrWhiteSpace(query))
                {
                    sql.Append("name LIKE @query");
                    searchQuery.Parameters.Add(queryParameter);
                    if (!string.IsNullOrWhiteSpace(brand) || !string.IsNullOrWhiteSpace(sizing) || minimumPrice > 0 || maximumPrice > 0)
                        sql.Append(" AND ");
                }
                if (!string.IsNullOrWhiteSpace(brand))
                {
                    sql.Append("brand LIKE @brand");
                    searchQuery.Parameters.Add(brandParameter);
                    if (!string.IsNullOrWhiteSpace(sizing) || minimumPrice > 0 || maximumPrice > 0)
                        sql.Append(" AND ");
                }
                if (!string.IsNullOrWhiteSpace(sizing))
                {
                    searchQuery.Parameters.Add(sizingParameter);
                    sql.Append("sizing = @sizing");
                    if (minimumPrice > 0 || maximumPrice > 0)
                        sql.Append(" AND ");
                }
                if (minimumPrice > 0)
                {
                    searchQuery.Parameters.Add(minimumPriceParameter);
                    sql.Append("price >= @minimumPrice");
                    if (maximumPrice > 0)
                        sql.Append(" AND ");
                }
                if (maximumPrice > 0)
                {
                    searchQuery.Parameters.Add(maximumPriceParameter);
                    sql.Append("price <= @maximumPrice");
                }

                sql.Append(";");

                searchQuery.CommandText = sql.ToString();
                searchQuery.Prepare();
                using (MySqlDataReader reader = searchQuery.ExecuteReader())
                {

                    if (reader.HasRows)
                    {
                        // OK: 200 (Search Results Found)
                        return null;

                    } else
                    {
                        // No Content: 204 (No Results Found)
                        this.HttpContext.Response.StatusCode = 400;
                        return null;
                    }

                }



            } else
            {
                // Bad Request: 400 (Invalid Query)
                this.HttpContext.Response.StatusCode = 400;
                return null;
            }

            /*
            if (foundSearchResults)
            {
                // OK: 200
                this.HttpContext.Response.StatusCode = 200;
                return Enumerable.Range(1, 10).Select(x =>
                {
                    Dictionary<String, String> item = DataGenerator.MakeItem();
                    return new Search()
                    {
                        ID = DataGenerator.MakeId(),
                        Image = DataGenerator.MakeId(),
                        Title = item["name"],
                        Price = DataGenerator.GetRng().Next(100, 10000) / 100
                    };
                });
            } else
            {
                // No Content: 204 (No search results found)
                this.HttpContext.Response.StatusCode = 204;
                return null;
            }
            */
        }

        public static void SetConnection(MySqlConnection connection)
        {
            searchQuery.Connection = connection;
        }
    }
}

