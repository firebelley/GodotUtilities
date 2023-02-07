using Godot;

namespace GodotUtilities
{
    public static class Particles2DExtension
    {
        public static void SetDirection(this GpuParticles2D particles, Vector2 direction)
        {
            if (particles.ProcessMaterial is ParticleProcessMaterial material)
            {
                material.Direction = new Vector3(direction.X, direction.Y, 0f);
            }
        }
    }
}
