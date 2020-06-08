using Godot;

public class Beamer : EnnemyBase
{
    private Line2D _laser;
    private Position2D _muzzle;
    private AnimatedSprite _animatedSprite;
    private RayCast2D _rayCast;

    private float _beamLength = 0f;
    private bool _canFire = false;
    private bool _isFirring = false;
    private bool _isMoving = true;

    [Export(PropertyHint.Range, "0,6,.5")]
    public float BeamDuration = 2f;

    [Export(PropertyHint.Range, "0,6,.1")]
    public float BeamChargeTime = 2f;

    [Export(PropertyHint.Range, "100,10000,5")]
    public float BeamLength = 200f;

    public override void _Ready()
    {
        this._laser = GetNode<Line2D>("Line2D");
        this._muzzle = GetNode<Position2D>("Position2D");
        this._animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        this._rayCast = GetNode<RayCast2D>("RayCast2D");

        // remove fom the image the last point
        this._laser.RemovePoint(1);
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

        if (this._isFirring)
        {
            this._beamLength += delta * (this.BeamLength / this.BeamDuration);

            Vector2 endPosition = new Vector2(this._laser.Points[0]);
            endPosition.x -= this._beamLength;

            this._laser.SetPointPosition(1, endPosition);

            if (RayTouchPlayer(this._beamLength))
            {
                // TODO damage the player
                StopLaser();
            }
        }
        else if (this._canFire)
        {
            // Test if raycast collide with player
            if (RayTouchPlayer(1000))
            {
                Shoot();
            }
        }
    }

    private async void Shoot()
    {
        if (!this._canFire) return;

        this._canFire = false;

        // Stop moving
        this._isMoving = false;

        this._animatedSprite.Animation = "Prepare";
        // Play sound
        // Wait for charging
        await ToSignal(GetTree().CreateTimer(this.BeamChargeTime), "timeout");
        this._animatedSprite.Animation = "Fire";
        StartLaser();
        // Wait fire time
        await ToSignal(GetTree().CreateTimer(this.BeamDuration), "timeout");
        StopLaser();
        this._animatedSprite.Animation = "Normal";

        // Start moving
        this._isMoving = true;
    }

    private void StartLaser()
    {
        // Play Firring sound

        this._beamLength = 0;
        this._laser.AddPoint(this._muzzle.Position);
        this._isFirring = true;
    }

    private void StopLaser()
    {
        // Play Firring sound
        this._isFirring = false;
        this._laser.RemovePoint(1);
    }

    private bool RayTouchPlayer(float length)
    {
        this._rayCast.Enabled = true;
        this._rayCast.Position = this._muzzle.Position;

        Vector2 endPosition = new Vector2(this._muzzle.Position);
        endPosition.x -= length;

        this._rayCast.CastTo = endPosition;

        this._laser.SetPointPosition(0, this._muzzle.Position);
        this._laser.SetPointPosition(1, endPosition);

        if (!this._rayCast.IsColliding()) return false;

        Node collision = (Node) this._rayCast.GetCollider();

        return !(collision is null) && collision.IsInGroup("Player");
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