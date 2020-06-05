using Godot;

public class EnemyPaco : RigidBody2D
{
    [Export] public int Speed = 200;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // TODO : Add random to decide if it's a floppy shooter
        // TODO : Add a timer to shoot bullets
    }
    
    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }
}
