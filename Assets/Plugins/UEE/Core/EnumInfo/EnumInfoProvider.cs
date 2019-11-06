using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace UniEnumExtension
{
    public sealed class EnumInfoProvider : IEnumInfoProvider
    {
        private readonly Dictionary<string, IEnumInfo> dic = new Dictionary<string, IEnumInfo>();

        public bool TryGetInfo(TypeReference typeReference, out IEnumInfo info)
        {
            if (dic.TryGetValue(typeReference.FullName, out info))
                return true;
            if (!typeReference.TryToDefinition(out var definition))
                return false;
            switch (definition.GetEnumUnderlyingType().Name)
            {
                case "Byte":
                    dic[definition.FullName] = info = new ByteEnumInfo(definition);
                    break;
                case "SByte":
                    dic[definition.FullName] = info = new SByteEnumInfo(definition);
                    break;
                case "Int16":
                    dic[definition.FullName] = info = new ShortEnumInfo(definition);
                    break;
                case "UInt16":
                    dic[definition.FullName] = info = new UShortEnumInfo(definition);
                    break;
                case "Int32":
                    dic[definition.FullName] = info = new IntEnumInfo(definition);
                    break;
                case "UInt32":
                    dic[definition.FullName] = info = new UIntEnumInfo(definition);
                    break;
                case "Int64":
                    dic[definition.FullName] = info = new LongEnumInfo(definition);
                    break;
                case "UInt64":
                    dic[definition.FullName] = info = new ULongEnumInfo(definition);
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
}