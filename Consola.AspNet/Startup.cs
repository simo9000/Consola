using Microsoft.Owin;
using Owin;
using SqueezeMe;

[assembly: OwinStartup(typeof(Consola.AspNet.Startup))]

namespace Consola.AspNet
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCompression();
            Consola.start.startApp(app);
        }
    }
}