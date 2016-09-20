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
        ScriptSession session { get; set; }
    }

    internal static class Exstensions
    {
        internal static void show(this IScriptable obj)
        {
            Type type = obj.GetType();
            StringBuilder builder = new StringBuilder(type.Name);
            builder.Append(":");
            builder.Append(Environment.NewLine);
            builder.Append(new String('-', type.Name.Count()));
            builder.Append(Environment.NewLine);
            PropertyInfo[] properties = obj.GetType().GetProperties();
            if (properties.Count() > 0)
            {
                builder.Append("Properties:");
                foreach (PropertyInfo property in properties)
                {
                    builder.Append('\t');
                    builder.Append(property.Name);
                    builder.Append(String.Format(" ({0}) ", property.PropertyType.ToString()));
                    Attribute descriptionAttribute = property.GetCustomAttribute(typeof(Description));
                    builder.Append(descriptionAttribute == null ? String.Empty : descriptionAttribute.ToString());
                    builder.Append(Environment.NewLine);
                }
            }
            obj.session.WriteLine(builder.ToString());
        }
    }

    [AttributeUsage (AttributeTargets.Property |
        AttributeTargets.Method | AttributeTargets.Field,
         AllowMultiple = false, Inherited = true)]
    public class Description : Attribute
    {
        private string desc;
        public Description(string desc)
        {
            this.desc = desc; 
        }

        public override string ToString()
        {
            return desc;
        }
    }
    
}
