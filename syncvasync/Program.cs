using System;
using System.Threading;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace syncvasync
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int processorCounter = Environment.ProcessorCount;
            bool success = ThreadPool.SetMaxThreads(processorCounter, processorCounter);

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
