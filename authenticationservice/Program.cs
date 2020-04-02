using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace authenticationservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var json = File.ReadAllText(@"appsettings.json");
            var o = JObject.Parse(@json);
            AppSettings.Settings = JsonConvert.DeserializeObject<AppSettings>(o["AppSettings"].ToString());

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
