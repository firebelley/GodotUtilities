using Godot;

namespace GodotUtilities
{
    public static class AnimationTreeExtension
    {
        public static T GetPlayback<T>(this AnimationTree animationTree) where T : Resource
        {
            return animationTree.Get("parameters/playback") as T;
        }
    }
}