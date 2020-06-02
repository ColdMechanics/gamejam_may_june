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
    
    private float _maxX;
    private float _maxY;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Vector2 screenSize = GetViewport().GetVisibleRect().Size;

        this._maxX = screenSize.x - this.XBorder;
        this._maxY = screenSize.y - this.YBorder;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Vector2 velocity = new Vector2(); // The player's movement vector.

        if (Input.IsActionJustPressed("player_shoot"))
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

            this.Position += velocity;
            this.Position = new Vector2(
                x: Mathf.Clamp(this.Position.x, this.XBorder, this._maxX),
                y: Mathf.Clamp(this.Position.y, this.YBorder, this._maxY)
            );
        }
    }
    
    public void OnPlayerBodyEntered(PhysicsBody2D body)
    {
        EmitSignal("Hit");
    }

    private void Shoot()
    {
        GD.Print("Shoot!");
    }
}