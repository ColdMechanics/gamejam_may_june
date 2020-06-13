using Godot;
using System;

public class HUD : CanvasLayer
{
	private Label _scoreLabel;

	private HealthBar _healthBar;

	private uint _score = 0;
	
	[Export]
	public int ScoreLength = 6;
	
	[Signal]
	public delegate void StartGame();

	public override void _Ready()
	{
		this._scoreLabel = GetNode<Label>("Score");
		this._healthBar = GetNode<HealthBar>("HealthBar");
		
		this._healthBar.SetValue(1);
	}

	public void OnStartGame()
	{
		this._score = 0;
	}

	public void AddScore(uint points)
	{
		this._score += points;
		
		this._scoreLabel.Text = this._score.ToString().PadLeft(this.ScoreLength, '0');
	}

	public void SetHealth(float value)
	{
		this._healthBar.SetValue(value);
	}
}
