using Godot;
using System;

public class Cloud : Node2D
{
    [Export]
    public int Speed1 = 100;

    [Export]
    public int Speed2 = 200;
    
    private Sprite _cloudSprite;

    private int _speed;
    
    private static readonly Random _random = new Random();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var spriteFrame = _random.Next(0, 16);
        _speed = _random.Next(2) == 1 ? Speed1 : Speed2;
        
        _cloudSprite = GetNode<Sprite>("CloudSprite");
        _cloudSprite.Frame = spriteFrame;

        var initialX = GetViewport().Size.x;
        var initialY = _random.Next(0, (int)GetViewport().Size.y);
        
        GlobalPosition = new Vector2(initialX, initialY);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var xPos = GlobalPosition.x - (_speed * delta);
        GlobalPosition = new Vector2(xPos, GlobalPosition.y);
    }

    public void OnVisibilityNotifier2DScreenExited()
    {
        QueueFree();
    }
}
