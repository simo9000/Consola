using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Consola.Library.util;

namespace Consola.Library
{
    /// <summary>
    /// Classed derived from Scriptable will be loaded as members of the proxy object in the scripting environment.
    /// </summary>
    public abstract class Scriptable
    {
        protected string TYPECOLOR = "#00ffcc";
        protected string COMMENTCOLOR = "#336600";
        protected string PRIMATIVECOLOR = "#3333ff";
        protected readonly string TAB = string.Concat(Enumerable.Repeat("&nbsp;", 4));
        private ScriptSession session;
        private readonly TypeLoadException uninitializedException = new TypeLoadException("Scriptable types that do not contain the appropriate constructor must be initialized by Scriptable.initialize before calling show, initializing other Scriptables or otherwise accessing the session.");
        /// <summary>
        /// Reference to the script environment where the derived class is instantiated.
        /// </summary>
        protected ScriptSession Session
        {
            get {
                if (session == null)
                    throw uninitializedException;
                return session;
            }
        }

        internal void setSession(ScriptSession session) { this.session = session; }

        /// <summary>
        /// Method used to initialize Scriptable derived classes generated from derived methods
        /// </summary>
        /// <param name="child">Progeny Scriptable instance</param>
        protected void initialize(Scriptable child)
        {
            if (session == null)
                throw uninitializedException;
            child.setSession(session);
        }

        /// <summary>
        /// Displays the derived class members to the console user.
        /// </summary>
        [Description("Inherited: Displays info about class members")]
        public void show()
        {
            if (session == null)
                throw uninitializedException;
            Type type = this.GetType();
            Outputline builder = new Outputline();
            builder.AppendColor(type.Name, TYPECOLOR);
            builder.Append(":").Append(Environment.NewLine);
            builder.Append(new String('-', type.Name.Count())).Append(Environment.NewLine);
            IEnumerable<FieldInfo> fields = this.GetType().GetFields().Where((FI) => FI.IsPublic);
            if (fields.Count() > 0)
            {
                builder.Append("Fields:").Append(Environment.NewLine);
                foreach(FieldInfo field in fields)
                {
                    builder.Append(TAB)
                           .Append(field.Name)
                           .Append(' ')
                           .AppendColor(field.FieldType.Name, isPrimative(field.FieldType) ? PRIMATIVECOLOR : TYPECOLOR);
                    Attribute descriptionAttribute = field.GetCustomAttribute(typeof(Description));
                    if (descriptionAttribute != null)
                        builder.Append(' ').AppendColor(descriptionAttribute.ToString(), COMMENTCOLOR);
                    builder.Append(Environment.NewLine);
                }
            }
            IEnumerable<PropertyInfo> properties = this.GetType().GetProperties();
            if (properties.Count() > 0)
            {
                builder.Append("Properties:").Append(Environment.NewLine);
                foreach (PropertyInfo property in properties)
                {
                    builder.Append(TAB)
                           .Append(property.Name)
                           .Append(' ')
                           .AppendColor(property.PropertyType.Name, isPrimative(property.PropertyType) ? PRIMATIVECOLOR : TYPECOLOR);
                    Attribute descriptionAttribute = property.GetCustomAttribute(typeof(Description));
                    if (descriptionAttribute != null)
                        builder.Append(' ').AppendColor(descriptionAttribute.ToString(), COMMENTCOLOR);
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
                builder.Append("Methods:").Append(Environment.NewLine);
                foreach(MethodInfo method in methods)
                {
                    builder.Append(TAB)
                           .AppendColor(method.ReturnType.Name, isPrimative(method.ReturnType) ? PRIMATIVECOLOR : TYPECOLOR)
                           .Append(' ')
                           .Append(method.Name);
                    ParameterInfo[] parameters = method.GetParameters();
                    builder.Append(parameters.Aggregate(new Outputline("("), (accum, param) =>
                     {
                         accum.AppendColor(param.ParameterType.Name, isPrimative(param.ParameterType) ? PRIMATIVECOLOR : TYPECOLOR)
                              .Append(' ')
                              .Append(param.Name);
                         if (param != parameters.Last())
                            accum.Append(',');
                         return accum;
                     }));
                    builder.Append(") ");
                    Attribute descriptionAttribute = method.GetCustomAttribute(typeof(Description));
                    if (descriptionAttribute != null)
                        builder.Append(' ').AppendColor(descriptionAttribute.ToString(), COMMENTCOLOR);
                    builder.Append(Environment.NewLine);
                }
            }
            this.Session.WriteLine(builder);
        }

        private bool isPrimative(Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(Decimal) || type == typeof(void);
        }
    }

    /// <summary>
    /// Attribute used to display metadata about members of classes derived from Scriptable with show()
    /// </summary>
    [AttributeUsage (AttributeTargets.Property |
        AttributeTargets.Method | AttributeTargets.Field,
         AllowMultiple = false, Inherited = true)]
    public class Description : Attribute
    {
        private string desc;
        /// <summary>
        /// Creates an attribute for the Sciptable derived class member
        /// </summary>
        /// <param name="desc">Description to display during show()</param>
        public Description(string desc)
        {
            this.desc = desc; 
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override string ToString()
        {
            return desc;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

}
