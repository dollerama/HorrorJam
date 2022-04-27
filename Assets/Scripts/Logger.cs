using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Logger
{
    static public bool Active = false;
    static public void Log(object message, string subject = "Debugger Main", bool over = false)
    {
        if (!Active && ! over) return;

        Debug.Log($"{subject}:{ message}");
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
