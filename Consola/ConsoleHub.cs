using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Consola.Library;
using Microsoft.AspNet.SignalR;
using Owin;

namespace Consola
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ConsoleHub : Hub
    {
        private static Dictionary<string, ScriptSession> sessions = new Dictionary<string, ScriptSession>();

        private ScriptSession Session
        {
            get
            {
                return sessions[this.Context.ConnectionId];
            }
        }

        public override Task OnConnected()
        {
            string id = this.Context.ConnectionId;
            ScriptSession session = new ScriptSession(new HubCallbacks
            {
                output = pushOutput,
                download = initiateDownload,
                outputHtml = pushHtmlOutput,
                returnControl = returnControl
            });
            sessions.Add(id, session);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            sessions.Remove(this.Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public void consoleReady()
        {
            if (Session.WriteStartupMessage())
                returnControl();
        }       

        public void submitCommand(string command)
        {
            Session.executeCommand(command);
        }
                
        public void pushOutput(string line)
        {
            Clients.Caller.pushOutput(line);
        }

        public void pushHtmlOutput(string line)
        {
            Clients.Caller.pushHtmlOutput(line);
        }

        public void returnControl()
        {
            Clients.Caller.newLine();
        }

        public void initiateDownload(string key)
        {
            Clients.Caller.initiateDownload(key);
        }

        public void confirmDownload(string key)
        {
            sessions[this.Context.ConnectionId].removeDownload(key);
        }
    }

    public static class start
    {
        public static void startApp(IAppBuilder app)
        {
            app.MapSignalR(new HubConfiguration { EnableJSONP = true });
        }
    }

    internal struct HubCallbacks
    {
        internal Action<string> output;
        internal Action<string> download;
        internal Action<string> outputHtml;
        internal Action returnControl;
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}