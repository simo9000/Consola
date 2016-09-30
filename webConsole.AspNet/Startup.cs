using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(webConsole.AspNet.Startup))]

namespace webConsole.AspNet
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            webConsole.start.startApp(app);
        }
    }
}