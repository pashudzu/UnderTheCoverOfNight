using Godot;
using System;

public partial class Area3d : Area3D
{
	private Node3D _player;
	private Sprite2D _pressESprite;
	private Polygon2D _dialogue;
	private AnimationPlayer _textAnimation;
	private Label _label;
	private bool _bodyInRange = false;
	private bool _IsDialogueOngoing = false;
	private string[] _pages = new string[4];
	private int _openPage = 0;
	private Area3D _petrol;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (GameManager.Instance.Player == null) {
			return;
		}
		_petrol = GetParent().GetParent().GetNode<Area3D>("Petrol");
		_player = GameManager.Instance.Player;
		_label = _player.GetNode<Label>("CharacterBody/Dialogue/Label");
		_textAnimation = _player.GetNode<AnimationPlayer>("CharacterBody/Dialogue/Label/TextAnimation");
		_dialogue = _player.GetNode<Polygon2D>("CharacterBody/Dialogue");
		_pressESprite = _player.GetNode<Sprite2D>("CharacterBody/PressESprite");
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
		_pages[0] = "Продавец: здравствуйте, что вам нужно?";
		_pages[1] = "Ты: здравствуйте, мне нужна канистра бензина на 5 литра.";
		_pages[2] = "Продавец: хорошо, вот, только тут больше 5 литров, секунду...\nДержи, с тебя 250 рублей.";
		_pages[3] = "Ты: спасибо до свидания!";
	}
	public override void _Process(double delta)
	{
		if (GameManager.Instance.Player == null) {
			return;
		}
		if (Input.IsActionJustPressed("next")) {
			_openPage++;
			if (_openPage < 4) {
				_label.SetText(_pages[_openPage]);
				_textAnimation.Play("show_text");
			} else {
				StopDialogue();
			}
		}
		if (Input.IsActionJustPressed("skip") && _IsDialogueOngoing) {
			StopDialogue();
		}
		if (_bodyInRange && Input.IsActionJustPressed("take_item")) {
			Input.SetMouseMode(Input.MouseModeEnum.Visible);
			_dialogue.Visible = true;
			_textAnimation.Play("show_text");
			GameManager.Instance.IsDialogueGoing = true;
			_IsDialogueOngoing = true;
			ChangePressESpriteVisibility();
		}
	}
	private void StopDialogue() {
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
		GameManager.Instance.IsDialogueGoing = false;
		ChangePressESpriteVisibility();
		_petrol.Visible = true;
		_petrol.Monitoring = true;
		GD.Print($"Petrol having {_petrol.Visible} visible and {_petrol.Monitoring} monitoring");
		_dialogue.QueueFree();
		QueueFree();
	}
	private void OnBodyEntered(Node body) {
		if (body.IsInGroup("Player")) {
			ChangePressESpriteVisibility();
			_bodyInRange = true;
		}
	}
	private void OnBodyExited(Node body) {
		if(body.IsInGroup("Player")) {
			ChangePressESpriteVisibility();
			_bodyInRange = false;
		}
	}
	private void ChangePressESpriteVisibility() {
		_pressESprite.Visible = !_pressESprite.Visible;
	}
}
