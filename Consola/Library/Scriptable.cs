using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        private List<Action> lazyInitialize = new List<Action>();
        private Func<MethodInfo, Type, bool> indcludeMethod = (MI, type) => MI.IsPublic && !MI.IsSpecialName && (MI.DeclaringType == type || MI.DeclaringType == typeof(Scriptable));
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

        internal void setSession(ScriptSession session) {
            this.session = session;
            lazyInitialize.ForEach(a => a());
        }

        /// <summary>
        /// Method used to initialize Scriptable derived classes generated from derived methods
        /// </summary>
        /// <param name="child">Progeny Scriptable instance</param>
        protected Scriptable initialize(Scriptable child)
        {
            if (session == null)
                lazyInitialize.Add(() => child.setSession(session));
            child.setSession(session);
            return child;
        }

        /// <summary>
        /// Displays the derived class members to the console user.
        /// </summary>
        [Description("Inherited: Displays info about class members")]
        public void show()
        {
            if (session == null)
                throw uninitializedException;
            Type type = GetType();
            Outputline builder = new Outputline();
            builder.AppendColor(type.Name, TYPECOLOR);
            builder.Append(":").Append(Environment.NewLine);
            builder.Append(new String('-', type.Name.Count())).Append(Environment.NewLine);
            IEnumerable<FieldInfo> fields = type.GetFields();
            IEnumerable<FieldInfo> hiddenFields = getHiddenMembers(type, (_type) => _type.GetFields());
            fields = fields.Except(hiddenFields, new MemberComparer<FieldInfo>());
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
            IEnumerable<PropertyInfo> properties = type.GetProperties();
            IEnumerable<PropertyInfo> hiddenProperties = getHiddenMembers(type, (_type) => _type.GetProperties());
            properties = properties.Except(hiddenProperties, new MemberComparer<PropertyInfo>());
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
            IEnumerable<MethodInfo> methods = type.GetMethods().Where((MI) => indcludeMethod(MI, type));
            IEnumerable<MethodInfo> hiddenMethods = getHiddenMembers(type, (_type) => _type.GetMethods().Where((MI) => indcludeMethod(MI, _type)));
            methods = methods.Except(hiddenMethods, new MemberComparer<MethodInfo>());
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

        private IEnumerable<T> getHiddenMembers<T>(Type type, Func<Type,IEnumerable<T>> getMembers) where T : MemberInfo
        {
            IEnumerable<T> properties = getMembers(type).ToList().Where(PI => PI.GetCustomAttribute(typeof(Hidden)) != null);
            Type baseType = type.BaseType;
            if (baseType == null)
                return new List<T>();
            IEnumerable<T> parentProperties = getHiddenMembers(baseType,getMembers);
            return properties.Union(parentProperties,new MemberComparer<T>());

        }
    }

    internal class MemberComparer<T> : IEqualityComparer<T> where T : MemberInfo
    {
        public bool Equals(T x, T y)
        {
            return x.Name + x.GetType().ToString() == y.Name + y.GetType().ToString();
        }

        public int GetHashCode(T obj)
        {
            return (obj.Name + obj.GetType().ToString()).GetHashCode();
        }
    }
}
