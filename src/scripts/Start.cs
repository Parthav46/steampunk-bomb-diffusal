using Godot;
using System;

public partial class Start : Sprite2D
{
    [Signal]
    public delegate void StartGameEventHandler();

    public void OnStartButtonPressed()
    {
        EmitSignal(SignalName.StartGame);
    }
}
