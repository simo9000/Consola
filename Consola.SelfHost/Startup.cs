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
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.HubPipeline.AddModule(new IISemulationSignalR());
            start.startApp(app);
            app.Use(typeof(IISemulationOwin));
            app.UseNancy();
        }
    }

    
}
