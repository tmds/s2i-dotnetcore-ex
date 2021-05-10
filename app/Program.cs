using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Real path is " + Realpath("/deployments/config/application.properties"));
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile(Realpath("/deployments/config/application.properties"), optional: false, reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        static string Realpath(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                IntPtr resolved = realpath(path);
                if (resolved != IntPtr.Zero)
                {
                    path = Marshal.PtrToStringAnsi(resolved);
                    Marshal.FreeHGlobal(resolved);
                }
            }
            return path;
        }

        [DllImport("libc")]
        static extern IntPtr realpath(string path, IntPtr resolved_path = default);
    }
}
