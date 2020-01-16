using Godot;

namespace GodotUtilities.Extension
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

        public static void PlayWithPitch(this AudioStreamPlayer2D audioStreamPlayer, float pitchScale)
        {
            audioStreamPlayer.PitchScale = pitchScale;
            audioStreamPlayer.Play();
        }

        public static void PlayWithPitchRange(this AudioStreamPlayer2D audioStreamPlayer, float minPitchScale, float maxPitchScale)
        {
            audioStreamPlayer.PitchScale = (float) GD.RandRange(minPitchScale, maxPitchScale);
            audioStreamPlayer.Play();
        }
    }
}