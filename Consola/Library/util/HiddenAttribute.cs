using System;

namespace Consola.Library.util
{
    [AttributeUsage(AttributeTargets.Field |
        AttributeTargets.Method | AttributeTargets.Property,
        AllowMultiple = true, Inherited = true)]
    public class Hidden : Attribute { }

}