using Godot;
using System;

public partial class ItemPopupPanel : CanvasLayer
{
	public static ItemPopupPanel Instance { get; private set; }
	private TextureButton _useItemButton;
	public Item ItemInSlot { get; set; }
	private TextureButton useItemButton;
	
	public override void _Ready() {
		if (Instance != null) {
			Instance.QueueFree();
		}
		Instance = this;
		_useItemButton = GetNode<TextureButton>("ColorRect/UseItem");
		if (ItemInSlot.IsUsableInInventory) {
			GD.Print("Инструмент будет использоваться в инвентаре.");
			_useItemButton.Pressed += OnUseItemButtonPressed;
		} else {
			GD.Print("Инструмент не будет использоваться в инвентаре.");
			_useItemButton.QueueFree();
		}
	}
	public override void _Process(double delta) {
		if (Input.IsActionJustPressed("inventory")) {
			DeleteCurrentItemPopupPanel();
		}
	}
	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed) {
			if (ItemInSlot.IsUsableInInventory) {
				Vector2 mouseGlobalPos = GetViewport().GetMousePosition();
				if (_useItemButton.GetGlobalRect().HasPoint(mouseGlobalPos)) {
					return;
				}
			}
			DeleteCurrentItemPopupPanel();
		}
	}
	private void DeleteCurrentItemPopupPanel() {
		ItemInSlot.DeleteFromInventory();
		Instance = null;
		QueueFree();
	}
	public void OnUseItemButtonPressed() {
		ItemInSlot.UseItem();
		
		DeleteCurrentItemPopupPanel();
	}
}
