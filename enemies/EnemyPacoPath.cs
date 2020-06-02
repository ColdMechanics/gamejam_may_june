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
        var offsetDelta = _enemyPaco.Speed * delta;
        _pathFollow2D.Offset += offsetDelta;
    }
}
