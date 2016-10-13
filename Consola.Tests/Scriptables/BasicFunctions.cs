using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consola.Library;

namespace Tests
{
    public class BasicFunctions : Scriptable
    {
        [Description("Basic name feild")]
        public string name { get; set; }

        public BasicFunctions(ScriptSession session)
        {
            session.startupMessage = String.Empty;
        }

        public void printMessage()
        {
            Session.WriteLine("Hello World");
        }
    }
}
