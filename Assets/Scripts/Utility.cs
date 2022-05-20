using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Utility
{
    static public bool IsLookingAtObject(Vector3 dir, Vector3 obj1, Vector3 obj2) => (Vector3.Dot(dir, (obj2-obj1).normalized) < 0) ? false : true;
    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }
}
