using System;
using System.Collections.Generic;

namespace UniEnumExtension
{
    public readonly struct NameValuePair<T>
        where T : unmanaged, IComparable<T>, IEquatable<T>
    {
        public readonly string Name;
        public readonly T Value;
        public readonly int Index;

        public (string, T) Tuple => (Name, Value);

        public NameValuePair(string name, T value, int index)
        {
            Name = name;
            Value = value;
            Index = index;
        }

        public override string ToString()
        {
            return Name + " : " + Value.ToString() + " of " + Index.ToString();
        }

        public static unsafe (string, T) operator |(NameValuePair<T> left, NameValuePair<T> right)
        {
            if (left.Value.Equals(default(T)))
            {
                return right.Value.Equals(default(T)) ? left.Tuple : right.Tuple;
            }
            if (typeof(T) == typeof(byte))
            {
                var value = (byte) (*(byte*) &left.Value | *(byte*) &right.Value);
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            if (typeof(T) == typeof(sbyte))
            {
                var value = (sbyte) (*(sbyte*) &left.Value | *(sbyte*) &right.Value);
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            if (typeof(T) == typeof(ushort))
            {
                var value = (ushort) (*(ushort*) &left.Value | *(ushort*) &right.Value);
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            if (typeof(T) == typeof(short))
            {
                var value = (short) (*(short*) &left.Value | *(short*) &right.Value);
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            if (typeof(T) == typeof(uint))
            {
                var value = *(uint*) &left.Value | *(uint*) &right.Value;
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            if (typeof(T) == typeof(int))
            {
                var value = *(int*) &left.Value | *(int*) &right.Value;
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            if (typeof(T) == typeof(long))
            {
                var value = *(long*) &left.Value | *(long*) &right.Value;
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            if (typeof(T) == typeof(ulong))
            {
                var value = *(ulong*) &left.Value | *(ulong*) &right.Value;
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            throw new InvalidCastException();
        }

        public static unsafe (string, T) operator |((string Name, T Value) left, NameValuePair<T> right)
        {
            if (left.Value.Equals(default(T)))
            {
                return right.Value.Equals(default(T)) ? left : right.Tuple;
            }
            if (typeof(T) == typeof(byte))
            {
                var value = *(byte*) &left.Value | *(byte*) &right.Value;
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            if (typeof(T) == typeof(sbyte))
            {
                var value = *(sbyte*) &left.Value | *(sbyte*) &right.Value;
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            if (typeof(T) == typeof(ushort))
            {
                var value = *(ushort*) &left.Value | *(ushort*) &right.Value;
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            if (typeof(T) == typeof(short))
            {
                var value = *(short*) &left.Value | *(short*) &right.Value;
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            if (typeof(T) == typeof(uint))
            {
                var value = *(uint*) &left.Value | *(uint*) &right.Value;
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            if (typeof(T) == typeof(int))
            {
                var value = *(int*) &left.Value | *(int*) &right.Value;
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            if (typeof(T) == typeof(long))
            {
                var value = *(long*) &left.Value | *(long*) &right.Value;
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            if (typeof(T) == typeof(ulong))
            {
                var value = *(ulong*) &left.Value | *(ulong*) &right.Value;
                return (left.Name + ", " + right.Name, *(T*) &value);
            }
            throw new InvalidCastException();
        }
    }

    public static class NameValuePairSorterByName
    {
        public static NameValuePairSorterByName<T> Default<T>()
            where T : unmanaged, IComparable<T>, IEquatable<T>
            => NameValuePairSorterByName<T>.Default;
    }

    public sealed class NameValuePairSorterByName<T>
        : IComparer<NameValuePair<T>>,
            IEqualityComparer<NameValuePair<T>>
        where T : unmanaged, IComparable<T>, IEquatable<T>
    {
        public static readonly NameValuePairSorterByName<T> Default = new NameValuePairSorterByName<T>();

        public int Compare(NameValuePair<T> x, NameValuePair<T> y)
        {
            var c0 = x.Name.Length.CompareTo(y.Name.Length);
            if (c0 != 0) return c0;
            var c1 = string.CompareOrdinal(x.Name, y.Name);
            if (c1 != 0) return c1;
            throw new InvalidOperationException(x.ToString() + ", " + y.ToString());
        }

        public bool Equals(NameValuePair<T> x, NameValuePair<T> y) => x.Name.Equals(y.Name);

        public int GetHashCode(NameValuePair<T> obj) => obj.Name.GetHashCode();
    }

    public static class NameValuePairSorterByValue
    {
        public static NameValuePairSorterByValue<T> Default<T>()
            where T : unmanaged, IComparable<T>, IEquatable<T>
            => NameValuePairSorterByValue<T>.Default;
    }

    public sealed class NameValuePairSorterByValue<T>
        : IComparer<NameValuePair<T>>,
            IEqualityComparer<NameValuePair<T>>
        where T : unmanaged, IComparable<T>, IEquatable<T>
    {
        public static readonly NameValuePairSorterByValue<T> Default = new NameValuePairSorterByValue<T>();

        public int Compare(NameValuePair<T> x, NameValuePair<T> y)
        {
            var c0 = x.Value.CompareTo(y.Value);
            return c0 != 0 ? c0 : x.Index.CompareTo(y.Index);
        }

        public bool Equals(NameValuePair<T> x, NameValuePair<T> y) => x.Value.Equals(y.Value);

        public int GetHashCode(NameValuePair<T> obj) => obj.Value.GetHashCode();
    }
}