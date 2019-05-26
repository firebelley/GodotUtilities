using Godot;

namespace GodotTools.Extension
{
    public static class AudioStreamPlayerExtension
    {
        public static void Play(this AudioStreamPlayer audioStreamPlayer, float minPitchScale, float maxPitchScale)
        {
            audioStreamPlayer.PitchScale = (float) GD.RandRange(minPitchScale, maxPitchScale);
            audioStreamPlayer.Play();
        }
    }
}