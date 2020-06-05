using Godot;
using System;

public class Menu : Node2D
{
    [Signal] public delegate void Unpause();
    
    [Signal] public delegate void Quit();
   
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Hide();
    }

    public void OnResumeButtonPressed()
    {
        EmitSignal("Unpause");
        Hide();
    }

    public void OnQuitButtonPressed()
    {
        EmitSignal("Quit");
    }
}
