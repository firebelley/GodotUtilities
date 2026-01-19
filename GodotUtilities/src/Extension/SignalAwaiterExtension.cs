namespace GodotUtilities;

public static class SignalAwaiterExtension
{
    public static Task<Variant[]> ToTask(this SignalAwaiter awaiter)
    {
        return Task.Run(async () => await awaiter);
    }
}
