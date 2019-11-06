using System;
using System.Collections.Generic;

namespace UniEnumExtension.Runtime
{
    public static class EnumInfoCache<T>
        where T : unmanaged, Enum
    {
        public static readonly Type Type;
        public static readonly Type UnderlyingType;
        public static readonly T[] Values;
        public static readonly string[] Names;
        public static readonly MemberInfo<T>[] MemberInfos;
        public static readonly KeyValuePair<string, T>[] KeyValuePairs;

        static EnumInfoCache()
        {
            Type = typeof(T);
            UnderlyingType = Type.GetEnumUnderlyingType();
            Values = (T[]) Type.GetEnumValues();
            Names = Type.GetEnumNames();
            for (var i = 0; i < Names.Length; i++)
            {
                Names[i] = string.Intern(Names[i]);
            }
            MemberInfos = new MemberInfo<T>[Values.LongLength];
            KeyValuePairs = new KeyValuePair<string, T>[Values.LongLength];
            var fields = Type.GetFields();
            for (var i = 0L; i < MemberInfos.LongLength; i++)
            {
                MemberInfos[i] = new MemberInfo<T>(fields[i + 1L], Names[i], Values[i]);
                KeyValuePairs[i] = new KeyValuePair<string, T>(Names[i], Values[i]);
            }
        }
    }
}