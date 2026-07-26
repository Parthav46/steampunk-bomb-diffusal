using Godot;
using System;

public partial class MeterPin : Node2D
{
    private const float Offset = -Mathf.Pi / 3;
    private const float RotationMultiplier = Mathf.Pi / 30;
    private float progress = 0; // To 600
    private float speed = 0f;

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public override void _Process(double delta)
    {
        progress += speed * (float)delta;
        if (progress > 20)
        {
            progress = 20;
            GD.Print("Blast");
        }
        float rotation = progress * RotationMultiplier;
        rotation += Offset;
        Rotation = rotation;
    }
}
