using Godot;
using System;

public partial class NewGameButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("pressed", Callable.From(OnNewGameButtonPressed));
	}
	
	public void OnNewGameButtonPressed() {
		GameManager.Instance.DownloadableScene = "res://scenes/begining.tscn";
		PackedScene _laodScene = (PackedScene)ResourceLoader.Load("res://scenes/loading_scene.tscn");
		GetTree().ChangeSceneToPacked(_laodScene);
	}
}
