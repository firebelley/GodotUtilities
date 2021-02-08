using Godot;

namespace GodotUtilities
{
    public static class AudioStreamPlayerExtension
    {
        public static void PlayWithPitchRange(this AudioStreamPlayer audioStreamPlayer, float minPitchScale, float maxPitchScale)
        {
            audioStreamPlayer.PitchScale = MathUtil.RNG.RandfRange(minPitchScale, maxPitchScale);
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
            audioStreamPlayer.PitchScale = MathUtil.RNG.RandfRange(minPitchScale, maxPitchScale);
            audioStreamPlayer.Play();
        }
    }
}
