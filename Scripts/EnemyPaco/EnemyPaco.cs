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

    [Signal]
    public delegate void ShootBullet(EnemyPacoProjectile instance, int projectileSpeed);
    
    private Timer _shootTimer;

    private bool _isDerp;
    
    private static readonly Random _random = new Random();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _shootTimer = GetNode<Timer>("ShootTimer");
        SetShootTimerDuration();

        _isDerp = _random.Next(0, 10) == 0;
    }
    
    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }

    public void OnShootTimerTimeout()
    {
        var projectileInstance = (EnemyPacoProjectile) Projectile.Instance();
        EmitSignal("ShootBullet", projectileInstance, ProjectileSpeed);
        
        SetShootTimerDuration();
    }

    private void SetShootTimerDuration()
    {
        _shootTimer.Stop();
        var duration = (float)(_random.NextDouble() * (ProjectileMaxDelay - ProjectileMinDelay)) + ProjectileMinDelay;

        _shootTimer.WaitTime = duration;
        _shootTimer.Start();
    }
}
