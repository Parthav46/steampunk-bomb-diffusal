using Godot;
using System;

public partial class Start : Sprite2D
{
    [Signal]
    public delegate void StartGameEventHandler();

    public override void _Ready()
    {
        GetNode<Button>("Start").Pressed += OnStartButtonPressed;
    }

    public void OnStartButtonPressed()
    {
        EmitSignal(SignalName.StartGame);
    }
}
