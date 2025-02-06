using Godot;
using System;

public partial class PauseMenu : Control {
	
	public override void _Ready() {
		Button resume = GetNodeOrNull<Button>("VBoxContainer/Resume");
		Button backToMenu = GetNodeOrNull<Button>("VBoxContainer/BackToMenu");
		Button quit = GetNodeOrNull<Button>("VBoxContainer/Quit");
		
		resume.Connect("pressed", Callable.From(OnRusumeButtonPressed));
		backToMenu.Connect("pressed", Callable.From(OnBackToMenuPressed));
		quit.Connect("pressed", Callable.From(OnQuitPresssed));
	}
	
	public override void _Process(double delta) {
	}
	
	public void OnRusumeButtonPressed() {
		
	}
	public void OnBackToMenuPressed() {
		GetTree().ChangeSceneToFile("res://scenes/menu.tscn");
	}
	public void OnQuitPresssed() {
		GetTree().Quit();
	}
}
