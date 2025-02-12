using Godot;
using System;

public partial class Begining : Node3D
{
	AnimationPlayer _animation;
	Camera3D _cutSceneCamera;
	Camera3D _basicCamera;
	Area3D _bakeArea;
	Sprite2D _pressESprite;
	bool _playerInArea = false;
	Node3D _fireParticles;
	Polygon2D _dialogue;
	
	public override void _Ready()
	{
		_animation = GetNode<AnimationPlayer>("AnimationPlayer");
		if (!GameManager.Instance.IsBeginingCutSceneSeen) {
			_animation.Play("BeginingCutScene");
		} else {
			GetNode<Polygon2D>("Car/Dialogue").Hide();
		}
		
		_cutSceneCamera = GetNode<Camera3D>("Car/CutSceneCamera");
		_cutSceneCamera.MakeCurrent();
		
		_basicCamera = GetNode<Camera3D>("Player/CharacterBody/Head/Camera3D");
		_bakeArea = GetNode<Area3D>("MainHome/BakeArea");
		_pressESprite = GetNode<Sprite2D>("Player/CharacterBody/PressESprite");
		_fireParticles = GetNode<Node3D>("house/FireParticles");
		_dialogue = GetNode<Polygon2D>("Car/Dialogue");
		
		_bakeArea.Connect("body_entered", new Callable(this, nameof(OnBakeAreaEntered)));
		_bakeArea.Connect("body_exited", new Callable(this, nameof(OnBakeAreaExited)));
	}

	public override void _Process(double delta)
	{
		if (!_animation.IsPlaying()) {
			_basicCamera.MakeCurrent();
			GameManager.Instance.IsBeginingCutSceneSeen = true;
		}
		if (_playerInArea) {
			ShowFire();
		}
		if (_animation.IsPlaying() && Input.IsActionPressed("skip")) {
			_animation.Stop();
			_dialogue.Hide();
		}
	}
	
	public void OnBakeAreaEntered(Node body) {
		if (body.IsInGroup("Player")) {
			_pressESprite.Visible = true;
			_playerInArea = true;
		}
	}
	public void OnBakeAreaExited(Node body) {
		if (body.IsInGroup("Player")) {
			_pressESprite.Visible = false;
			_playerInArea = false;
		}
	}
	private void ShowFire() {
		if (Input.IsActionJustPressed("take_item")) {
			_fireParticles.Show();
			GD.Print("Игрок зажёг печь.");
			_pressESprite.Visible = false;
			_bakeArea.QueueFree();
		}
	}
}
