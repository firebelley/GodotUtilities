global using Godot;
global using System;
global using System.Linq;
global using System.Threading.Tasks;

namespace GodotUtilities;

public static class ProjectSettingsExtended
{
    /// <summary>
    /// Returns the given project setting, or default if the setting doesn't exist.
    /// </summary>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetSettingOrDefault<T>(string name) => ProjectSettings.HasSetting(name) ? ProjectSettings.GetSetting(name).As<T>() : default;

    /// <summary>
    /// Returns the given project setting if not a debug build. Returns default value if a debug build or if the setting doesn't exist.
    /// </summary>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetDebugSettingOrDefault<T>(string name) => OS.IsDebugBuild() ? GetSettingOrDefault<T>(name) : default;
}
