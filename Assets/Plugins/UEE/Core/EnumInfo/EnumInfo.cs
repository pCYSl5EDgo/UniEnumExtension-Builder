using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace UniEnumExtension
{
    public abstract class EnumInfo : IEnumInfo
    {
        protected string[][] array = Array.Empty<string[]>();

        protected EnumInfo(TypeDefinition typeDefinition)
        {
            this.TypeDefinition = typeDefinition;
            HasFlag = typeDefinition.CustomAttributes.Any(attribute => attribute.AttributeType.Name == "FlagsAttribute");
            ValueFieldDefinition = typeDefinition.Fields[0];
            switch (ValueFieldDefinition.FieldType.Name)
            {
                case nameof(UnderlyingType.Byte):
                    UnderlyingType = UnderlyingType.Byte;
                    break;
                case nameof(UnderlyingType.SByte):
                    UnderlyingType = UnderlyingType.SByte;
                    break;
                case nameof(UnderlyingType.Int16):
                    UnderlyingType = UnderlyingType.Int16;
                    break;
                case nameof(UnderlyingType.UInt16):
                    UnderlyingType = UnderlyingType.UInt16;
                    break;
                case nameof(UnderlyingType.Int32):
                    UnderlyingType = UnderlyingType.Int32;
                    break;
                case nameof(UnderlyingType.UInt32):
                    UnderlyingType = UnderlyingType.UInt32;
                    break;
                case nameof(UnderlyingType.Int64):
                    UnderlyingType = UnderlyingType.Int64;
                    break;
                case nameof(UnderlyingType.UInt64):
                    UnderlyingType = UnderlyingType.UInt64;
                    break;
            }
            FieldDefinitions = new FieldDefinition[typeDefinition.Fields.Count - 1];
            Names = new string[FieldDefinitions.LongLength];
            for (var i = 0; i < FieldDefinitions.Length; i++)
            {
                ref var field = ref FieldDefinitions[i];
                field = typeDefinition.Fields[i + 1];
                Names[i] = field.Name;
            }
            if (Names.Length == 0)
            {
                ShortestName = LongestName = default;
                IsNameLengthContinuous = true;
            }
            else
            {
                ShortestName = LongestName = Names[0];
                for (var i = 1; i < Names.Length; i++)
                {
                    var name = Names[i];
                    if (ShortestName.Length > name.Length)
                        ShortestName = name;
                    else if (LongestName.Length < name.Length)
                        LongestName = name;
                }
                IsNameLengthContinuous = LongestName.Length - ShortestName.Length + 1 == Names.Length && Names.Select(x => x.Length).Distinct().Count() == Names.Length;
            }
        }

        public bool Equals(IEnumInfo other) => ReferenceEquals(this, other);

        public bool Equals(TypeReference other) => TypeDefinition.FullName == other?.FullName;

        public TypeDefinition TypeDefinition { get; }
        public bool IsOverWritten { get; internal set; }
        public bool HasFlag { get; }
        public FieldDefinition ValueFieldDefinition { get; }
        public UnderlyingType UnderlyingType { get; }
        public abstract bool HasNegativeValue { get; }
        public bool IsValueZeroStart { get; protected set; } = true;
        public bool IsValueContinuous { get; protected set; } = true;
        public bool IsValueApproximatelyContinuous { get; protected set; } = true;
        public bool HasDuplicate { get; protected set; }
        public virtual long[] GetLongValues() => throw new InvalidOperationException();

        public virtual ulong[] GetULongValues() => throw new InvalidOperationException();

        public virtual int[] GetIntValues() => throw new InvalidOperationException();

        public virtual uint[] GetUIntValues() => throw new InvalidOperationException();


        public string[] Names { get; }
        public string LongestName { get; }
        public string ShortestName { get; }
        public bool IsNameLengthContinuous { get; }
        public FieldDefinition[] FieldDefinitions { get; }
        public object MinValue { get; protected set; }
        public object MaxValue { get; protected set; }
        public string[][] ToStringGroups() => array;
    }
}