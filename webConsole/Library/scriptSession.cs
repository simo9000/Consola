using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;


namespace scriptConsole.Library
{
    public class ScriptSession
    {
        private const string pythonErrorNameSpace = "IronPython.Runtime";
        private const string scriptErrorNameSpace = "Microsoft.Scripting";
        private ScriptEngine engine;
        private static List<IScriptable> scriptObjects;
        private ScriptScope scope { get; }
        private ForwardingMemoryStream buffer;

        private Action<string> consoleOut;

        internal ScriptSession(Action<string> outputMethod)
        {
            this.consoleOut = outputMethod;
            engine = Python.CreateEngine();
            buffer = new ForwardingMemoryStream();
            buffer.writeEvent = consoleOut;
            dynamic mScope = scope = engine.CreateScope();
            populateScope(mScope);
            engine.Runtime.IO.SetOutput(buffer, Encoding.Default);
        }

        internal void executeCommand(string command)
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

        private void populateScope(ref dynamic ScriptScope)
        {
            foreach (IScriptable obj in scriptObjects)
            {
                obj.initialize(this);
                ScriptScope[obj.GetType().Name] = obj;
            }
        }

        public void WriteLine(string text)
        {
            byte[] output = Encoding.Default.GetBytes(text.ToArray<char>());
            buffer.Write(output, 0, output.Length);
        }
    }


}