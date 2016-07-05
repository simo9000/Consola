using Nancy.ViewEngines.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scriptConsole
{
    public class RazorConfig : IRazorConfiguration
    {
        public bool AutoIncludeModelNamespace
        {
            get
            {
                return true;
            }
        }

        public IEnumerable<string> GetAssemblyNames()
        {
            yield return typeof(Enumerable).Assembly.FullName;
            yield return typeof(System.Web.Script.Serialization.JavaScriptSerializer).Assembly.FullName;
            yield return "System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            yield return "Nancy.ViewEngines.Razor";
            yield return "System.Linq";
            yield return "System.Web.Script.Serialization";
            yield return "System.Web";
        }
    }
}