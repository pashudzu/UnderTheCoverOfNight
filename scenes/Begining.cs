using Godot;
using System;

public partial class Begining : Node3D
{
	AnimationPlayer animation;
	Camera3D cutSceneCamera;
	Camera3D basicCamera;
	Area3D bakeArea;
	Sprite2D pressESprite;
	bool playerInArea = false;
	Node3D fireParticles;
	Polygon2D dialogue;
	
	public override void _Ready()
	{
		cutSceneCamera = GetNode<Camera3D>("Car/CutSceneCamera");
		cutSceneCamera.MakeCurrent();
		animation = GetNode<AnimationPlayer>("AnimationPlayer");
		animation.Play("BeginingCutScene");
		basicCamera = GetNode<Camera3D>("Player/CharacterBody/Head/Camera3D");
		bakeArea = GetNode<Area3D>("MainHome/BakeArea");
		pressESprite = GetNode<Sprite2D>("Player/CharacterBody/PressESprite");
		fireParticles = GetNode<Node3D>("house/FireParticles");
		dialogue = GetNode<Polygon2D>("Car/Dialogue");
		bakeArea.Connect("body_entered", new Callable(this, nameof(OnBakeAreaEntered)));
		bakeArea.Connect("body_exited", new Callable(this, nameof(OnBakeAreaExited)));
	}

	public override void _Process(double delta)
	{
		if (animation != null && !animation.IsPlaying()) {
			basicCamera.MakeCurrent();
		}
		if (playerInArea) {
			ShowFire();
		}
		if (animation.IsPlaying() && Input.IsActionPressed("skip")) {
			animation.Stop();
			dialogue.Hide();
		}
	}
	public void OnBakeAreaEntered(Node body) {
		if (body.IsInGroup("Player")) {
			ChangePressESpriteVisibility();
			playerInArea = true;
		}
	}
	public void OnBakeAreaExited(Node body) {
		if (body.IsInGroup("Player")) {
			ChangePressESpriteVisibility();
			playerInArea = false;
		}
	}
	private void ShowFire() {
		if (Input.IsActionJustPressed("take_item")) {
			fireParticles.Show();
			GD.Print("Игрок зажёг печь.");
			ChangePressESpriteVisibility();
			bakeArea.QueueFree();
		}
	}
	private void ChangePressESpriteVisibility() {
		pressESprite.Visible = !pressESprite.Visible;
	}
}
