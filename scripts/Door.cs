using Godot;
using System;

public partial class Door : Node3D
{
	private AnimationPlayer _animation;
	private Area3D _area;
	private Sprite2D _pressESprite;
	private Node3D _player;
	private bool _isInArea = false;
	private bool _isDoorOpened = false;
	
	public override void _Ready() {
		_player = GameManager.Instance.Player;
		_animation = GetNode<AnimationPlayer>("AnimationPlayer");
		_area = GetNode<Area3D>("Area3D");
		_pressESprite = _player.GetNode<Sprite2D>("CharacterBody/PressESprite");
		_area.BodyEntered += OnBodyEntered;
		_area.BodyExited += OnBodyEntered;
	}
	public override void _Process (double delta) {
		if (_isInArea && Input.IsActionJustPressed("take_item")) {
			InteractionDoor();
		}
	}
	private void InteractionDoor() {
		if (_isDoorOpened) {
			_animation.Play("close_door");
			_isDoorOpened = false;
		} else {
			_animation.Play("open_door");
			_isDoorOpened = true;
		}
	}
	public void OnBodyEntered(Node body) {
		if (body.IsInGroup("Player")) {
			_isInArea = true;
			_pressESprite.Visible = true;
		}
	}
	public void OnBodyExited(Node body) {
		if (body.IsInGroup("Player")) {
			_isInArea = false;
			_pressESprite.Visible = false;
		}
	}
}
