using Godot;
using System;
using System.Threading.Tasks;

public partial class Area3d : Area3D
{
	private AnimationPlayer _casherAnimation;
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
	public override void _Ready()
	{
		if (GameManager.Instance.Player == null) {
			GD.PrintErr("Ошибка в area3d, игрок отсутвует в GameManager.");
			return;
		}
		if (GameManager.Instance.CasherAnimationPlayer == null) {
			GD.PrintErr("Ошибка в area3d, анимация касира не записана в GameManager.");
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
		_casherAnimation = GameManager.Instance.CasherAnimationPlayer;
	}
	public override void _Process(double delta)
	{
		if (GameManager.Instance.Player == null) {
			GD.PrintErr("Игрок в коде area3d не обозначин в GameManager.");
			return;
		}
		if (Input.IsActionJustPressed("next") && _IsDialogueOngoing) {
			_openPage++;
			if (_openPage < 4) {
				_label.SetText(_pages[_openPage]);
				_textAnimation.Play("show_text");
			} else {
				GD.Print("Количество страниц диалога с кассиром меньше четырёх, по этой причине разговор окончился.");
				StopDialogue();
			}
			ShowAnimation();
		}
		if (Input.IsActionJustPressed("skip") && _IsDialogueOngoing) {
			GD.Print("Игрок скипнул диалог с кассиром.");
			StopDialogue();
		}
		if (_bodyInRange && Input.IsActionJustPressed("take_item")) {
			Input.SetMouseMode(Input.MouseModeEnum.Visible);
			_dialogue.Visible = true;
			_textAnimation.Play("show_text");
			GameManager.Instance.IsDialogueGoing = true;
			_IsDialogueOngoing = true;
			_pressESprite.Visible = false;
		}
	}
	private void ShowAnimation() {
		if (_openPage == 2) {
			GameManager.Instance.CasherAnimationPlayer.Play("sell_petrol");
		}
		if (_openPage == 3) {
			GameManager.Instance.CasherAnimationPlayer.Play("put_petrol");
		}
	}
	private void StopDialogue() {
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
		GameManager.Instance.IsDialogueGoing = false;
		_pressESprite.Visible = false;
		_petrol.Visible = true;
		_petrol.Monitoring = true;
		GD.Print($"Petrol having {_petrol.Visible} visible and {_petrol.Monitoring} monitoring");
		_dialogue.QueueFree();
		QueueFree();
	}
	private void OnBodyEntered(Node body) {
		if (body.IsInGroup("Player")) {
			GD.Print("Игрок зашёл в зону разговора с кассиром.");
			_pressESprite.Visible = true;
			_bodyInRange = true;
		}
	}
	private void OnBodyExited(Node body) {
		if(body.IsInGroup("Player")) {
			_pressESprite.Visible = false;
			_bodyInRange = false;
		}
	}
}
