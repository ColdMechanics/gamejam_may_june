using Godot;
using Godot.Collections;

public class Beamer : EnnemyBase
{
    private Line2D _laser;
    private Position2D _muzzle;
    private AnimatedSprite _animatedSprite;
    
    private float _beamLength = 0f;
    private bool _canFire = true;
    private bool _isFirring = false;
    private bool _isMoving = false;

    [Export(PropertyHint.Range, "0,6,.5")]
    public float BeamDuration = 2f;
    
    [Export(PropertyHint.Range, "0,6,.1")]
    public float BeamChargeTime = 2f;
    
    [Export(PropertyHint.Range, "100,1000,5")]
    public float BeamLength = 200f;

    public override void _Ready()
    {
        this._laser = GetNode<Line2D>("Line2D");
        this._muzzle = GetNode<Position2D>("Position2D");
        this._animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        
        // remove fom the image the last point
        // this._laser.RemovePoint(1);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (this._isMoving && this.Speed > 0)
        {
            this.Position = new Vector2(
                x: this.Position.x + this.Speed * delta * -1,
                y: this.Position.y
            );
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        
        if(this._isFirring)
        {
            this._beamLength += delta * (this.BeamLength / this.BeamDuration);

            Vector2 endPosition = new Vector2(this._muzzle.GlobalPosition);
            endPosition.x += this._beamLength;
        
            this._laser.SetPointPosition(1, endPosition);
        
            // TODO test if touch
        }
        else if(this._canFire)
        {
            // Test if raycast collide with player
            RayTouchPlayer(1000);
        }
    }

    private async void Shoot()
    {
        if(!this._canFire) return;

        this._canFire = false;
        
        // Stop moving
        this._isMoving = false;
        
        this._animatedSprite.Animation = "Prepare";
        // Play sound
        // Wait for charging
        GetTree().CreateTimer(this.BeamChargeTime);
        this._animatedSprite.Animation = "Fire";
        StartLaser();
        // Wait fire time
        GetTree().CreateTimer(this.BeamDuration);
        StopLaser();
        this._animatedSprite.Animation = "Normal";
        
        // Start moving
        this._isMoving = true;
    }

    private void StartLaser()
    {
        // Play Firring sound
        this._isFirring = true;
    }
    
    private void StopLaser()
    {
        // Play Firring sound
        this._isFirring = false;
        this._laser.SetProcess(false);
    }

    private bool RayTouchPlayer(float length)
    {
        Physics2DDirectSpaceState spaceState = GetWorld2d().DirectSpaceState;
        Dictionary touched = spaceState.IntersectRay(this._muzzle.GlobalPosition, this._muzzle.GlobalPosition + this.Transform.x * length * -1);

        this._laser.SetPointPosition(0, this._muzzle.Position);
        this._laser.SetPointPosition(1, this._muzzle.Position + this.Transform.x * length * -1);
        
        if (touched.Count > 0)
        {
            GD.Print("Raycast hit at point: ", touched["position"]);
        }

        return false;
    }
    
    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }
    
    public void OnVisibilityNotifier2DScreenEnter()
    {
        this._canFire = true;
    }
}