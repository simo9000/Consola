using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consola.Library
{
    interface SessionStartup
    {
        void Startup(ScriptSession session);
    }
}
