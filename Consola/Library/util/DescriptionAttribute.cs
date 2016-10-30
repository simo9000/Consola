using System;

namespace Consola.Library.util
{
    /// <summary>
    /// Attribute used to display metadata about members of classes derived from Scriptable with show()
    /// </summary>
    [AttributeUsage(AttributeTargets.Property |
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