using Godot;
using System;

public class HUD : CanvasLayer
{
	private Label _scoreLabel;

	private HealthBar _healthBar;

	private PlayerVariables _playerVariables;
	
	[Export]
	public int ScoreLength = 6;
	
	[Signal]
	public delegate void StartGame();

	public override void _Ready()
	{
		this._scoreLabel = GetNode<Label>("Score");
		this._healthBar = GetNode<HealthBar>("HealthBar");
		
		this._healthBar.SetValue(1);

		this._playerVariables = GetNode<PlayerVariables>("/root/PlayerVariables");
	}

	public void OnStartGame()
	{
		this._playerVariables.Score = 0;
	}

	public void AddScore(uint points)
	{
		this._playerVariables.Score += points;
		
		this._scoreLabel.Text = this._playerVariables.Score.ToString().PadLeft(this.ScoreLength, '0');
	}

	public void SetHealth(float value)
	{
		this._healthBar.SetValue(value);
	}
}
