using Godot;
using System;

public partial class EventAnimationPlayer : AnimationPlayer
{
	Camera3D mainCamera;
	Camera3D awakeningCamera;
	
	public override void _Ready()
	{
		Play("Awakening");
		awakeningCamera = GetNode<Camera3D>("Camera3D");
		mainCamera = GameManager.Instance.Player.GetNode<Camera3D>("CharacterBody/Head/Camera3D");
	}
	public override void _Process(double delta) {
		if (IsPlaying()) {
			GameManager.Instance.IsEventAnimationIsOngoing = true;
			awakeningCamera.MakeCurrent();
		} else {
			GameManager.Instance.IsEventAnimationIsOngoing = false;
			mainCamera.MakeCurrent();
		}
	}
}
