using Godot;
using System;

public class GameOver : Control
{
    [Export]
    public int ScoreLength = 6;
    
    [Export] 
    public PackedScene LevelToLoad;
    
    private PlayerVariables _playerVariables;

    private Label _scoreLabel;
        
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this._playerVariables = GetNode<PlayerVariables>("/root/PlayerVariables");
        this._scoreLabel = GetNode<Label>("ScoreLabel");
        
        this._scoreLabel.Text = this._playerVariables.Score.ToString().PadLeft(this.ScoreLength, '0');
    }

    public void OnRestartBtnPressed()
    {
        GetTree().ChangeSceneTo(LevelToLoad);
    }

    public void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}
