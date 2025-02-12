using Godot;
using System;

public partial class RefuelingArea : Area3D
{
	private bool _bodyInArea = false;
	private Node3D _player;
	private Sprite2D _pressESprite;
	private Area3D _carEnterArea;
	
	public override void _Ready()
	{
		_carEnterArea = GetParent().GetNode<Area3D>("CarEnterArea");
		_player = GameManager.Instance.Player;
		_pressESprite = _player.GetNode<Sprite2D>("CharacterBody/PressESprite");
		Connect("body_entered", new Callable(this, nameof(OnRefuelingAreaEntered)));
		Connect("body_exited", new Callable(this, nameof(OnRefuelingAreaExited)));
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_bodyInArea && Input.IsActionJustPressed("take_item")) {
			if (petrolInHand()) {
				_carEnterArea.Monitoring = true;
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
			
			_bodyInArea = true;
		}
	}
	private void OnRefuelingAreaExited(Node3D body) {
		if (body.IsInGroup("Player")) {
			ChangePressESpriteVisibility();
			
			_bodyInArea = false;
		}
	}
	private void ChangePressESpriteVisibility() {
		_pressESprite.Visible = !_pressESprite.Visible;
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
