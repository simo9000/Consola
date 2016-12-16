using Consola.Library;
using Consola.Library.util;
using Nancy;
using Nancy.Responses;

using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Consola
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class IndexModule : NancyModule
    {
        private const string CONNECTION_ID_HEADER = "CONNECTION_ID";
        internal static Dictionary<string, Download> Downloads = new Dictionary<string, Download>();
        public IndexModule() : base("/Console")
        {
            Get["/"] = parameters =>
            {
                return View["Home"];
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

            Post["/Command"] = parameters =>
            {
                ScriptSession session = ConsoleHub.sessions[Request.Headers[CONNECTION_ID_HEADER].FirstOrDefault()];
                if (session != null)
                {
                    string body = new StreamReader(Request.Body).ReadToEnd();
                    session.executeCommand(body);
                    return new Response() { StatusCode = HttpStatusCode.Accepted };
                }
                return new Response() { StatusCode = HttpStatusCode.InternalServerError, ReasonPhrase = "Connection not found" };
            };
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}