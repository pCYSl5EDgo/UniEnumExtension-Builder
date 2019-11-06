using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public sealed class Demo1Script : MonoBehaviour
{
    private enum XEnum : byte
    {
        A,
        B,
        C,
    }

    private enum YEnum : ulong
    {
        A,
        B,
        C,
    }

    private enum MyEnum
    {
        A,
        B,
        C,
    }

    private void Start()
    {
        {
            Debug.Log(MyEnum.A.ToString());
            Debug.Log(MyEnum.B.ToString());
            Debug.Log(MyEnum.C.ToString());
            Debug.Log(Enum.GetUnderlyingType(typeof(MyEnum)));
            var values = (MyEnum[]) Enum.GetValues(typeof(MyEnum));
            Debug.Log(values.Length);
            foreach (var e in values)
            {
                Debug.Log(e.ToString());
            }
        }
        {
            Debug.Log(XEnum.A.ToString());
            Debug.Log(XEnum.B.ToString());
            Debug.Log(XEnum.C.ToString());
            Debug.Log(Enum.GetUnderlyingType(typeof(XEnum)));
            var values = (XEnum[]) Enum.GetValues(typeof(XEnum));
            Debug.Log(values.Length);
            foreach (var e in values)
            {
                Debug.Log(e.ToString());
            }
        }
        {
            Debug.Log(YEnum.A.ToString());
            Debug.Log(YEnum.B.ToString());
            Debug.Log(YEnum.C.ToString());
            Debug.Log(((YEnum) unchecked((ulong) -1145141919810L)).ToString());
            Debug.Log(Enum.GetUnderlyingType(typeof(YEnum)));
            var values = (YEnum[]) Enum.GetValues(typeof(YEnum));
            Debug.Log(values.Length);
            foreach (var e in values)
            {
                Debug.Log(e.ToString());
            }
        }
    }

    [BurstCompile]
    struct JSX : IJob
    {
        private NativeArray<int> a;

        public JSX(NativeArray<int> a)
        {
            this.a = a;
        }

        public void Execute()
        {
            foreach (var i2 in a)
            {
                DELTA:
                if (i2 % 4 == 0)
                {
                    foreach (var i in a)
                    {
                        a[i] = i;
                    }
                }
                else
                {
                    foreach (var i3 in a)
                    {
                        foreach (var i4 in a)
                        {
                            if (a[i4] == 3)
                                goto DELTA;
                            a[i4 + i3 * i2] = 114;
                        }
                    }
                }
            }
        }
    }
}