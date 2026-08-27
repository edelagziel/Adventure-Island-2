using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ErrorLoggerExtension
{
    public static void LogErrors(this MonoBehaviour behaviour)
    {
        Application.logMessageReceived += LogError;
    }

    private static void LogError(string condition, string stackTrace, LogType type)
    {
        if(type == LogType.Error || type == LogType.Exception)
        {
            Debug.LogError(stackTrace);
        }
    }
}
