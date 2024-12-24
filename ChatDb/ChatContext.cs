﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace ChatDb
{
    internal class ChatContext: DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Message> Messages => Set<Message>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #region logging
            //optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            #endregion
            var config = new ConfigurationBuilder()
                        .AddJsonFile("appsetting.json")
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        }
    }
}
