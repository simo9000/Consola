using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;


namespace Consola.Library
{
    /// <summary>
    /// Container class for interfacing with script environment.
    /// </summary>
    public class ScriptSession
    {
        private const string pythonErrorNameSpace = "IronPython.Runtime";
        private const string scriptErrorNameSpace = "Microsoft.Scripting";
        private const string defaultStartUpMessage = @"Consola Startup";
        private ScriptEngine engine;
        internal static List<Type> scriptObjects = new List<Type>();
        private ScriptScope scope { get; }
        /// <summary>
        /// Message displayed at console start up.
        /// </summary>
        public string startupMessage { get; set; }
        private ForwardingMemoryStream buffer;

        private HubCallbacks client;

        internal ScriptSession(HubCallbacks callbacks)
        {
            this.client = callbacks;    
            engine = Python.CreateEngine();
            buffer = new ForwardingMemoryStream();
            buffer.writeEvent = callbacks.output;
            dynamic mScope = scope = engine.CreateScope();
            populateScope(ref mScope);
            engine.Runtime.IO.SetOutput(buffer, Encoding.Default);
            startupMessage = startupMessage ?? defaultStartUpMessage;
            IEnumerable<Type> startUpClasses = Bootstrapper.getQualifiedTypes(typeof(SessionStartup));
            foreach (Type T in startUpClasses)
                ((SessionStartup)T.GetConstructor(new Type[0]).Invoke(new dynamic[0])).Startup(this);
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
                    client.output(e.Message);
                    client.output("\r\n");
                }
            }
        }

        internal void WriteStartupMessage()
        {
            if (startupMessage != String.Empty)
                WriteLine(startupMessage);
        }

        private void populateScope(ref dynamic ScriptScope)
        {
            IDictionary<string, object> proxy = new ExpandoObject();
            foreach (Type t in scriptObjects)
            {
                ConstructorInfo CI = t.GetConstructor(new Type[1] { typeof(ScriptSession) });
                Scriptable obj = (Scriptable)CI.Invoke(new ScriptSession[1] { this });
                obj.setSession(this);
                proxy.Add(obj.GetType().Name, obj);
            }
            ScriptScope.proxy = proxy;
            ScriptScope.ListObjects = new Action(ListObjects);
        }

        private void ListObjects()
        {
            foreach (Type t in scriptObjects)
                WriteLine(String.Concat("proxy.",t.Name));
        }

        /// <summary>
        /// Output text to console.
        /// </summary>
        /// <param name="text">Text to output</param>
        public void WriteLine(string text)
        {
            byte[] output = Encoding.Default.GetBytes(text.ToArray<char>());
            buffer.Write(output, 0, output.Length);
            output = Encoding.Default.GetBytes(Environment.NewLine.ToArray<char>());
            buffer.Write(output, 0, output.Length);
        }

        /// <summary>
        /// Initiates a download to client
        /// </summary>
        /// <param name="item">Download object to send</param>
        public void pushDownload(Download item)
        {
            string key = item.Key.ToString();
            IndexModule.Downloads.Add(key, item);
            client.download(key);
        }

        internal void removeDownload(string key)
        {
            IndexModule.Downloads[key].Clear();
            IndexModule.Downloads.Remove(key);
        }
    }


}