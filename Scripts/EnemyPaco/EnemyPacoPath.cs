using Godot;

public class EnemyPacoPath : Node2D
{
    private EnemyPaco _enemyPaco;

    private PathFollow2D _pathFollow2D;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _enemyPaco = GetNode<EnemyPaco>("Path2D/PathFollow2D/Enemy");
        _pathFollow2D = GetNode<PathFollow2D>("Path2D/PathFollow2D");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (GetChildren().Count == 1 && _pathFollow2D.GetChildren().Count == 0)
        {
            QueueFree();
            return;
        }
        
        var offsetDelta = _enemyPaco.Speed * delta;
        _pathFollow2D.Offset += offsetDelta;
    }

    public void OnEnemyShootBullet(EnemyPacoProjectile instance, int projectileSpeed, bool isDerp)
    {
        if (isDerp)
        {
            projectileSpeed /= 2;
            instance.GravityScale = 15;
            instance.AngularVelocity = -3;
        }
        
        AddChild(instance);

        instance.Position = new Vector2(_enemyPaco.GlobalPosition.x - 85, _enemyPaco.GlobalPosition.y);
        instance.LinearVelocity = new Vector2(-projectileSpeed, 0);
    }
}
