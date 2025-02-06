using Godot;
using System;

public partial class CatScenes : AnimationPlayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Camera3D cameraHappyEnd = GetParent().GetNode<Camera3D>("car3/CameraHappyEnd");
		Camera3D cameraBadEnd = GetParent().GetNode<Camera3D>("Armature/PlayerBody/CameraBadEnd");
		if (GameManager.Instance.IsEndHappy) {
			cameraHappyEnd.MakeCurrent();
			Play("HappyEnd");
		} else {
			cameraBadEnd.MakeCurrent();
			Play("BadEnd");
		}
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!IsPlaying()) {
			Input.SetMouseMode(Input.MouseModeEnum.Visible);
			GameManager.Instance.DownloadableScene = "res://scenes/ui/menu.tscn";
			PackedScene nextScene = (PackedScene)ResourceLoader.Load("res://scenes/loading_scene.tscn");
			GetTree().ChangeSceneToPacked(nextScene);
		}
	}
}
