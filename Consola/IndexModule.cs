using Consola.Library;
using Nancy;
using Nancy.Responses;
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
        internal static Dictionary<string, Download> Downloads = new Dictionary<string, Download>();
        public IndexModule() : base("/Console")
        {
            Get["/"] = parameters =>
            {
                return View["Home"];
            };

            Get["/Download/{Key}"] = parameters =>
            {
                string key = (string)parameters.Key;
                Download item = Downloads[key];
                StreamResponse response = new StreamResponse(() => item.Content, item.MimeType);
                return response.AsAttachment(item.FileName);
            };
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}