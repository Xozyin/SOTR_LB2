using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SportsNewsAPI.Models;
using Mongo2Go;

namespace SportsNewsAPI.Tests.Factories
{
    public class ApiFactory : WebApplicationFactory<Program>
    {
        public MongoDbRunner _dbRunner { get; }

        public ApiFactory()
        {
            _dbRunner = MongoDbRunner.Start();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.Configure<SportsNewsDatabaseSettings>(options =>
                {
                    options.ConnectionString = _dbRunner.ConnectionString;
                    options.DatabaseName = "TestDatabase";
                    options.NewsCollectionName = "News";
                });
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _dbRunner.Dispose();
        }
    }
}
