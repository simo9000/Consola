using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webConsole.Library;

namespace Tests
{
    public class BasicFunctions : Scriptable
    {
        [Description("Basic name feild")]
        public string name { get; set; }

        public void printMessage()
        {
            Session.WriteLine("Hello World");
        }
    }
}
