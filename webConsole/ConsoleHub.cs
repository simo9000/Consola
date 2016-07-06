using Microsoft.AspNet.SignalR;
using scriptConsole.Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace scriptConsole
{
    public class ConsoleHub : Hub
    {
        private static Dictionary<string, ScriptSession> sessions = new Dictionary<string, ScriptSession>();

        public override Task OnConnected()
        {
            string id = this.Context.ConnectionId;
            ScriptSession session = new ScriptSession(pushOutput);
            sessions.Add(id, session);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            sessions.Remove(this.Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public void submitCommand(string command)
        {
            ScriptSession session = sessions[this.Context.ConnectionId];
            session.executeCommand(command);
        }
                
        public void pushOutput(string line)
        {
            Clients.Caller.pushOutput(line);
        }
    }
}