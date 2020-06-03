using Godot;
using System;

public class HealthBar : TextureProgress
{
    private Sprite _hearth;
    
    [Export]
    public Color ColorHealthy = Colors.Green;

    [Export]
    public Color ColorCaution = Colors.Yellow;

    [Export]
    public Color ColorDanger = Colors.Red;

    [Export(PropertyHint.Range, "0,1,0.05")]
    public float CautionZone = 0.5f;

    [Export(PropertyHint.Range, "0,1,0.05")]
    public float DangerZone = 0.2f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this._hearth = GetNode<Sprite>("HearthBackground/HearthContent");
    }

    public void SetValue(float percent)
    {
        this.Value = percent * 100;
        AssignColor(percent);
    }

    private void AssignColor(float percent)
    {
        if (percent > this.CautionZone)
        {
            this.TintProgress = this.ColorHealthy;
            this._hearth.Modulate = this.ColorHealthy;
        }
        else if (percent > this.DangerZone)
        {
            this.TintProgress = this.ColorCaution;
            this._hearth.Modulate = this.ColorCaution;
        }
        else
        {
            this.TintProgress = this.ColorDanger;
            this._hearth.Modulate = this.ColorDanger;
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}