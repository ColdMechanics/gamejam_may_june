using Godot;

public abstract class EnnemyBase : RigidBody2D
{
    [Export]
    public float SpawnRate = 0.1f;
    
    [Export]
    public int Speed = 200;

    [Export]
    public int ScoreValue = 100;

    [Export]
    public int ContactDamage = 1;

    public virtual void DisableCollisions()
    {
    }

    public virtual void Die()
    {
    }
}
