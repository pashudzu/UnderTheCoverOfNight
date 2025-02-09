using Godot;
using System;

public partial class RefuelingArea : Area3D
{
	private Area3D carEnterArea;
	private bool bodyInArea = false;
	private Node3D player;
	private Sprite2D pressESprite;
	
	public override void _Ready()
	{
		carEnterArea = GetParent().GetNode<Area3D>("CarEnterArea");
		player = GameManager.Instance.Player;
		pressESprite = player.GetNode<Sprite2D>("CharacterBody/PressESprite");
		Connect("body_entered", new Callable(this, nameof(OnRefuelingAreaEntered)));
		Connect("body_exited", new Callable(this, nameof(OnRefuelingAreaExited)));
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (bodyInArea && Input.IsActionJustPressed("take_item")) {
			if (petrolInHand()) {
				carEnterArea.Monitoring = true;
				GD.Print("Машина заправлена.");
				GameManager.Instance.CarIsFueled = true;
				QueueFree();
			} else {
				GD.Print("У игрока нет бензина в руке, чтобы заправить машину.");
			}
		}
	}
	
	private void OnRefuelingAreaEntered(Node3D body) {
		if (body.IsInGroup("Player")) {
			ChangePressESpriteVisibility();
			
			bodyInArea = true;
		}
	}
	private void OnRefuelingAreaExited(Node3D body) {
		if (body.IsInGroup("Player")) {
			ChangePressESpriteVisibility();
			
			bodyInArea = false;
		}
	}
	private void ChangePressESpriteVisibility() {
		pressESprite.Visible = !pressESprite.Visible;
	}
	private bool petrolInHand () {
		string leftHandChild = GameManager.Instance.LeftHandChild;
		string rightHandChild = GameManager.Instance.RightHandChild;
		if (leftHandChild == "Бензин" || rightHandChild == "Бензин") {
			return true;
		} else {
			return false;
		}
	}
}
