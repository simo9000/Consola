using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(Consola.AspNet.Startup))]

namespace Consola.AspNet
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Consola.start.startApp(app);
        }
    }
}