using Consola.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Scriptables
{
    public class CreatorFunctions : Scriptable
    {
        public CreatorFunctions(ScriptSession session)
        {

        }
        
        public Progeny CreateProgeny()
        {
            Progeny p = new Progeny();
            initialize(p);
            return p;
        }
    }
}
