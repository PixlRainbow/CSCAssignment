using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CSCAssignment
{
    public class Program
    {
        public static readonly string publishableKey = "pk_test_RXaCrs7b1KiItiJl6iQjooZl00otznokwN";
        public static readonly string secretKey = "sk_test_r6Fg2eIWjS6ZKhBGT4nVdBJ100U3MnVc7K";
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
