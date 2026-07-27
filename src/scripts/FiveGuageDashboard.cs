using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class FiveGuageDashboard : Node2D
{
    [Signal]
    public delegate void NextGameEventHandler(bool success);
    [Export]
    public Texture2D[] GuageTextures { get; set; }
    [Export]
    public Texture2D[] PinTextures { get; set; }
    [Export]
    public PackedScene GuageScene { get; set; }

    private int[] order;
    private Guage[] guages;
    private MeterPin meterPin;
    private ItemList pinList;
    public override void _Ready()
    {
        if (GuageScene == null)
        {
            GD.PushError("GuageScene is not set on FiveGuageDashboard.");
            return;
        }
        if (GuageTextures == null || GuageTextures.Length == 0 || PinTextures == null || PinTextures.Length == 0)
        {
            GD.PushError("GuageTextures and PinTextures must both be set with at least one texture.");
            return;
        }

        Marker2D[] guageMarkers = GetChildren().OfType<Marker2D>().Where(n => n.IsInGroup("guage_markers")).ToArray();
        order = new int[guageMarkers.Length];
        guages = new Guage[guageMarkers.Length];
        for (int i = 0; i < guageMarkers.Length; i++)
        {
            order[i] = i + 1;
        }
        order = order.OrderBy(_ => GD.Randi()).ToArray();

        float speed = 0.1f * order[0];
        foreach(int i in order)
        {
            Marker2D marker = guageMarkers[i - 1];
            Guage guage = GuageScene.Instantiate<Guage>();
            guage.Initialize(
                GuageTextures[GD.Randi() % GuageTextures.Length],
                PinTextures[GD.Randi() % PinTextures.Length],
                speed,
                i
            );
            marker.AddChild(guage);
            guages[i - 1] = guage;
            guage.Diffused += DiffuseGuage;
            speed = guage.Output;
        }

        pinList = GetNode<ItemList>("PinList");
        meterPin = GetNode<MeterPin>("MeterPin");
        meterPin.SetSpeed(speed);
        meterPin.Timeout += () => EmitSignal(SignalName.NextGame, false);
    }

    public void DiffuseGuage(int ratio)
    {
        if (pinList.ItemCount == 0) return;
        pinList.RemoveItem(0);
        int index = System.Array.IndexOf(order, ratio);
        if (index < 0)
        {
            GD.PushWarning($"Unknown guage ratio: {ratio}");
            return;
        }

        if (index == 0)
        {
            foreach (Guage guage in guages)
            {
                guage.SetProcess(false);
            }
            meterPin.SetProcess(false);
            EmitSignal(SignalName.NextGame, true);
        }
        else
        {
            Guage guage = guages[ratio - 1];
            guage.Diffuse();
            float speed = guage.Output;
            for(int i = index; i < order.Length; i++)
            {
                guages[order[i] - 1].SetSpeed(speed);
                speed = guages[order[i] - 1].Output;
            }
            meterPin.SetSpeed(speed);
        }
    }
}
