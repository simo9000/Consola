using Nancy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace scriptConsole
{
    public class IndexModule : NancyModule
    {
        public IndexModule() : base("/Console")
        {
            Get["/"] = parameters =>
            {
                /*var assembly = Assembly.GetExecutingAssembly();
                var resource = "scriptConsole.Views.Home.html";
                using (Stream stream = assembly.GetManifestResourceStream(resource))
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }*/
                return View["Home"];
            };

            Get["/test"] = x => View["test"];
        }
    }
}