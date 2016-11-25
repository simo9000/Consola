using System.Collections.Generic;

using Consola.Library.util;
using Nancy;
using Nancy.Cookies;
using Nancy.Responses;



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
                return View["Home"].WithCookie(new NancyCookie("fileDownload", "true"));
            };

            Get["/Download/{Key}"] = parameters =>
            {
                if (this.Request.Headers.UserAgent.Contains("PhantomJS"))
                    return 200;
                string key = (string)parameters.Key;
                Download item = Downloads[key];
                StreamResponse response = new StreamResponse(() => item.Content, item.MimeType);
                return response.AsAttachment(item.FileName);
            };
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}