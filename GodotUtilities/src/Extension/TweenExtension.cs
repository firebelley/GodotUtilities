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
}
