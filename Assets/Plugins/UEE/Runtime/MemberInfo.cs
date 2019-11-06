using System;
using System.Reflection;

namespace UniEnumExtension.Runtime
{
    public readonly struct MemberInfo<T>
        where T : unmanaged, Enum
    {
        public readonly T Value;
        public readonly string Name;
        public readonly FieldInfo FieldInfo;

        public MemberInfo(FieldInfo fieldInfo, string name, T value)
        {
            FieldInfo = fieldInfo;
            Name = name;
            Value = value;
        }
    }
}