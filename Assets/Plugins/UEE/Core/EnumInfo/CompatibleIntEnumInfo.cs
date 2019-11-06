using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace UniEnumExtension
{
    public abstract class CompatibleIntEnumInfo : EnumInfo
    {
        protected readonly int[] Values;
        protected readonly NameValuePair<int>[] Pairs;
        private (string toString, int value)[][] toStringArrayNoFlag;

        public (string toString, int value)[][] ToStringArrayNoFlag
        {
            get
            {
                if (!(toStringArrayNoFlag is null)) return toStringArrayNoFlag;
                switch (Pairs.Length)
                {
                    case 0:
                        return toStringArrayNoFlag = Array.Empty<(string, int)[]>();
                    case 1:
                        return toStringArrayNoFlag = new[]
                        {
                            new[] {(Pairs[0].Name, Pairs[0].Value)}
                        };
                    default:
                    {
                        Array.Sort(Pairs, NameValuePairSorterByValue.Default<int>());
                        var list = new List<List<(string, int)>> {new List<(string, int)> {(Pairs[0].Name, Pairs[0].Value)}};
                        for (var i = 1; i < Pairs.Length; i++)
                        {
                            ref var prev = ref Pairs[i - 1];
                            ref var current = ref Pairs[i];
                            switch (current.Value - prev.Value)
                            {
                                case 0: continue;
                                case 1:
                                    list[list.Count - 1].Add((current.Name, current.Value));
                                    break;
                                case 2:
                                    list[list.Count - 1].Add(((prev.Value + 1).ToString(), prev.Value + 1));
                                    list[list.Count - 1].Add((current.Name, current.Value));
                                    break;
                                case 3:
                                    list[list.Count - 1].Add(((prev.Value + 1).ToString(), prev.Value + 1));
                                    list[list.Count - 1].Add(((prev.Value + 2).ToString(), prev.Value + 2));
                                    list[list.Count - 1].Add((current.Name, current.Value));
                                    break;
                                default:
                                    list.Add(new List<(string, int)>
                                    {
                                        (current.Name, current.Value)
                                    });
                                    break;
                            }
                        }
                        toStringArrayNoFlag = new (string toString, int value)[list.Count][];
                        for (var i = 0; i < toStringArrayNoFlag.Length; i++)
                        {
                            toStringArrayNoFlag[i] = list[i].ToArray();
                        }
                        return toStringArrayNoFlag;
                    }
                }
            }
        }

        protected CompatibleIntEnumInfo(TypeDefinition typeDefinition) : base(typeDefinition)
        {
            Values = new int[this.FieldDefinitions.LongLength];
            Pairs = new NameValuePair<int>[FieldDefinitions.LongLength];
        }

        public override int[] GetIntValues() => Values;
    }
}