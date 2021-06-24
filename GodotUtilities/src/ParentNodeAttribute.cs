using System;

namespace GodotUtilities
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ParentAttribute : Attribute
    {
        public ParentAttribute() { }
    }
}
