using Elements.Core;

namespace ArbitraryComponentAccess;

internal static class UniLogPlugin
{
    public static void Log(object obj, bool stackTrace = false)
    {
        string s = (obj != null) ? obj.ToString() : "NULL";
        UniLog.Log($"[ArbitraryComponentAccess]: {s}", stackTrace);
    }

    public static void Warning(object obj, bool stackTrace = false)
    {
        string s = (obj != null) ? obj.ToString() : "NULL";
        UniLog.Warning($"[ArbitraryComponentAccess]: {s}", stackTrace);
    }

    public static void Error(object obj, bool stackTrace = true)
    {
        string s = (obj != null) ? obj.ToString() : "NULL";
        UniLog.Error($"[ArbitraryComponentAccess]: {s}", stackTrace);
    }
}