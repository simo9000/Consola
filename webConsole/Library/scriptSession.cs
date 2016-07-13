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
        private const string pythonErrorNameSpace = "IronPython.Runtime";
        private const string scriptErrorNameSpace = "Microsoft.Scripting";
        private ScriptEngine engine;
        public ScriptScope scope { get; }
        private ForwardingMemoryStream buffer;

        private Action<string> consoleOut;

        public ScriptSession(Action<string> outputMethod)
        {
            this.consoleOut = outputMethod;
            engine = Python.CreateEngine();
            buffer = new ForwardingMemoryStream();
            buffer.writeEvent = consoleOut;
            scope = engine.CreateScope(); 
            engine.Runtime.IO.SetOutput(buffer, Encoding.Default);
        }

        public void executeCommand(string command)
        {
            ScriptSource source = engine.CreateScriptSourceFromString(command);
            try
            {
                source.Execute(scope);
            }
            catch(Exception e){
                Type exceptionType = e.GetType();
                if (exceptionType.Namespace.Contains(scriptErrorNameSpace) 
                    || exceptionType.Namespace.Contains(pythonErrorNameSpace))
                {
                    consoleOut(e.Message);
                    consoleOut("\r\n");
                }
            }
        }
    }
}