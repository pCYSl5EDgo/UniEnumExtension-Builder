using System;
using UnityEngine;

public sealed class Demo1Script : MonoBehaviour
{
    private enum MyEnum
    {
        A,
        B,
        C,
    }

    private void Start()
    {
        Debug.Log(MyEnum.A.ToString());
        Debug.Log(MyEnum.B.ToString());
        Debug.Log(MyEnum.C.ToString());
        Debug.Log(Enum.GetUnderlyingType(typeof(MyEnum)));
        var values = (MyEnum[])Enum.GetValues(typeof(MyEnum));
        Debug.Log(values.Length);
        foreach (var e in values)
        {
            Debug.Log(e.ToString());
        }
    }
}
