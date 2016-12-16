using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consola.SelfHost
{
    class IISemulationOwin : OwinMiddleware
    {
        public IISemulationOwin(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            string requestFilterError = context.getRequestFilterError();
            if (requestFilterError != null)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(requestFilterError);
            }
            else
            {
                await Next.Invoke(context);
            }
        }
    }

    class IISemulationSignalR : HubPipelineModule
    {
        public override Func<IHubIncomingInvokerContext, Task<object>> BuildIncoming(Func<IHubIncomingInvokerContext, Task<object>> invoke)
        {
            return async context =>
            {
                string requestFilterError = context.getRequestFilterError();
                if (requestFilterError != null)
                    return new HubResponse() { Error = requestFilterError };
                return await invoke(context);
            };
        }
    }

    public static class IISemulationGeneric
    {
        private const int DEFAULT_MAXIMUM_URL_LENTGH = 4096;
        private const int DEFAULT_MAXIMUM_QUERY_STRING = 2048;

        public static string getRequestFilterError(this IOwinContext context)
        {
            return getRequestFilterError(context.Request.Uri.OriginalString,
                                         context.Request.QueryString.Value);
        }

        public static string getRequestFilterError(this IHubIncomingInvokerContext context)
        {
            string url = context.Hub.Context.Request.Url.OriginalString;
            string queryString = url.Substring(url.IndexOf('?')) ?? String.Empty;
            return getRequestFilterError(url, queryString);
        }
        
        private static string getRequestFilterError(string url, string queryString)
        {
            string returnVal = null;
            if (url.Length > DEFAULT_MAXIMUM_URL_LENTGH)
                return "URL length exceeded IIS default";
            else if (queryString.Length > DEFAULT_MAXIMUM_QUERY_STRING)
                return "Query string length exceeded IIS default";
            return returnVal;
        }
    }
}
