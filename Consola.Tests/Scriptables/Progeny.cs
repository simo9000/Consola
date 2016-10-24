using Consola.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Scriptables
{
    public class Progeny : Scriptable
    {
        [Description("Simple Name Field")]
        public string Name { get; set; }
    }
}
