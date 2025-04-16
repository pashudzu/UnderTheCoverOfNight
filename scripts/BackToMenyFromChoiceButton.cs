using Godot;
using System;

public partial class BackToMenyFromChoiceButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("pressed", Callable.From(OnButtonPressed));
	}

	private void OnButtonPressed() {
		PackedScene _menuScene = (PackedScene)ResourceLoader.Load(ProjectSettings.GlobalizePath("res://scenes/ui/menu.tscn"));
		GetTree().ChangeSceneToPacked(_menuScene);
	}
}
