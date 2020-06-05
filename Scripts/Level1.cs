using Godot;
using System;

public class Level1 : Node2D
{
    private Menu _menu;

    private bool isCancelPressed = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _menu = GetNode<Menu>("Menu");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("ui_cancel") && !isCancelPressed)
        {
            isCancelPressed = true;
            MenuToggle();
        }
        else if (!Input.IsActionPressed("ui_cancel"))
        {
            isCancelPressed = false;
        }
    }

    private void MenuToggle()
    {
        if (GetTree().Paused)
        {
            OnMenuUnpause();
        }
        else
        {
            _menu.Show();
            GetTree().Paused = true;
        }
    }

    public void OnMenuUnpause()
    {
        _menu.Hide();
        GetTree().Paused = false;
    }

    public void OnMenuQuit()
    {
        GetTree().Quit();
    }
}
