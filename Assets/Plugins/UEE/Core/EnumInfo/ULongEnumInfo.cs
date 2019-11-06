using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace UniEnumExtension
{
    public sealed class ULongEnumInfo : EnumInfo
    {
        private readonly ulong[] values;
        private readonly NameValuePair<ulong>[] pairs;

        public ULongEnumInfo(TypeDefinition typeDefinition) : base(typeDefinition)
        {
            values = new ulong[FieldDefinitions.LongLength];
            pairs = new NameValuePair<ulong>[values.LongLength];
            for (var i = 0; i < this.values.Length; i++)
            {
                values[i] = (ulong) this.FieldDefinitions[i].Constant;
                pairs[i] = new NameValuePair<ulong>(Names[i], values[i], i);
            }
            if (pairs.Length == 0)
            {
                return;
            }
            Array.Sort(pairs, NameValuePairSorterByValue.Default<ulong>());
            if (pairs[0].Value == 0)
            {
                IsValueZeroStart = true;
            }
            else
            {
                IsValueZeroStart = false;
            }
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

        private IEnumerable<NameValuePair<ulong>> TryAssign(ulong b, IEnumerable<NameValuePair<ulong>> candidate)
        {
            if (b == default)
            {
                return IsValueZeroStart ? new[] {pairs[0]} : Array.Empty<NameValuePair<ulong>>();
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
            return Array.Empty<NameValuePair<ulong>>();
        }

        public override bool HasNegativeValue => false;
        public override ulong[] GetULongValues() => values;
    }
}