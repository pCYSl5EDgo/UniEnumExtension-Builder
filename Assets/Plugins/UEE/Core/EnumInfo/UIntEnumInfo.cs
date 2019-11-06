using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace UniEnumExtension
{
    public sealed class UIntEnumInfo : EnumInfo
    {
        private readonly uint[] values;
        private readonly NameValuePair<uint>[] pairs;

        public UIntEnumInfo(TypeDefinition typeDefinition) : base(typeDefinition)
        {
            values = new uint[this.FieldDefinitions.LongLength];
            pairs = new NameValuePair<uint>[values.LongLength];
            for (var i = 0; i < this.values.Length; i++)
            {
                values[i] = (uint) this.FieldDefinitions[i].Constant;
                pairs[i] = new NameValuePair<uint>(Names[i], values[i], i);
            }
            if (pairs.Length == 0)
            {
                return;
            }
            Array.Sort(pairs, NameValuePairSorterByValue.Default<uint>());
            IsValueZeroStart = pairs[0].Value == 0;
            for (var i = 0; i < pairs.Length - 1; i++)
            {
                var diff = pairs[i + 1].Value - pairs[i].Value;
                if (diff == 1) continue;
                IsValueContinuous = false;
                if (diff != 0) continue;
                HasDuplicate = true;
                break;
            }
        }

        private IEnumerable<NameValuePair<uint>> TryAssign(uint b, IEnumerable<NameValuePair<uint>> candidate)
        {
            if (b == default)
            {
                return IsValueZeroStart ? new[] {pairs[0]} : Array.Empty<NameValuePair<uint>>();
            }
            for (var i = pairs.Length - 1; i >= 0; i--)
            {
                var value = pairs[i].Value;
                if (value == default) break;
                if (b == value)
                    return candidate.Append(pairs[i]);
                if ((b & value) == value)
                {
                    return TryAssign(b ^ value, candidate.Append(pairs[i]));
                }
            }
            return Array.Empty<NameValuePair<uint>>();
        }

        public override bool HasNegativeValue => false;
        public override uint[] GetUIntValues() => values;
    }
}