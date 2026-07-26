using Godot;
using System;

public partial class Guage : Node2D
{
    [Signal]
    public delegate void DiffusedEventHandler(int ratio);

    private const double RotationMultiplier = 2 * Math.PI;
    private Sprite2D _pin;
    private Sprite2D _diffusal;

    private float _speed = 0f;
    private int _ratio = 0;
    private bool _isDiffused = false;
    private Texture2D _guageTexture;
    private Texture2D _pinTexture;

    public float Output { get; private set; } = 0f;

    public void Initialize(Texture2D guageTexture, Texture2D pinTexture, float speed, int ratio)
    {
        _pinTexture = pinTexture;
        _guageTexture = guageTexture;

        _speed = speed;
        _ratio = ratio;
        _isDiffused = false;
        UpdateOutput();
    }

    public override void _Ready()
    {
        _pin = GetNode<Sprite2D>("Pin");
        _pin.Texture = _pinTexture;

        GetNode<Sprite2D>("Label").Texture = _guageTexture;

        _diffusal = GetNode<Sprite2D>("DiffusalPin");
        _diffusal.Visible = false;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
        UpdateOutput();
    }

    public void Diffuse()
    {
        if (_isDiffused) return;
        _isDiffused = true;
        _diffusal.Visible = true;
        UpdateOutput();
        EmitSignal(SignalName.Diffused, _ratio);
    }

    public override void _Process(double delta)
    {
        double rotation = _speed;
        rotation *= _ratio * RotationMultiplier * delta;
        
        rotation %= RotationMultiplier;
        _pin.Rotation += (float) rotation;
    }

    public void OnGuageEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            Diffuse();
        }
    }

    private void UpdateOutput()
    {
        Output = _isDiffused ? _speed * _ratio : _speed;
    }
}
