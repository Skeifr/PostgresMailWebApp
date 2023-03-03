using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MailWebApp
{
    internal class ApplicationContext : DbContext
    {
        internal DbSet<Contact> ContactsDBSet { get; set; }
        internal DbSet<Message> MessagesDBSet { get; set; }
        internal ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            // connect to postgres with connection string from app settings
            optionsBuilder.UseNpgsql($"Host={configuration["Host"]};Port={configuration["Port"]};Database={configuration["Database"]};" +
                $"Username={configuration["Username"]};Password={configuration["Password"]}");
        }
    }
}