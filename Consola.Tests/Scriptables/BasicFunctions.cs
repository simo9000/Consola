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
        [Description("Basic Name field")]
        public string Name { get; set; }

        public BasicFunctions(ScriptSession session)
        {
            session.startupMessage = String.Empty;
        }

        [Description("Prints Hello World")]
        public void printMessage()
        {
            Session.WriteLine("Hello World");
        }
    }
}
