# GodotUtilities

## Usage

Built for Godot 4. Use versions <= 2 for Godot 3 compatibility.

Add `Firebelley.GodotUtilities` to your `.csproj`: https://www.nuget.org/packages/Firebelley.GodotUtilities/#readme-body-tab

## Source Generation
You may find it unwieldy in C# to constantly be assigning member variables to nodes using `GetNode`. Consider:
```csharp
using Godot;

public partial class MyClass : Node {
    private Label label;
    private Sprite2D sprite2d;
    private AudioStreamPlayer audioStreamPlayer;
    
    public override void _Ready() {
        label = GetNodeOrNull<Label>("Label");
        sprite2d = GetNodeOrNull<Sprite2D>("Sprite2D");
        // and on...
    }
}
```
This is a lot of boilerplate to write. This repository includes a source generator to reduce this boilerplate, based on [this repository](https://github.com/Cat-Lips/GodotSharp.SourceGenerators). Be sure to check out that repository for a more sophisticated set of generators.

With the included source generator, you can now write this code like so with the `[Scene]` and `[Node]` attributes:
```csharp
using Godot;
using GodotUtilities;

[Scene]
public partial class MyClass : Node {
    [Node]
    private Label label;
    [Node]
    private Sprite2D sprite2d;
    [Node("Some/Nested/AudioStreamPlayer")] // a node path can optionally be supplied
    private AudioStreamPlayer audioStreamPlayer;
    
    public override void _Notification(int what) {
        if (what == NotificationSceneInstantiated) {
            WireNodes(); // this is a generated method
        }
    }
}
```

I recommend using the `NotificationSceneInstantiated` notification because this will make your node assignments available immediately upon instantiating a scene. However, you can call `WireNodes` whenever you want. Be aware that nodes will not be assigned until this method is called.

Nodes are matched using the following rules. The first match is assigned to the member.
1. Get a node at the node path if supplied in the `[Node]` attribute
1. Get a direct child with `PascalCase` member name: `GetNodeOrNull<Label>("MyLabel")`
1. Get a unique descendant with `PascalCase` member name: `GetNodeOrNull<Label>("%MyLabel")`
1. Get a direct child with `snake_case` member name: `GetNodeOrNull<Label>("my_label")`
1. Get a unique descendant with `snake_case` member name: `GetNodeOrNull<Label>("%my_label)`
1. Get a direct child with `camelCase` member name: `GetNodeOrNull<Label>("myLabel")`
1. Get a unique descendant with `camelCase` member name: `GetNodeOrNull<Label>("%myLabel)`
1. Get a direct child with member name converted to lower case: `GetNodeOrNull<Label>("mylabel")`
1. Get a unique descendant with member name converted to lower case: `GetNodeOrNull<Label>("%mylabel")`

An error will be printed in the console if nothing was matched.

## Delegate State Machine
There is a `DelegateStateMachine` which is a simple finite state machine that can be used like so:
```csharp
using GodotUtilities.Logic;

private DelegateStateMachine stateMachine = new DelegateStateMachine();

public override void _Ready() {
    stateMachine.AddStates(StateNormal, EnterStateNormal, LeaveStateNormal);
    stateMachine.AddStates(StateFlee);
    stateMachine.AddStates(StateAttack, null, LeaveStateAttack);
    stateMachine.SetInitialState(StateNormal); // important. The initial state will have its enter function called
}

public override void _Process(double delta) {
    stateMachine.Update(); // triggers an update of the current state and calls enter and leave states as necessary
}

private void EnterStateNormal() {
    // called when normal state is entered
}

private void StateNormal() {
    // frame-by-frame state logic here
    // when you want to change states:
    stateMachine.ChangeState(StateAttack);
}

private void LeaveStateNormal() {
    // called when normal state is left
}

// so on...
```

## Extensions
There are various extensions available to make your development experience more streamlined. Take a look through the source code to get an idea of what is available to you.

## A Note on Versioning

This project follows semantic versioning. All minor and patch version upgrades will be safe to use in your project. Major version upgrades will contain breaking API changes.

If you're using version 3.x, then any future 3.x version will be backward compatible with your project.
