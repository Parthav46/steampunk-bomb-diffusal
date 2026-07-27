using Godot;
using System;

public partial class KillScreen : Sprite2D
{
    [Signal]
    public delegate void ResetEventHandler();
    private Label _comment;
    private Timer _resetTimer;
    public override void _Ready()
    {
        _comment = GetNode<Label>("Comment");
        _resetTimer = GetNode<Timer>("ResetTimer");
        _resetTimer.Timeout += () => EmitSignal(SignalName.Reset);
    }

    public void SetComment(int fixes)
    {
        string comment;
        if (fixes == 0)
        {
            comment = "You failed to diffuse the bomb.";
        } else
        {
            string bombWord = fixes == 1 ? "bomb" : "bombs";
            comment = $"You diffused {fixes} {bombWord} though. Good job!";
        }
        _comment.Text = comment;
        _resetTimer.Start();
    }
}
