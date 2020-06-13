using System;
using Godot;

public class Level1 : Node2D
{
    [Export]
    public PackedScene EnemyPaco;

    [Export]
    public int EnemyPacoPackCount = 5;

    [Export]
    public PackedScene Cloud;

    [Export]
    public PackedScene Enemy1;

    [Export]
    public PackedScene Enemy2;

    [Export]
    public PackedScene Enemy3;

    [Export]
    public PackedScene Enemy4;

    [Export]
    public Boss Boss;

    public static Player Player => _player;
    public static HUD HUD => _hud;

    private readonly Random _random = new Random();

    private Node2D _gameNode;

    private Menu _menu;

    private Node2D _cloudRoot;

    private static Player _player;
    private static HUD _hud;

    private bool _isCancelPressed;

    private Timer _enemyPacoSpawnTimer;

    private int _enemyPacoSpawnCount;

    private float _spawnProbability;

    private PathFollow2D _mobSpawnLocation;

    private bool _hasBoss = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this._gameNode = GetNode<Node2D>("Game");
        this._menu = GetNode<Menu>("MenuCanvas/Menu");
        this._cloudRoot = GetNode<Node2D>("Game/CloudRoot");

        _player = GetNode<Player>("Game/Player");
        _hud = GetNode<HUD>("Game/HUD");

        this._isCancelPressed = false;
        this._enemyPacoSpawnCount = 0;
        this._enemyPacoSpawnTimer = GetNode<Timer>("Game/EnemyPacoSpawnTimer");

        this._spawnProbability = 1; //this.Enemy1.SpawnRate + this.Enemy2.SpawnRate + this.Enemy3.SpawnRate +
        // this.Enemy4.SpawnRate;

        this._mobSpawnLocation = GetNode<PathFollow2D>("Game/MonsterSpawner/MonsterSpawnerLocation");
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

    public void OnCloudTimerTimeout()
    {
        var cloudInstance = this.Cloud.Instance();
        _cloudRoot.AddChild(cloudInstance);
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

    public void OnMonsterTimerTimeout()
    {
        if (this._hasBoss) return;

        // Choose a random location on Path2D.
        this._mobSpawnLocation.Offset = this._random.Next();

        // Create a EnnemyBase instance and add it to the scene.
        RigidBody2D mobInstance;

        float randMonster = RandRange(0, this._spawnProbability);

        // if (randMonster <= this.Enemy1.SpawnRate)
        if (randMonster <= 0.25)
            mobInstance = (RigidBody2D) this.Enemy1.Instance();
        // else if (randMonster <= (this.Enemy1.SpawnRate + this.Enemy2.SpawnRate))
        else if (randMonster <= 0.5)
            mobInstance = (RigidBody2D) this.Enemy2.Instance();
        // else if (randMonster <= (this.Enemy1.SpawnRate + this.Enemy2.SpawnRate + this.Enemy3.SpawnRate))
        else if (randMonster <= 0.75)
            mobInstance = (RigidBody2D) this.Enemy3.Instance();
        else
            mobInstance = (RigidBody2D) this.Enemy4.Instance();

        AddChild(mobInstance);

        // Set the mob's position to a random location.
        mobInstance.Position = this._mobSpawnLocation.Position;
    }

    public void OnBossTimerTimeout()
    {
        if (this._hasBoss) return;

        this._hasBoss = true;

        // Choose a random location on Path2D.
        this._mobSpawnLocation.Offset = this._random.Next();

        // Create a Boss instance and add it to the scene.
        RigidBody2D mobInstance = this.Boss;

        AddChild(mobInstance);

        // Set the mob's position to a random location.
        mobInstance.Position = this._mobSpawnLocation.Position;
    }

    private float RandRange(float min, float max)
    {
        return (float) this._random.NextDouble() * (max - min) + min;
    }
}