using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace scriptConsole.Library
{
    interface IScriptable
    {
        void initialize(ScriptSession session);
    }

    internal static class Exstensions
    {
        static string show(this IScriptable obj)
        {
            StringBuilder builder = new StringBuilder();
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach(PropertyInfo property in properties)
            {

            }



            return builder.ToString();
        }
    }

}
