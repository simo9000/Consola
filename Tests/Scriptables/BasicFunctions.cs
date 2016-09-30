using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webConsole.Library;

namespace Tests.Scriptables
{
    class BasicFunctions : IScriptable
    {
        public ScriptSession session{ get; set; }

        public void printMessage()
        {
            session.WriteLine("Hello World");
        }
    }
}
