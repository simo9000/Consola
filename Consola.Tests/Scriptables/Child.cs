using Consola.Library;
using Consola.Library.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consola.Tests.Scriptables
{
    public class Child : Parent
    {
        public Child(ScriptSession session) { }

        [Description("Child Function")]
        public void printChild()
        {
            Session.WriteLine("Child");}

        // should not display
        public int hiddenInParent { get; set; }

    }
}
