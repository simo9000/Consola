using Nancy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Consola
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}