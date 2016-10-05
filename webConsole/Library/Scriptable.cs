﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace webConsole.Library
{
    public abstract class Scriptable
    {
        private ScriptSession session;
        protected ScriptSession Session
        {
            get { return session; }
        }

        internal void setSession(ScriptSession session) { this.session = session; }

        [Description("Inherited: Displays info about class memebers")]
        public void show()
        {
            Type type = this.GetType();
            StringBuilder builder = new StringBuilder(type.Name);
            builder.Append(":");
            builder.Append(Environment.NewLine);
            builder.Append(new String('-', type.Name.Count()));
            builder.Append(Environment.NewLine);
            IEnumerable<FieldInfo> fields = this.GetType().GetFields().Where((FI) => FI.IsPublic);
            if (fields.Count() > 0)
            {
                builder.Append("Fields:");
                builder.Append(Environment.NewLine);
                foreach(FieldInfo field in fields)
                {
                    builder.Append('\t');
                    builder.Append(field.Name);
                    builder.Append(String.Format(" ({0}) ", field.FieldType.ToString()));
                    Attribute descriptionAttribute = field.GetCustomAttribute(typeof(Description));
                    builder.Append(descriptionAttribute == null ? String.Empty : descriptionAttribute.ToString());
                    builder.Append(Environment.NewLine);
                }
            }
            IEnumerable<PropertyInfo> properties = this.GetType().GetProperties();
            if (properties.Count() > 0)
            {
                builder.Append("Properties:");
                builder.Append(Environment.NewLine);
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
            IEnumerable<MethodInfo> methods = this.GetType().GetMethods().Where((MI) => MI.IsPublic 
                                                                                    && !MI.IsSpecialName
                                                                                    && (
                                                                                        MI.DeclaringType == this.GetType()
                                                                                        ||
                                                                                        MI.DeclaringType == typeof(Scriptable)
                                                                                        )
                                                                                    );
            if (methods.Count() > 0)
            {
                builder.Append("Methods:");
                builder.Append(Environment.NewLine);
                foreach(MethodInfo method in methods)
                {
                    builder.Append('\t');
                    builder.Append(method.Name);
                    ParameterInfo[] parameters = method.GetParameters();
                    builder.Append(parameters.Aggregate("(", (accum, param) =>
                     {
                         return String.Format("{0}{1} {2}{3}", accum, param.GetType().ToString(), 
                                              param.Name, param != parameters.Last() ? "," : String.Empty);
                     }));
                    builder.Append(") ");
                    Attribute descriptionAttribute = method.GetCustomAttribute(typeof(Description));
                    builder.Append(descriptionAttribute == null ? String.Empty : descriptionAttribute.ToString());
                    builder.Append(Environment.NewLine);
                }
            }
            this.Session.WriteLine(builder.ToString());
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