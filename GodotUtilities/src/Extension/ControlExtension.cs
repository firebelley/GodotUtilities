namespace GodotUtilities;

public static class ControlExtension
{
    public static void CenterPivotOffset(this Control control) => control.PivotOffset = control.Size / 2f;

    public static Vector2 ToLocal(this Control control, Vector2 globalPosition) => control.GetGlobalTransform().AffineInverse() * globalPosition;
}
