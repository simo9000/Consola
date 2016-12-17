using System.Web.Http;
using Microsoft.Owin.Hosting;
using System;
using System.Net;
using System.Linq;

namespace Consola.SelfHost
{
    public class Host
    {
        public static IDisposable app;
        static void Main(string[] args)
        {                
            start(args.Contains("--IISemulation"));
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
            app.Dispose();
        }

        public static void start(bool useIISEmulation = false)
        {
            var url = "http://+:80/";
            try
            {
                if (useIISEmulation)
                    Startup.usIISEmulation = true;
                app = WebApp.Start<Startup>(url);
            }
            catch (HttpListenerException ex)
            {
                throw new UnauthorizedAccessException(String.Format("Administrative permission for ({0}) required. Run as Administrator or See https://msdn.microsoft.com/en-us/library/ms733768.aspx.", url), ex);
            }
            Console.WriteLine("Running on {0}", url);
        }

        public static void stop()
        {
            app.Dispose();
        }
    }
}
