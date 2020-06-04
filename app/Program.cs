using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Win32.SafeHandles;

namespace app
{
    public class Program
    {
        internal const string SystemNative = "System.Native.so";

        [DllImport(SystemNative, EntryPoint = "SystemNative_IsATty", SetLastError = true)]
        internal static extern bool IsATty(SafeFileHandle fd);

        public static void Main(string[] args)
        {
            Console.Write("IsATty : " + IsATty(new SafeFileHandle((IntPtr)1, ownsHandle: false)));

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
