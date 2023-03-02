# GodotUtilities

## Usage

Add `Firebelley.GodotUtilities` to your `.csproj`: https://www.nuget.org/packages/Firebelley.GodotUtilities/#readme-body-tab

## Building

Make sure `nuget.exe` is in your `PATH` environment variable. You will also need to install the .NET 6.0 SDK which should already be installed if you are using C# with Godot 4. There is a post build event that will invoke `nuget.exe` and will create a `.nupkg` in `bin/Release`. You can then pull this `.nupkg` into your Godot project. In order to do so, you will need to configure a [local nuget repository location](https://docs.microsoft.com/en-us/nuget/hosting-packages/local-feeds).

```
dotnet build -c Release
```

## Other Notes

There is an attribute which can automatically set nodes for you. It will find the Node that is a child that has the same name as the variable name.

```cd
using GodotUtilities;

[Node]
private Sprite2D sprite; // Finds node with name "Sprite" or "sprite"

[Node("HBoxContainer/Label")] // If a Node path is specified, that will be used instead
private Label someLabel;

public override void _EnterTree() {
    this.WireNodes(); // assigns all nodes with a [Node] attribute
    // you can also use the NotificationInstanced notification
    // in a _Notification override
}
```

Special characters are stripped when trying to match names, so if you choose to use `_` to denote private variables you don't need to include `_` in the node name.

## Versioning

This project follows semantic versioning. All minor and patch version upgrades will be safe to use in your project. Major version upgrades will contain breaking API changes.

If you're using version 3.x, then any future 3.x version will be backward compatible with your project.
