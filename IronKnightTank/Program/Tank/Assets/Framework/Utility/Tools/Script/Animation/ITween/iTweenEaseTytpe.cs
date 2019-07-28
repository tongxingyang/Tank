using System;
using UnityEngine;

public partial class iTween
{
    public static iTween Tween=new iTween(null);

    public static iTween GetITween(GameObject root, string id)
    {
        foreach (var component in root.GetComponents<iTween>())
        {
            if (component.id == id) return component;
        }
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>start, end, value,result</returns>
    public static Func<float, float, float, float> GetEaseFunction(EaseType easeType)
    {
        switch (easeType)
        {
            case EaseType.easeInQuad:
                return Tween.easeInQuad;
            case EaseType.easeOutQuad:
                return Tween.easeOutQuad;
            case EaseType.easeInOutQuad:
                return Tween.easeInOutQuad;
            case EaseType.easeInCubic:
                return Tween.easeInCubic;
            case EaseType.easeOutCubic:
                return Tween.easeOutCubic;
            case EaseType.easeInOutCubic:
                return Tween.easeInOutCubic;
            case EaseType.easeInQuart:
                return Tween.easeInQuart;
            case EaseType.easeOutQuart:
                return Tween.easeOutQuart;
            case EaseType.easeInOutQuart:
                return Tween.easeInOutQuart;
            case EaseType.easeInQuint:
                return Tween.easeInQuint;
            case EaseType.easeOutQuint:
                return Tween.easeOutQuint;
            case EaseType.easeInOutQuint:
                return Tween.easeInOutQuint;
            case EaseType.easeInSine:
                return Tween.easeInSine;
            case EaseType.easeOutSine:
                return Tween.easeOutSine;
            case EaseType.easeInOutSine:
                return Tween.easeInOutSine;
            case EaseType.easeInExpo:
                return Tween.easeInExpo;
            case EaseType.easeOutExpo:
                return Tween.easeOutExpo;
            case EaseType.easeInOutExpo:
                return Tween.easeInOutExpo;
            case EaseType.easeInCirc:
                return Tween.easeInCirc;
            case EaseType.easeOutCirc:
                return Tween.easeOutCirc;
            case EaseType.easeInOutCirc:
                return Tween.easeInOutCirc;
            case EaseType.linear:
                return Tween.linear;
            case EaseType.spring:
                return Tween.spring;
            /* GFX47 MOD START */
            /*case EaseType.bounce:
                return Tween.bounce);
                break;*/
            case EaseType.easeInBounce:
                return Tween.easeInBounce;
            case EaseType.easeOutBounce:
                return Tween.easeOutBounce;
            case EaseType.easeInOutBounce:
                return Tween.easeInOutBounce;
            /* GFX47 MOD END */
            case EaseType.easeInBack:
                return Tween.easeInBack;
            case EaseType.easeOutBack:
                return Tween.easeOutBack;
            case EaseType.easeInOutBack:
                return Tween.easeInOutBack;
            /* GFX47 MOD START */
            /*case EaseType.elastic:
                return Tween.elastic);
                break;*/
            case EaseType.easeInElastic:
                return Tween.easeInElastic;
            case EaseType.easeOutElastic:
                return Tween.easeOutElastic;
            case EaseType.easeInOutElastic:
                return Tween.easeInOutElastic;
            /* GFX47 MOD END */
        }
        return Tween.linear;
    }
}
