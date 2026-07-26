using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class FiveGuageDashboard : Node2D
{
    [Export]
    public Texture2D[] GuageTextures { get; set; }
    [Export]
    public Texture2D[] PinTextures { get; set; }
    [Export]
    public PackedScene GuageScene { get; set; }

    private int[] order;
    private Guage[] guages;
    private MeterPin meterPin;
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

        Array<Node> guageMarkerNodes = GetTree().GetNodesInGroup("guage_markers");
        Marker2D[] guageMarkers = guageMarkerNodes.OfType<Marker2D>().ToArray();
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
            GD.Print($"{i} -> ");
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

        meterPin = GetNode<MeterPin>("MeterPin");
        meterPin.SetSpeed(speed);
    }

    public void DiffuseGuage(int ratio)
    {
        int index = System.Array.IndexOf(order, ratio);
        if (index < 0)
        {
            GD.PushWarning($"Unknown guage ratio: {ratio}");
            return;
        }

        if (index == 0)
        {
            GD.Print("Diffused");
            guages[ratio - 1].SetSpeed(0f);
            foreach (Guage guage in guages)
            {
                guage.SetProcess(false);
            }
            meterPin.SetProcess(false);
        }
        else
        {
            Guage guage = guages[ratio - 1];
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
