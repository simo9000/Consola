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
            IEnumerable<FieldInfo> fields = getAllMembers(type, (_type) => _type.GetFields().Where( fi => fi.IsPublic));
            if (fields.Count() > 0)
            {
                builder.Append("Fields:").Append(Environment.NewLine);
                foreach(FieldInfo field in fields)
                {
                    builder.Append(TAB);
                    addDataType(ref builder, field.FieldType);
                    builder.Append(' ').Append(field.Name);
                    Attribute descriptionAttribute = field.GetCustomAttribute(typeof(Description));
                    if (descriptionAttribute != null)
                        builder.Append(' ').AppendColor(descriptionAttribute.ToString(), COMMENTCOLOR);
                    builder.Append(Environment.NewLine);
                }
            }
            IEnumerable<PropertyInfo> properties = getAllMembers(type, (_type) => _type.GetProperties());
            if (properties.Count() > 0)
            {
                builder.Append("Properties:").Append(Environment.NewLine);
                foreach (PropertyInfo property in properties)
                {
                    builder.Append(TAB);
                    addDataType(ref builder, property.PropertyType);
                    builder.Append(' ').Append(property.Name);
                    Attribute descriptionAttribute = property.GetCustomAttribute(typeof(Description));
                    if (descriptionAttribute != null)
                        builder.Append(' ').AppendColor(descriptionAttribute.ToString(), COMMENTCOLOR);
                    builder.Append(Environment.NewLine);
                }
            }
            IEnumerable<MethodInfo> methods = getAllMembers(type, (_type) => _type.GetMethods().Where((MI) => indcludeMethod(MI, _type)));
            if (methods.Count() > 0)
            {
                builder.Append("Methods:").Append(Environment.NewLine);
                foreach(MethodInfo method in methods)
                {
                    builder.Append(TAB);
                    addDataType(ref builder, method.ReturnType);
                    builder.Append(' ').Append(method.Name);
                    ParameterInfo[] parameters = method.GetParameters();
                    builder.Append(parameters.Aggregate(new Outputline("("), (accum, param) =>
                     {
                         addDataType(ref accum, param.ParameterType);
                         accum.Append(' ').Append(param.Name);
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

        private bool isNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private IEnumerable<T> getAllMembers<T>(Type type, Func<Type,IEnumerable<T>> getMembers) where T : MemberInfo
        {
            IEnumerable<T> properties = getMembers(type).Where(PI => PI.GetCustomAttribute(typeof(Hidden)) == null);
            Type baseType = type.BaseType;
            if (baseType == null)
                return new List<T>();
            IEnumerable<T> parentProperties = getAllMembers(baseType,getMembers);
            return properties.Union(parentProperties,new MemberComparer<T>());
        }

        private void addDataType(ref Outputline line, Type type)
        {
            bool _isPrimative = isPrimative(type);
            bool _isNullable = isNullable(type);
            string name = _isNullable ? Nullable.GetUnderlyingType(type).Name : type.Name;
            line.AppendColor(name, _isPrimative ? PRIMATIVECOLOR : TYPECOLOR);
            if (_isNullable)
                line.Append("?");
        }

        private class MemberComparer<T> : IEqualityComparer<T> where T : MemberInfo
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

    
}
