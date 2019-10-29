using System;

[Flags]
public enum TestEnum2 : long
{
    z = 0,
    a = 1,
    b = 2,
    c = 4,
    d = 8,
    e = 0x10,
    f = 0x20,
    g = 0x40,
    h = 0x80,
}
