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
}
