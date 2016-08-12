using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scriptConsole
{
    public class IndexModule : NancyModule
    {
        public IndexModule() : base("/Console")
        {
            Get["/"] = parameters =>
            {
                return View["Home"];
            };

            Get["/test"] = x => View["test"];
        }

        internal void test(int[] t)
        {
            throw new System.NotImplementedException();
        }
    }
}