# GodotUtilities

**NOTE**: This code is not designed for general public use. It was developed to meet my personal development needs and as such several features may be unclear. The nuspec versioning also violates the standards of semantic versioning. Nevertheless, you may find something here that is useful in your project.

## Building

There is a post build event that will invoke `nuget.exe` in your path (Windows) and will create a `.nupkg` in `bin/Release`. You can then pull this `.nupkg` into your Godot project.

## Other Notes

There is an attribute which can automatically set nodes for you. It will find the Node that is a child that has the same name as the variable name.

```cd
using GodotUtilities;

[Node]
private Sprite sprite; // Finds node with name "Sprite" or "sprite"

[Node("HBoxContainer/Label")] // If a Node path is specified, that will be used instead
private Label someLabel;

public override void _EnterTree() {
    this.WireNodes(); // assigns all nodes with a [Node] attribute
}
```

Special characters are stripped when trying to match names, so if you choose to use `_` to denote private variables you don't need to include `_` in the node name.
