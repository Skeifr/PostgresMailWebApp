using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MailWebApp
{
    public class PostgresConnect
    {
        [Required]
        public string pg_host { get; set; }
        [Required]
        public string pg_user { get; set; }
        [Required]
        public string pg_pass { get; set; }
        [Required]
        public string pg_dbName { get; set; }
        [Required]
        public string pg_port { get; set; }

        public void UpdateAppSetting(string key, string value)
        {
            var configJson = System.IO.File.ReadAllText("appsettings.json");
            var config = JsonSerializer.Deserialize<Dictionary<string, object>>(configJson);
            config[key] = value;
            var updatedConfigJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText("appsettings.json", updatedConfigJson);
        }
    }
}
