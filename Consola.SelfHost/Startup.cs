using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(Consola.SelfHost.Startup))]

namespace Consola.SelfHost
{
    public class Startup
    {
        public static bool usIISEmulation = false;

        public void Configuration(IAppBuilder app)
        {
            if (usIISEmulation) 
                GlobalHost.HubPipeline.AddModule(new IISemulationSignalR());
            start.startApp(app);
            if (usIISEmulation)
                app.Use(typeof(IISemulationOwin));
            app.UseNancy();
        }
    }

    
}
