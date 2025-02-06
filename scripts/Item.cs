using Godot;
using System;

public partial class Item : Area3D
{
	public string itemName;
	public int itemId;
	public string itemDescription;
	public bool isPlayerInRange = false;
	public bool isCollectible = false;
	public string inSlotTexturePath;
	public string texturePath;
	public string itemScenePath;
	public string itemInHandScenePath;
	public string ScenePath;
	public PackedScene itemInHandScene;
	public Texture2D itemTextureInSlot;
	public Texture2D itemTexture;
	public PackedScene itemScene;
	private Node3D player;
	private Sprite2D pressESprite;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (GameManager.Instance.Player == null) {
			return;
		}
		player = GameManager.Instance.Player;
		pressESprite = player.GetNode<Sprite2D>("CharacterBody/PressESprite");
		if (Connect("body_entered", new Callable(this, nameof(OnBodyEntered))) == Error.Ok) {
			GD.Print("Сигнал body_entered успешно установлен");
		}
		if (Connect("body_exited", new Callable(this, nameof(OnBodyExited))) == Error.Ok) {
			GD.Print("Сигнал body_exited успешно установлен");
		}
	}

	private void OnBodyEntered(Node body) {
		
		GD.Print("Some body entered in range");
		if (body.IsInGroup("Player")) {
			ChangePressESpriteVisibility();
			isPlayerInRange = true;
			GD.Print("Player is in range");
		}
	}
	
	private void OnBodyExited(Node body) {
		if(body.IsInGroup("Player")) {
			ChangePressESpriteVisibility();
			isPlayerInRange = false;
			GD.Print("Player is exited range");
		}
	}
	public void HandleInteraction() {
		if (Input.IsActionJustPressed("take_item") && isPlayerInRange) {
			AddToInventory();
			GD.Print("Item is free");
		}
	}
	private void AddToInventory() {
		GD.Print($"Освобождение объекта {itemName}");
		QueueFree();
		Inventory.Instance.addItem(this);
	}
	private void ChangePressESpriteVisibility() {
		pressESprite.Visible = !pressESprite.Visible;
	}
}
