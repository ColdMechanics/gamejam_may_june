using Godot;

public class Player : Area2D
{
    [Signal]
    public delegate void Hit();

    [Export]
    public int Speed = 400;

    [Export]
    public int XBorder = 100;

    [Export]
    public int YBorder = 64;

    [Export]
    public int MaxHealth = 3;

    [Export]
    public int BrokenSpriteHealth = 1;

    [Export]
    public PackedScene PlayerBullet;

    public bool CanFire = true;

    private float _maxX;
    private float _maxY;
    private int _health;

    private AnimatedSprite _animatedSprite;
    private Position2D _muzzle;
    private Timer _shootCooldown;
    private Node _bulletContainer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Vector2 screenSize = GetViewport().GetVisibleRect().Size;
        this._animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        this._muzzle = GetNode<Position2D>("Muzzle");
        this._shootCooldown = GetNode<Timer>("ShootCooldown");
        this._bulletContainer = GetNode<Node>("BulletContainer");

        this._maxX = screenSize.x - this.XBorder;
        this._maxY = screenSize.y - this.YBorder;

        this._health = this.MaxHealth;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Vector2 velocity = new Vector2(); // The player's movement vector.

        if (Input.IsActionPressed("player_shoot"))
        {
            Shoot();
        }

        if (Input.IsActionPressed("ui_right"))
        {
            velocity.x += 1;
        }

        if (Input.IsActionPressed("ui_left"))
        {
            velocity.x -= 1;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            velocity.y += 1;
        }

        if (Input.IsActionPressed("ui_up"))
        {
            velocity.y -= 1;
        }

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * this.Speed * delta;

            Vector2 nextPosition = this.Position + velocity;
            this.Position = new Vector2(
                x: Mathf.Clamp(nextPosition.x, this.XBorder, this._maxX),
                y: Mathf.Clamp(nextPosition.y, this.YBorder, this._maxY)
            );
        }
    }

    public void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        if (body is EnnemyBase enemy)
        {
            Damage(enemy.ContactDamage);
            enemy.DisableCollisions();

            enemy.Die();
        }

        if (body is EnemyPacoProjectile pacoProjectile)
        {
            pacoProjectile.DisableCollisions();
            Damage(pacoProjectile.Damage);
            pacoProjectile.QueueFree();
        }
        
        EmitSignal("Hit");
    }

    private void Shoot()
    {
        if(!this.CanFire) return;

        if (this._shootCooldown.TimeLeft > 0) return;

        this._shootCooldown.Start();
        
        Area2D bullet = (Area2D)this.PlayerBullet.Instance();
        this._bulletContainer.AddChild(bullet);

        bullet.Position = new Vector2(this._muzzle.GlobalPosition);
    }

    public void Damage(int damage)
    {
        this._health -= damage;

        Level1.HUD.SetHealth(((float) this._health) / this.MaxHealth);

        if (this._health > this.BrokenSpriteHealth)
            this._animatedSprite.Animation = "Healthy";
        else if (this._health <= 0)
        {
            this._animatedSprite.Animation = "Dead";
            
            // TODO : Add a delay or something
            GetTree().ChangeScene("res://scenes/GameOver.tscn");
        }
        else
            this._animatedSprite.Animation = "Damage";
    }
}