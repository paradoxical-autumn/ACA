namespace ArbitraryComponentAccess;

public class ACAReflections
{
    public static object GetProp(object src, string propName)
    {
        return src.GetType().GetProperty(propName);
    }

    public static object GetPropValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName).GetValue(src, null);
    }
}