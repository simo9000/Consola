using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Consola.SelfHost.Startup))]

namespace Consola.SelfHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            start.startApp(app);
            app.UseNancy();
        }
    }
}
