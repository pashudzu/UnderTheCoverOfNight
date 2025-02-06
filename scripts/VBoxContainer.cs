using Godot;
using System;

public partial class VBoxContainer : Godot.VBoxContainer {
	public override void _Ready() {
		Button resume = GetNodeOrNull<Button>("Resume");
		Button backToMenu = GetNodeOrNull<Button>("BackToMenu");
		Button quit = GetNodeOrNull<Button>("Quit");
		
		resume.Connect("pressed", Callable.From(OnRusumeButtonPressed));
		backToMenu.Connect("pressed", Callable.From(OnBackToMenuPressed));
		quit.Connect("pressed", Callable.From(OnQuitPresssed));
	}
	
	public override void _Input(InputEvent @event) {
		if (Input.IsActionJustPressed("pause")) {
			Resume();
		}
	}
	
	public void OnRusumeButtonPressed() {
		Resume();
	}
	public void OnBackToMenuPressed() {
		GetTree().ChangeSceneToFile("res://scenes/ui/menu.tscn");
		Input.SetMouseMode(Input.MouseModeEnum.Visible);
	}
	public void OnQuitPresssed() {
		GetTree().Quit();
	}
	private void Resume() {
		Engine.TimeScale = 1;
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
	}
}
