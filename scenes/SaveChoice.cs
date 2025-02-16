using Godot;
using System;

public partial class SaveChoice : Control
{
	public override void _Ready()
	{
		Button button1 = GetNode<Button>("SlotButton1");
		Button button2 = GetNode<Button>("SlotButton2");
		Button button3 = GetNode<Button>("SlotButton3");
		button1.Connect("pressed", Callable.From(OnSlotButton1Pressed));
		button2.Connect("pressed", Callable.From(OnSlotButton2Pressed));
		button3.Connect("pressed", Callable.From(OnSlotButton3Pressed));
	}
	public void OnSlotButton1Pressed() {
		ChangeScene();
	}
	public void OnSlotButton2Pressed() {
		ChangeScene();
	}
	public void OnSlotButton3Pressed() {
		ChangeScene();
	}
	private void ChangeScene() {
		PackedScene _nextScene = (PackedScene)ResourceLoader.Load("res://scenes/ui/continue_or_new_game_choice.tscn");
		GetTree().ChangeSceneToPacked(_nextScene);
	}
}
