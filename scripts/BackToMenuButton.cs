using Godot;
using System;

public partial class BackToMenuButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("pressed", new Callable(this, nameof(OnPressed)));
	}

	private void OnPressed() {
		GetTree().ChangeSceneToFile("res://scenes/ui/menu.tscn");
	}
}
