using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.IO;

namespace MailWebApp.Controllers
{
    [Route("/api/[controller]")]
    public class PostgresController : Controller
    {
        [HttpGet]
        public PostgresConnect Get()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                return new PostgresConnect()
                {
                    pg_dbName = configuration["Database"],
                    pg_host = configuration["Host"],
                    pg_pass = configuration["Password"],
                    pg_port = configuration["Port"],
                    pg_user = configuration["Username"]
                };
            }
        }

        [HttpPost]
        public IActionResult Post(PostgresConnect postgresConnect)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                postgresConnect.UpdateAppSetting("Host", postgresConnect.pg_host);
                postgresConnect.UpdateAppSetting("Password", postgresConnect.pg_pass);
                postgresConnect.UpdateAppSetting("Database", postgresConnect.pg_dbName);
                postgresConnect.UpdateAppSetting("Port", postgresConnect.pg_port);
                postgresConnect.UpdateAppSetting("Username", postgresConnect.pg_user);

                return Ok(postgresConnect);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("InitPostgres")]
        public IActionResult PostBody([FromBody] PostgresConnect connection) => Post(connection);
    }
}
