using Godot;

namespace GodotApiTools.Extension
{
    public static class AudioStreamPlayerExtension
    {
        public static void PlayWithPitchRange(this AudioStreamPlayer audioStreamPlayer, float minPitchScale, float maxPitchScale)
        {
            audioStreamPlayer.PitchScale = (float) GD.RandRange(minPitchScale, maxPitchScale);
            audioStreamPlayer.Play();
        }

        public static void PlayWithPitch(this AudioStreamPlayer audioStreamPlayer, float pitchScale)
        {
            audioStreamPlayer.PitchScale = pitchScale;
            audioStreamPlayer.Play();
        }
    }
}