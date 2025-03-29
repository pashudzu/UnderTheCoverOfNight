using Godot;
using System;
using System.Threading.Tasks;

public partial class LieingArea : Area3D
{
	private AnimationPlayer _animationPlayer;
	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnAreaEntered)));
		_animationPlayer = GetParent().GetNode<AnimationPlayer>("AnimationPlayer");
	}
	
	private async void OnAreaEntered(Node body) {
		if (body.IsInGroup("Player")) {
			GameManager.Instance.IsWentSleep = true;
			_animationPlayer.Play("LieingDown");
			await Task.Delay(5000);
			GameManager.Instance.DownloadableScene = "res://scenes/cutScene.tscn";
			PackedScene loadindScene = (PackedScene)ResourceLoader.Load("res://scenes/home_scene.tscn");
			GetTree().ChangeSceneToPacked(loadindScene);
			QueueFree();
		}
	}
}
