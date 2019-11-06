using System;
using Mono.Cecil;

namespace UniEnumExtension
{
    public interface IEnumInfo : IEquatable<IEnumInfo>, IEquatable<TypeReference>
    {
        TypeDefinition TypeDefinition { get; }
        bool IsOverWritten { get; }
        
        bool HasFlag { get; }
        
        FieldDefinition ValueFieldDefinition { get; }
        UnderlyingType UnderlyingType { get; }
        
        bool HasNegativeValue { get; }
        bool IsValueZeroStart { get; }
        bool IsValueContinuous { get; }
        bool IsValueApproximatelyContinuous { get; }
        bool HasDuplicate { get; }
        
        long[] GetLongValues();
        ulong[] GetULongValues();
        int[] GetIntValues();
        uint[] GetUIntValues();
        
        string[] Names { get; }
        string LongestName { get; }
        string ShortestName { get; }
        bool IsNameLengthContinuous { get; }

        FieldDefinition[] FieldDefinitions { get; }

        object MinValue { get; }
        object MaxValue { get; }
    }
}