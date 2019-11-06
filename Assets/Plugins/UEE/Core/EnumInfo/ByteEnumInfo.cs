using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace UniEnumExtension
{
    public sealed class ByteEnumInfo : CompatibleIntEnumInfo
    {
        public ByteEnumInfo(TypeDefinition typeDefinition) : base(typeDefinition)
        {
            for (var i = 0; i < this.Values.Length; i++)
            {
                Values[i] = (byte) this.FieldDefinitions[i].Constant;
                Pairs[i] = new NameValuePair<int>(Names[i], Values[i], i);
            }
            if (Pairs.Length == 0)
            {
                return;
            }
            Array.Sort(Pairs, NameValuePairSorterByValue.Default<int>());
            IsValueZeroStart = Pairs[0].Value == 0;
            for (var i = 0; i < Pairs.Length - 1; i++)
            {
                var diff = Pairs[i + 1].Value - Pairs[i].Value;
                if (diff == 1) continue;
                IsValueContinuous = false;
                if (diff != 0) continue;
                HasDuplicate = true;
                break;
            }
        }

        private IEnumerable<NameValuePair<int>> TryAssign(byte b, IEnumerable<NameValuePair<int>> candidate)
        {
            if (b == default)
            {
                return IsValueZeroStart ? new[] {Pairs[0]} : Array.Empty<NameValuePair<int>>();
            }
            for (var i = Pairs.Length - 1; i >= 0; i--)
            {
                var value = (byte) Pairs[i].Value;
                if (value == default) break;
                if (b == value)
                    return candidate.Append(Pairs[i]);
                if ((b & value) == value)
                {
                    return TryAssign((byte) (b ^ value), candidate.Append(Pairs[i]));
                }
            }
            return Array.Empty<NameValuePair<int>>();
        }

        public override bool HasNegativeValue => false;
    }
}