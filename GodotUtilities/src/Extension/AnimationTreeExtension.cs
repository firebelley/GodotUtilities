using Godot;

namespace GodotUtilities.Extension
{
    public static class AnimationTreeExtension
    {
        public static T GetPlayback<T>(this AnimationTree animationTree) where T : AnimationRootNode
        {
            return animationTree.Get("parameters/playback") as T;
        }
    }
}