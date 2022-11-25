using Godot;

namespace GodotUtilities
{
    public static class ControlExtension
    {
        public static void CenterPivotOffset(this Control control)
        {
            control.RectPivotOffset = control.RectSize / 2f;
        }
    }
}
