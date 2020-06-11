using Godot;

public class PlayerBullet : Area2D
{
    [Export]
    public float Speed = 1000;

    [Export]
    public float Damage = 1;

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        Vector2 velocity = new Vector2
        {
            x = this.Speed * delta
        };
        
        Vector2 nextPosition = this.Position + velocity;
        this.Position = nextPosition;
    }
    
    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }
}
