using System.Web.Http;
using Microsoft.Owin.Hosting;
using System;

namespace webConsole.SelfHost
{
    public class Host
    {
        public static IDisposable app;
        static void Main(string[] args)
        {
            start();
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
            app.Dispose();
        }

        public static void start()
        {
            var url = "http://+:80";
            app = WebApp.Start<Startup>(url);
            Console.WriteLine("Running on {0}", url);
        }

        public static void stop()
        {
            app.Dispose();
        }
    }
}
