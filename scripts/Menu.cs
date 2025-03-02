using Godot;
using System;

public partial class Menu : CanvasLayer
{
	public override void _Ready()
	{
		TextureButton play = GetNodeOrNull<TextureButton>("VBoxContainer/Play");
		TextureButton settingButton = GetNodeOrNull<TextureButton>("VBoxContainer/Settings");
		TextureButton exitButton = GetNodeOrNull<TextureButton>("VBoxContainer/Exit");
		
		play.Connect("pressed", Callable.From(OnPlayButtonPressed));
		settingButton.Connect("pressed", Callable.From(OnSettingButtonPressed));
		exitButton.Connect("pressed", Callable.From(OnExitButtonPressed));
	}
	
	public void OnPlayButtonPressed() {
		GetTree().ChangeSceneToFile("res://scenes/saveChoice.tscn");
	}
	
	public void OnSettingButtonPressed() {
		GetTree().ChangeSceneToFile("res://scenes/ui/settings.tscn");
	}
	
	public void OnExitButtonPressed() {
		GetTree().Quit();
	}
}
