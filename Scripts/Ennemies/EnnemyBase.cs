using Godot;

public abstract class EnnemyBase : RigidBody2D
{
    [Export]
    public float SpawnRate = 0.1f;
    
    [Export]
    public int Speed = 200;

    [Export]
    public int ScoreValue = 100;
}
