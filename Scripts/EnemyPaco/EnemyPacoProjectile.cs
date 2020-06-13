using Godot;

public class EnemyPacoProjectile : RigidBody2D
{
    [Export] public int Damage = 1;

    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }
    
    public void DisableCollisions()
    {
        _collisionShape.SetDeferred("disabled", true);
    }
}
