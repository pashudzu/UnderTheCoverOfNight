using Godot;
using System;

public partial class ItemPopupPanel : CanvasLayer
{
	public static ItemPopupPanel Instance { get; private set; }
	TextureButton useItemButton;
	
	public override void _Ready() {
		if (Instance != null) {
			Instance.QueueFree();
		}
		Instance = this;
		useItemButton = GetNode<TextureButton>("UseItem");
		useItemButton.Pressed += OnUseItemButtonPressed;
	}
	public override void _Process(double delta) {
		if (Input.IsActionJustPressed("inventory")) {
			DeleteCurrentItemPopupPanel();
		}
	}
	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed) {
			DeleteCurrentItemPopupPanel();
		}
	}
	private void DeleteCurrentItemPopupPanel() {
		Instance = null;
		QueueFree();
	}
	public void OnUseItemButtonPressed() {
		
	}
}
