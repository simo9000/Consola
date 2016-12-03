using Consola.Library;
using Consola.Library.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consola.Tests.Scriptables
{
    public class Parent : Scriptable
    {
        [Description("Parent Function")]
        public void printParent()
        {
            Session.WriteLine("Parent");
        }
    }
}
