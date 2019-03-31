using UnityEngine;

public enum Ease
{
    Linear             = 0,
    EaseInOutQuadratic = 1,
    EaseInOutCubic     = 2,
    EaseInOutQuartic   = 3
}

public class Tween
{
    public static Vector2 EaseVector2(Vector2 a, Vector2 b, float t, Ease e)
    {
        return new Vector2(EaseFloat(a.x, b.x, t, e), EaseFloat(a.y, b.y, t, e));
    }

    public static float EaseFloat(float a, float b, float t, Ease e)
    {
        switch (e)
        {
            case Ease.Linear:             
                return EaseFloatLinear(a,b,t);
            case Ease.EaseInOutQuadratic: 
                return EaseFloatInOutQuadratic(a,b,t);
            case Ease.EaseInOutCubic:
                return EaseFloatInOutCubic(a,b,t);
            case Ease.EaseInOutQuartic:
                return EaseFloatInOutQuartic(a,b,t);
        }
        return a;
    }

    private static float EaseFloatLinear(float a, float b, float t)
    {
        return Mathf.Lerp(a, b, t);
    }

    private static float EaseFloatInOutQuadratic(float a, float b, float t)
    {
        return Mathf.Lerp(a, b, (t < 0.5f) ? 2*t*t : -1+(4-2*t)*t);
    }

    private static float EaseFloatInOutCubic(float a, float b, float t)
    {
        return Mathf.Lerp(a, b, (t < 0.5f) ? 4*t*t*t : (t-1)*(2*t-2)*(2*t-2)+1);
    }

    private static float EaseFloatInOutQuartic(float a, float b, float t)
    {
        return Mathf.Lerp(a, b, (t < 0.5f) ? 8*t*t*t*t : 1-8*(--t)*t*t*t);
    } 

}