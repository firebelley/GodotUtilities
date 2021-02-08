using System.Linq;
using Godot;
using GodotUtilities;

namespace Game.CustomNode
{
    [Tool]
    public class RandomAudioStreamPlayer : Node
    {
        [Export]
        private bool randomPitch;
        [Export]
        private float minPitch = .9f;
        [Export]
        private float maxPitch = 1.1f;

        public void Play()
        {
            var validChildren = GetChildren().Cast<AudioStreamPlayer>().Where(x => !x.Playing);
            var childCount = validChildren.Count();
            if (childCount == 0) return;
            var childIdx = MathUtil.RNG.RandiRange(0, childCount - 1);

            var child = GetChild<AudioStreamPlayer>(childIdx);
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
            if (GetChildren().Cast<Node>().Any(x => !(x is AudioStreamPlayer)))
            {
                return $"All children must be of type {typeof(AudioStreamPlayer).Name}";
            }
            return string.Empty;
        }
    }
}
