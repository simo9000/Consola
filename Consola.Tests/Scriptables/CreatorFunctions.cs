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
        public Progeny LazyProgeny;

        public CreatorFunctions(ScriptSession session)
        {
            LazyProgeny = new Progeny();
            initialize(LazyProgeny);
        }
        
        public Progeny CreateProgeny()
        {
            Progeny p = new Progeny();
            initialize(p);
            return p;
        }
    }
}
