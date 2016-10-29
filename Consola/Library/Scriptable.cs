using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Consola.Library
{
    /// <summary>
    /// Classed derived from Scriptable will be loaded as members of the proxy object in the scripting environment.
    /// </summary>
    public abstract class Scriptable
    {
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
                    builder.Append(method.ReturnType.ToString());
                    builder.Append(' ');
                    builder.Append(method.Name);
                    ParameterInfo[] parameters = method.GetParameters();
                    builder.Append(parameters.Aggregate("(", (accum, param) =>
                     {
                         return String.Format("{0}{1} {2}{3}", accum, param.ParameterType.ToString(), 
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
