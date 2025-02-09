using Godot;
using System;

public partial class Menu : CanvasLayer
{
	public override void _Ready()
	{
		Button newGameButton = GetNodeOrNull<Button>("VBoxContainer/NewGame");
		Button settingButton = GetNodeOrNull<Button>("VBoxContainer/Settings");
		Button exitButton = GetNodeOrNull<Button>("VBoxContainer/Exit");
		Button continueGameButton = GetNodeOrNull<Button>("VBoxContainer/ContinueGame");
		
		newGameButton.Connect("pressed", Callable.From(OnNewGameButtonPressed));
		settingButton.Connect("pressed", Callable.From(OnSettingButtonPressed));
		exitButton.Connect("pressed", Callable.From(OnExitButtonPressed));
		continueGameButton.Connect("pressed", Callable.From(OnContinueGameButtonPressed));
	}
	
	public void OnNewGameButtonPressed() {
		GameManager.Instance.DownloadableScene = "res://scenes/begining.tscn";
		PackedScene loadScene = (PackedScene)ResourceLoader.Load("res://scenes/loading_scene.tscn");
		GetTree().ChangeSceneToPacked(loadScene);
	}
	
	public void OnSettingButtonPressed() {
		GetTree().ChangeSceneToFile("res://scenes/ui/settings.tscn");
	}
	
	public void OnExitButtonPressed() {
		GetTree().Quit();
	}
	
	public void OnContinueGameButtonPressed() {
		GetTree().ChangeSceneToFile("res://scenes/saveChoice.tscn");
	}
}
