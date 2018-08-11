using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AwesomeHostedService
{
    public class AwesomeHostedService : IHostedService
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment env;

        public AwesomeHostedService(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            this.env = env;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var client = new HttpClient();
            var file = $@"{env.ContentRootPath}\wwwroot\text.txt";
            while (true)
            {
                var response = await client.GetAsync("https://www.apress.com/us");
                using (var output = File.OpenWrite(file))
                    using (var content = await response.Content.ReadAsStreamAsync())
                    {
                        content.CopyTo(output);
                    }
                Thread.Sleep(60000);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}