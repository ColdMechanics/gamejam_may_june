using Godot;

public class TitleScreen : Control
{
    [Export] 
    public PackedScene LevelToLoad;

    public void OnNewGameButtonPressed()
    {
        GetTree().ChangeSceneTo(LevelToLoad);
    }

    public void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}
