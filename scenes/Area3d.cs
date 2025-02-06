using Godot;
using System;

public partial class Area3d : Area3D
{
	private Node3D player;
	private Sprite2D pressESprite;
	private Polygon2D dialogue;
	private AnimationPlayer textAnimation;
	private Label label;
	private bool bodyInRange = false;
	private bool IsDialogueOngoing = false;
	private string[] pages = new string[4];
	private int openPage = 0;
	private Area3D petrol;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (GameManager.Instance.Player == null) {
			return;
		}
		petrol = GetParent().GetParent().GetNode<Area3D>("Petrol");
		player = GameManager.Instance.Player;
		label = player.GetNode<Label>("CharacterBody/Dialogue/Label");
		textAnimation = player.GetNode<AnimationPlayer>("CharacterBody/Dialogue/Label/TextAnimation");
		dialogue = player.GetNode<Polygon2D>("CharacterBody/Dialogue");
		pressESprite = player.GetNode<Sprite2D>("CharacterBody/PressESprite");
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
		pages[0] = "Продавец: здравствуйте, что вам нужно?";
		pages[1] = "Ты: здравствуйте, мне нужна канистра бензина на 5 литра.";
		pages[2] = "Продавец: хорошо, вот, только тут больше 5 литров, секунду...\nДержи, с тебя 250 рублей.";
		pages[3] = "Ты: спасибо до свидания!";
	}
	public override void _Process(double delta)
	{
		if (GameManager.Instance.Player == null) {
			return;
		}
		if (Input.IsActionJustPressed("next")) {
			openPage++;
			if (openPage < 4) {
				label.SetText(pages[openPage]);
				textAnimation.Play("show_text");
			} else {
				StopDialogue();
			}
		}
		if (Input.IsActionJustPressed("skip") && IsDialogueOngoing) {
			StopDialogue();
		}
		if (bodyInRange && Input.IsActionJustPressed("take_item")) {
			Input.SetMouseMode(Input.MouseModeEnum.Visible);
			dialogue.Visible = true;
			textAnimation.Play("show_text");
			GameManager.Instance.IsDialogueGoing = true;
			IsDialogueOngoing = true;
			ChangePressESpriteVisibility();
		}
	}
	private void StopDialogue() {
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
		GameManager.Instance.IsDialogueGoing = false;
		ChangePressESpriteVisibility();
		petrol.Visible = true;
		petrol.Monitoring = true;
		GD.Print($"Petrol having {petrol.Visible} visible and {petrol.Monitoring} monitoring");
		dialogue.QueueFree();
		QueueFree();
	}
	private void OnBodyEntered(Node body) {
		if (body.IsInGroup("Player")) {
			ChangePressESpriteVisibility();
			bodyInRange = true;
		}
	}
	private void OnBodyExited(Node body) {
		if(body.IsInGroup("Player")) {
			ChangePressESpriteVisibility();
			bodyInRange = false;
		}
	}
	private void ChangePressESpriteVisibility() {
		pressESprite.Visible = !pressESprite.Visible;
	}
}
