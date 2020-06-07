using System;
using Godot;

public class EnemyPaco : RigidBody2D
{
    [Export]
    public PackedScene Projectile;
    
    [Export]
    public int Speed = 200;

    [Export]
    public int ProjectileSpeed = 400;

    [Export]
    public float ProjectileMinDelay = 1;

    [Export]
    public float ProjectileMaxDelay = 10;

    [Export]
    public int DerpChance = 10;

    [Signal]
    public delegate void ShootBullet(EnemyPacoProjectile instance, int projectileSpeed, bool isDerp);

    private AudioStreamPlayer _shootSound;
    
    private AudioStreamPlayer _deathSound;

    private AnimatedSprite _animation;
    
    private Timer _shootTimer;

    private bool _isDerp;
    
    private static readonly Random _random = new Random();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _animation = GetNode<AnimatedSprite>("Animation");
        _deathSound = GetNode<AudioStreamPlayer>("DeathSound");
        _shootSound = GetNode<AudioStreamPlayer>("ShootSound");
        _shootTimer = GetNode<Timer>("ShootTimer");
        SetShootTimerDuration();

        _isDerp = _random.Next(0, 100) < DerpChance;
    }
    
    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }

    public void OnShootTimerTimeout()
    {
        var projectileInstance = (EnemyPacoProjectile) Projectile.Instance();
        EmitSignal("ShootBullet", projectileInstance, ProjectileSpeed, _isDerp);
        _shootSound.Play();
        
        SetShootTimerDuration();
    }

    public void OnEnemyPacoBodyEntered()
    {
        _shootTimer.Stop();
        _animation.Play("Death");
        _deathSound.Play();
    }

    public void OnAnimatedSpriteAnimationFinished()
    {
        if (_animation.Animation == "Death")
        {
            QueueFree();
        }
    }

    private void SetShootTimerDuration()
    {
        _shootTimer.Stop();
        var duration = (float)(_random.NextDouble() * (ProjectileMaxDelay - ProjectileMinDelay)) + ProjectileMinDelay;

        _shootTimer.WaitTime = duration;
        _shootTimer.Start();
    }
}
