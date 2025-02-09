using Godot;
using System;

public partial class CarEnterArea : Area3D
{
	private bool playerInArea;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnCarEnterAreaEntered)));
		Connect("body_exited", new Callable(this, nameof(OnCarEnterAreaExited)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (playerInArea && Input.IsActionJustPressed("take_item")) {
			if (GameManager.Instance.CarIsFueled) {
				GameManager.Instance.DownloadableScene = "res://scenes/cutScene.tscn";
				PackedScene loadScene = (PackedScene)ResourceLoader.Load("res://scenes/loading_scene.tscn");
				GetTree().ChangeSceneToPacked(loadScene);
			}
		}
	}
	private void OnCarEnterAreaEntered(Node body) {
		playerInArea = true;
	}
	private void OnCarEnterAreaExited(Node body) {
		playerInArea = false;
	}
}
