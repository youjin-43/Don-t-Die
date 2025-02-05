using UnityEngine;

public enum DebugLevel
{
    NONE,
    LOG
}

public static class DebugController
{
    public static DebugLevel currentDebugLevel = DebugLevel.NONE;

    public static void Log(string message)
    {
        if (currentDebugLevel >= DebugLevel.LOG)
            Debug.Log(message);
    }
}
