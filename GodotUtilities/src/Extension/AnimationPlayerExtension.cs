namespace GodotUtilities;

public static class AnimationPlayerExtension
{
    private static readonly StringName ANIM_RESET = "RESET";

    public static void ResetAndPlay(this AnimationPlayer animationPlayer, StringName animation, double customBlend = -1, float customSpeed = 1f, bool fromEnd = false)
    {
        animationPlayer.Play(ANIM_RESET);
        animationPlayer.Seek(0, true);
        animationPlayer.Play(animation, customBlend, customSpeed, fromEnd);
    }
}
