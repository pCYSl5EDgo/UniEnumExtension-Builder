using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace UniEnumExtension
{
    public sealed class LongEnumInfo : EnumInfo
    {
        private readonly long[] values;
        private readonly NameValuePair<long>[] pairs;

        public LongEnumInfo(TypeDefinition typeDefinition) : base(typeDefinition)
        {
            values = new long[FieldDefinitions.LongLength];
            pairs = new NameValuePair<long>[values.LongLength];
            for (var i = 0; i < this.values.Length; i++)
            {
                values[i] = (long) this.FieldDefinitions[i].Constant;
                pairs[i] = new NameValuePair<long>(Names[i], values[i], i);
            }
            if (pairs.Length == 0)
            {
                return;
            }
            Array.Sort(pairs, NameValuePairSorterByValue.Default<long>());
            if (pairs[0].Value == 0)
            {
                IsValueZeroStart = true;
            }
            else if (pairs[0].Value < 0)
            {
                HasNegativeValue = true;
                IsValueZeroStart = false;
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

        private IEnumerable<NameValuePair<long>> TryAssign(long b, IEnumerable<NameValuePair<long>> candidate)
        {
            if (b == default)
            {
                return IsValueZeroStart ? new[] {pairs[0]} : Array.Empty<NameValuePair<long>>();
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
            return Array.Empty<NameValuePair<long>>();
        }

        public override bool HasNegativeValue { get; }
        public override long[] GetLongValues() => values;
    }
}