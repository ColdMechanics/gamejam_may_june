using Godot;

public class Boss : EnnemyBase
{
    public override void _Ready()
    {
        // TODO target the player
    }

    public override void _Process(float delta)
    {
        if (Level1.Player is null) return;

        Vector2 velocity = this.GlobalPosition.DirectionTo(Level1.Player.GlobalPosition);
        
        if((this.GlobalPosition - Level1.Player.GlobalPosition).Length() < 100) return;
        
        this.Position += (velocity * this.Speed * delta);
        LookAt(Level1.Player.GlobalPosition);
    }
}
