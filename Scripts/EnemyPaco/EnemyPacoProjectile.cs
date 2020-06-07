using Godot;

public class EnemyPacoProjectile : RigidBody2D
{
    [Export] public int Speed;
    
    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }

    public override void _Process(float delta)
    {
    }
}
