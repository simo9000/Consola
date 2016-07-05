using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;


namespace scriptConsole.Library
{
    public class ScriptSession
    {
        private ScriptEngine engine;
        public ScriptScope scope { get; }
        private MemoryStreamWithEvents buffer;

        public ScriptSession(Action<string> outputMethod)
        {
            engine = Python.CreateEngine();
            buffer = new MemoryStreamWithEvents();
            buffer.writeEvent = outputMethod;
            engine.Runtime.IO.SetOutput(buffer, Encoding.Unicode);
            scope = engine.CreateScope();
        }

        public void executeCommand(string command)
        {
            ScriptSource source = engine.CreateScriptSourceFromString(command);
            source.Execute(scope);
        }
    }
}