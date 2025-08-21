using Elements.Core;

namespace ArbitraryComponentAccess;

internal static class UniLogPlugin
{
    public static void Log(object obj, bool stackTrace = false)
    {
        string s = obj.ToString() ?? "NULL";
        UniLog.Log($"[Arbitrarily Contained Anarchy]: {s}", stackTrace);
    }

    public static void Warning(object obj, bool stackTrace = false)
    {
        string s = obj.ToString() ?? "NULL";
        UniLog.Warning($"[Arbitrarily Contained Anarchy]: {s}", stackTrace);
    }

    public static void Error(object obj, bool stackTrace = true)
    {
        string s = obj.ToString() ?? "NULL";
        UniLog.Error($"[Arbitrarily Contained Anarchy]: {s}", stackTrace);
    }
}