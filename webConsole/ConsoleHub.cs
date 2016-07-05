using Microsoft.AspNet.SignalR;
using scriptConsole.Library;
using System;
using System.Collections.Generic;

namespace scriptConsole
{
    public class ConsoleHub : Hub
    {
        private static Dictionary<string, ScriptSession> sessions = new Dictionary<string, ScriptSession>();
        
        public void registerEngine()
        {
            string id = this.Context.ConnectionId;
            ScriptSession session = new ScriptSession(pushOutput);
            sessions.Add(id, session);
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