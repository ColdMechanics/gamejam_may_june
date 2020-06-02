using Godot;
using System;

public class HUD : CanvasLayer
{
    private Label _scoreLabel;

    private uint _score = 0;
    
    [Export]
    public int ScoreLength = 6;
    
    [Signal]
    public delegate void StartGame();

    [Signal]
    public delegate void UpdateScore();
    
    public override void _Ready()
    {
        this._scoreLabel = GetNode<Label>("Score");
    }

    public void OnStartGame()
    {
        this._score = 0;
    }

    public void OnUpdateScore(uint points)
    {
        this._score += points;
        
        this._scoreLabel.Text = this._score.ToString().PadLeft(this.ScoreLength, '0');
    }
}
