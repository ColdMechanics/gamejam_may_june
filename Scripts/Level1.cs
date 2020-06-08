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
        this._gameNode = GetNode<Node2D>("Game");
        this._menu = GetNode<Menu>("Menu");

        this._isCancelPressed = false;
        this._enemyPacoSpawnCount = 0;
        this._enemyPacoSpawnTimer = GetNode<Timer>("Game/EnemyPacoSpawnTimer");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("ui_cancel") && !this._isCancelPressed)
        {
            this._isCancelPressed = true;
            MenuToggle();
        }
        else if (!Input.IsActionPressed("ui_cancel"))
        {
            this._isCancelPressed = false;
        }
    }

    public void OnEnemyPacoTimerTimeout()
    {
        this._enemyPacoSpawnTimer.Start();
    }

    public void OnEnemyPacoSpawnTimerTimeout()
    {
        if (this._enemyPacoSpawnCount < this.EnemyPacoPackCount)
        {
            this._enemyPacoSpawnCount++;
            var enemyPacoInstance = (EnemyPacoPath) this.EnemyPaco.Instance();
            this._gameNode.AddChild(enemyPacoInstance);
        }
        else
        {
            this._enemyPacoSpawnCount = 0;
            this._enemyPacoSpawnTimer.Stop();
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
            this._menu.Show();
            GetTree().Paused = true;
        }
    }

    public void OnMenuUnpause()
    {
        this._menu.Hide();
        GetTree().Paused = false;
    }

    public void OnMenuQuit()
    {
        GetTree().Quit();
    }
}
