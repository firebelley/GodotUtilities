using Godot;

namespace GodotUtilities
{
    public static class ProjectSettingsExtended
    {
        public static T GetSettingOrDefault<T>(string name)
        {
            if (ProjectSettings.HasSetting(name))
            {
                return (T) ProjectSettings.GetSetting(name);
            }
            return default(T);
        }
    }
}