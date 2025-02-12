using Godot;
using System;

public partial class EventAnimationPlayer : AnimationPlayer
{
	private Camera3D _mainCamera;
	private Camera3D _awakeningCamera;
	
	public override void _Ready()
	{
		Play("Awakening");
		_awakeningCamera = GetNode<Camera3D>("Camera3D");
		_mainCamera = GameManager.Instance.Player.GetNode<Camera3D>("CharacterBody/Head/Camera3D");
	}
	public override void _Process(double delta) {
		if (IsPlaying()) {
			GameManager.Instance.IsEventAnimationIsOngoing = true;
			_awakeningCamera.MakeCurrent();
		} else {
			GameManager.Instance.IsEventAnimationIsOngoing = false;
			_mainCamera.MakeCurrent();
		}
	}
}
