using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;

public class Beamer : EnnemyBase
{
    private static readonly Color PartialColor = new Color(1f, 1f, 1f, 0.5f);
    private static readonly Color NormalColor = new Color(1f, 1f, 1f, 1f);
    
    private Line2D _laser;
    private Position2D _muzzle;
    private AnimatedSprite _animatedSprite;
    private RayCast2D _rayCast;
    private AudioStreamPlayer _laserSound;
    private AudioStreamPlayer _deathSound;

    private float _beamLength = 0f;
    private bool _canFire = false;
    private bool _isFirring = false;
    private bool _isMoving = true;
    private bool _isDead = false;

    private CancellationTokenSource _fireSequenceToken;

    [Export(PropertyHint.Range, "0,6,.1")]
    public float BeamDuration = 2f;

    [Export(PropertyHint.Range, "0,6,.1")]
    public float BeamChargeTime = 2f;

    [Export(PropertyHint.Range, "100,10000,5")]
    public float BeamLength = 200f;

    [Export]
    public int BeamDamage = 2;

    public override void _Ready()
    {
        this._laser = GetNode<Line2D>("Line2D");
        this._muzzle = GetNode<Position2D>("Position2D");
        this._animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
        this._rayCast = GetNode<RayCast2D>("RayCast2D");
        this._laserSound = GetNode<AudioStreamPlayer>("Laser");
        this._deathSound = GetNode<AudioStreamPlayer>("Death");

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

            if(this._laser.Points.Length > 1)
                this._laser.SetPointPosition(1, endPosition);

            if (RayTouchPlayer(this._beamLength))
            {
                //Damage the player
                Level1.Player.Damage(this.BeamDamage);

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
        
        if(this._isDead) return;

        this._canFire = false;

        // Stop moving
        this._isMoving = false;

        this._laserSound.Play();

        this._animatedSprite.Animation = "Prepare";

        this._fireSequenceToken?.Cancel();
        this._fireSequenceToken = new CancellationTokenSource();

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(this.BeamChargeTime), this._fireSequenceToken.Token);
            if (this._isDead) return;
            this._animatedSprite.Animation = "Fire";
            StartLaser();
            // Wait fire time
            await Task.Delay(TimeSpan.FromSeconds(this.BeamChargeTime), this._fireSequenceToken.Token);
            if (this._isDead) return;
            StopLaser();
            this._animatedSprite.Animation = "Normal";

            // Start moving
            this._isMoving = true;
        }
        catch (OperationCanceledException e)
        {
            // ignore cancel token exception
        }
    }

    private void StartLaser()
    {
        if(this._isFirring) return;
        
        this._beamLength = 0;
        this._laser.AddPoint(this._muzzle.Position);
        this._isFirring = true;
    }

    private void StopLaser()
    {
        if(!this._isFirring) return;
        
        this._isFirring = false;
        this._laser.RemovePoint(1);
    }

    private bool RayTouchPlayer(float length)
    {
        this._rayCast.Enabled = true;
        this._rayCast.Position = this._muzzle.Position;

        Vector2 endPosition = new Vector2(this._muzzle.Position) {x = -length};

        this._rayCast.CastTo = endPosition;

        if (!this._rayCast.IsColliding()) return false;

        Node collision = (Node) this._rayCast.GetCollider();

        // To refresh the RayCast
        this._rayCast.Enabled = false;

        return !(collision is null) && collision.IsInGroup("Player");
    }

    private void DisableCollision()
    {
        GetNode<CollisionPolygon2D>("CollisionPolygon2D").Disabled = true;

    }

    private async void Blink()
    {
        for (int i = 0; i < 4; ++i)
        {
            if(this._isDead) return;
            
            this._animatedSprite.Modulate = PartialColor;
            await ToSignal(GetTree(), "idle_frame");
            this._animatedSprite.Modulate = NormalColor;
            await ToSignal(GetTree(), "idle_frame");
        }
    }

    public override async void Die()
    {
        if(this._isDead) return;

        CallDeferred("DisableCollision");
        this._isDead = true;
        this._fireSequenceToken?.Cancel();
        this._laserSound.Stop();
        StopLaser();
        this._animatedSprite.Animation = "Dead";
        this._deathSound.Play();
        this._isMoving = false;
        await Task.Delay(TimeSpan.FromSeconds(this._deathSound.Stream.GetLength()), CancellationToken.None);
        QueueFree();
    }

    public override void Hit(int damage)
    {
        this.Life -= damage;

        if (this.Life <= 0)
            Die();
        else
            Blink();
        
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