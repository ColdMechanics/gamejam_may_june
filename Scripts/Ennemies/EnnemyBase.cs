using Godot;

public abstract class EnnemyBase : RigidBody2D
{
    [Export(PropertyHint.Range, "1,100,1")]
    public int Life = 1;
    
    [Export]
    public float SpawnRate = 0.1f;
    
    [Export]
    public int Speed = 200;

    [Export]
    public uint ScoreValue = 100;

    [Export]
    public int ContactDamage = 1;

    public virtual void DisableCollisions()
    {
    }

    public virtual void Die()
    {
        Level1.HUD.AddScore(this.ScoreValue);
    }
    
    public virtual void Hit(int damage)
    {
        Life -= damage;
        if (Life <= 0)
        {
            Die();
        }
    }
}
