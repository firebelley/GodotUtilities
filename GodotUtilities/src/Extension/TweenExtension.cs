namespace GodotUtilities;

public static class TweenExtension
{
    public static void KillIfValid(this Tween tween)
    {
        if (tween.IsValid())
        {
            tween.Kill();
        }
    }
}
