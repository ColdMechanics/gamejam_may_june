using Godot;

public class Level1 : Node2D
{
    [Export] public PackedScene EnemyPaco;

    [Export] public int EnemyPacoPackCount = 5;

    private Node2D _gameNode;
    
    private Menu _menu;

    private bool _isCancelPressed;

    private Timer _enemyPacoSpawnTimer;

    private int _enemyPacoSpawnCount;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _gameNode = GetNode<Node2D>("Game");
        _menu = GetNode<Menu>("Menu");
        
        _isCancelPressed = false;
        _enemyPacoSpawnCount = 0;
        _enemyPacoSpawnTimer = GetNode<Timer>("Game/EnemyPacoSpawnTimer");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("ui_cancel") && !_isCancelPressed)
        {
            _isCancelPressed = true;
            MenuToggle();
        }
        else if (!Input.IsActionPressed("ui_cancel"))
        {
            _isCancelPressed = false;
        }
    }

    public void OnEnemyPacoTimerTimeout()
    {
        _enemyPacoSpawnTimer.Start();
    }

    public void OnEnemyPacoSpawnTimerTimeout()
    {
        if (_enemyPacoSpawnCount < EnemyPacoPackCount)
        {
            _enemyPacoSpawnCount++;
            var enemyPacoInstance = (EnemyPacoPath) EnemyPaco.Instance();
            _gameNode.AddChild(enemyPacoInstance);
        }
        else
        {
            _enemyPacoSpawnCount = 0;
            _enemyPacoSpawnTimer.Stop();
        }
    }

    private void MenuToggle()
    {
        if (GetTree().Paused)
        {
            OnMenuUnpause();
        }
        else
        {
            _menu.Show();
            GetTree().Paused = true;
        }
    }

    public void OnMenuUnpause()
    {
        _menu.Hide();
        GetTree().Paused = false;
    }

    public void OnMenuQuit()
    {
        GetTree().Quit();
    }
}
