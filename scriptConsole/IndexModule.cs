using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scriptConsole
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                return View["Home"];
            };
        }
    }
}