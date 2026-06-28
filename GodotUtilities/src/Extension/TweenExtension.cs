namespace GodotUtilities;

public static class TweenExtension
{
    private const string PROPERTY_POSITION = "position";
    private const string PROPERTY_GLOBAL_POSITION = "global_position";
    private const string PROPERTY_SCALE = "scale";

    public static void KillIfValid(this Tween tween)
    {
        if (tween.IsValid())
        {
            tween.Kill();
        }
    }

    public static PropertyTweener TweenPosition(this Tween tween, Node node, Vector2 toPosition, float duration)
    {
        return tween.TweenProperty(node, PROPERTY_POSITION, toPosition, duration);
    }

    public static PropertyTweener TweenGlobalPosition(this Tween tween, Node node, Vector2 toPosition, float duration)
    {
        return tween.TweenProperty(node, PROPERTY_GLOBAL_POSITION, toPosition, duration);
    }

    public static PropertyTweener TweenScale(this Tween tween, Node node, Vector2 toScale, float duration)
    {
        return tween.TweenProperty(node, PROPERTY_SCALE, toScale, duration);
    }

    public static CallbackTweener TweenAction(this Tween tween, Action callback)
    {
        return tween.TweenCallback(Callable.From(callback));
    }

    public static MethodTweener TweenCustom<[MustBeVariant] T>(this Tween tween, Action<T> action, T start, T end, float time)
    {
        return tween.TweenMethod(Callable.From(action), Variant.From(start), Variant.From(end), time);
    }

    public static PropertyTweener TweenControlOffsetTransform(this Tween tween, Control control, Vector2 offsetPosition, float time)
    {
        return tween.TweenProperty(control, (string)Control.PropertyName.OffsetTransformPosition, offsetPosition, time);
    }

    public static PropertyTweener TweenControlOffsetTransformPositionRatio(this Tween tween, Control control, Vector2 ratio, float time)
    {
        return tween.TweenProperty(control, (string)Control.PropertyName.OffsetTransformPositionRatio, ratio, time);
    }

    public static PropertyTweener TweenControlOffsetRotation(this Tween tween, Control control, float rotationDegrees, float time)
    {
        return tween.TweenProperty(control, (string)Control.PropertyName.OffsetTransformRotation, Mathf.DegToRad(rotationDegrees), time);
    }

    public static PropertyTweener TweenControlOffsetScale(this Tween tween, Control control, Vector2 scale, float time)
    {
        return tween.TweenProperty(control, (string)Control.PropertyName.OffsetTransformScale, scale, time);
    }
}
