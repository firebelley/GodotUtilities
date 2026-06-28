namespace GodotUtilities;

public static class PropertyTweenerExtension
{
    public static PropertyTweener Out(this PropertyTweener propertyTweener) => propertyTweener.SetEase(Tween.EaseType.Out);

    public static PropertyTweener In(this PropertyTweener propertyTweener) => propertyTweener.SetEase(Tween.EaseType.In);

    public static PropertyTweener InOut(this PropertyTweener propertyTweener) => propertyTweener.SetEase(Tween.EaseType.InOut);

    public static PropertyTweener OutIn(this PropertyTweener propertyTweener) => propertyTweener.SetEase(Tween.EaseType.OutIn);

    public static PropertyTweener Linear(this PropertyTweener propertyTweener) => propertyTweener.SetTrans(Tween.TransitionType.Linear);

    public static PropertyTweener Sine(this PropertyTweener propertyTweener) => propertyTweener.SetTrans(Tween.TransitionType.Sine);

    public static PropertyTweener Quint(this PropertyTweener propertyTweener) => propertyTweener.SetTrans(Tween.TransitionType.Quint);

    public static PropertyTweener Quart(this PropertyTweener propertyTweener) => propertyTweener.SetTrans(Tween.TransitionType.Quart);

    public static PropertyTweener Quad(this PropertyTweener propertyTweener) => propertyTweener.SetTrans(Tween.TransitionType.Quad);

    public static PropertyTweener Expo(this PropertyTweener propertyTweener) => propertyTweener.SetTrans(Tween.TransitionType.Expo);

    public static PropertyTweener Elastic(this PropertyTweener propertyTweener) => propertyTweener.SetTrans(Tween.TransitionType.Elastic);

    public static PropertyTweener Cubic(this PropertyTweener propertyTweener) => propertyTweener.SetTrans(Tween.TransitionType.Cubic);

    public static PropertyTweener Circ(this PropertyTweener propertyTweener) => propertyTweener.SetTrans(Tween.TransitionType.Circ);

    public static PropertyTweener Bounce(this PropertyTweener propertyTweener) => propertyTweener.SetTrans(Tween.TransitionType.Bounce);

    public static PropertyTweener Back(this PropertyTweener propertyTweener) => propertyTweener.SetTrans(Tween.TransitionType.Back);

    public static PropertyTweener Spring(this PropertyTweener propertyTweener) => propertyTweener.SetTrans(Tween.TransitionType.Spring);
}
