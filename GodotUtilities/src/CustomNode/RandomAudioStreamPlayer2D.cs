using System.Linq;
using Godot;

namespace GodotUtilities.CustomNode
{
    [Tool]
    public class RandomAudioStreamPlayer2D : Node2D
    {
        [Export]
        private bool randomPitch;
        [Export]
        private float minPitch = .9f;
        [Export]
        private float maxPitch = 1.1f;

        public void Play()
        {
            var validChildren = GetChildren().Cast<AudioStreamPlayer2D>().Where(x => !x.Playing);
            var childCount = validChildren.Count();
            if (childCount == 0) return;
            var childIdx = MathUtil.RNG.RandiRange(0, childCount - 1);

            var child = validChildren.ElementAt(childIdx);
            if (randomPitch)
            {
                child.PlayWithPitchRange(minPitch, maxPitch);
            }
            else
            {
                child.Play();
            }
        }

        public void PlayTimes(int times)
        {
            for (int i = 0; i < times; i++)
            {
                Play();
            }
        }

        public override string _GetConfigurationWarning()
        {
            if (GetChildren().Cast<Node>().Any(x => !(x is AudioStreamPlayer2D)))
            {
                return $"All children must be of type {typeof(AudioStreamPlayer2D).Name}";
            }
            return string.Empty;
        }
    }
}
