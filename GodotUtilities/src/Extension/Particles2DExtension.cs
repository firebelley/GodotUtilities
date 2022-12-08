using Godot;

namespace GodotUtilities
{
    public static class Particles2DExtension
    {
        public static void SetDirection(this GPUParticles2D particles, Vector2 direction)
        {
            if (particles.ProcessMaterial is ParticleProcessMaterial material)
            {
                material.Direction = new Vector3(direction.x, direction.y, 0f);
            }
        }
    }
}
