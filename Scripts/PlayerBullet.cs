using Godot;

public class PlayerBullet : Area2D
{
    [Export]
    public float Speed = 1000;

    [Export]
    public int Damage = 1;

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

    public void OnPlayerBulletBodyEntered(Node body)
    {
        if (body is EnnemyBase enemy)
        {
            enemy.Hit(Damage);
            QueueFree();
        }
    }
}
