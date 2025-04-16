using Godot;
using System;
using System.Collections.Generic;

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
	public static Dictionary<string, Item> itemContains = new Dictionary<string, Item>();
	private Node3D _player;
	private Sprite2D _pressESprite;
	public string ItemInWorldPath;
	public bool IsUsableInInventory;
	public bool IsUseItemButtonPressed;
	
	public override void _Ready()
	{
		if (GameManager.Instance.Player == null) {
			return;
		}
		_player = GameManager.Instance.Player;
		_pressESprite = _player.GetNode<Sprite2D>("CharacterBody/PressESprite");
		if (Connect("body_entered", new Callable(this, nameof(OnBodyEntered))) == Error.Ok) {
			GD.Print("Сигнал body_entered успешно установлен");
		}
		if (Connect("body_exited", new Callable(this, nameof(OnBodyExited))) == Error.Ok) {
			GD.Print("Сигнал body_exited успешно установлен");
		}
	}
	protected int FindFreeKey(string itemName) {
		for (int i = 1; true; i++) {
			if (!itemContains.ContainsKey(itemName + i)) {
				return i;
			}
			if (i > 10) {//сделано для предотвращения возможного бесконечного цикла
				GD.PrintErr($"В {itemName} мог быть бесконечный цикл.");
				return -1;
			}
		}
	}
	private void OnBodyEntered(Node body) {
		GD.Print("Some body entered in range");
		if (body.IsInGroup("Player")) {
			_pressESprite.Visible = true;
			isPlayerInRange = true;
			GD.Print("Player is in range");
		}
	}
	
	private void OnBodyExited(Node body) {
		if(body.IsInGroup("Player")) {
			_pressESprite.Visible = false;
			isPlayerInRange = false;
			GD.Print("Player is exited range");
		}
	}
	public virtual void HandleInteraction() {
		if (Input.IsActionJustPressed("take_item") && isPlayerInRange) {
			AddToInventory();
			GD.Print("Item is free");
		}
	}
	private void AddToInventory() {
		GD.Print($"Пытаюсь добавить item {itemName} в инвентарь.");
		if (Inventory.Instance == null) {
			GD.PrintErr("Ошибка: Inventory.Instance == null!");
			return;
		}
		Inventory.Instance.AddItem(this);
		GD.Print($"Добавлен item {itemName}, теперь освобождаю.");
		QueueFree();
	}
	public void DeleteFromInventory() {
		GD.Print($"Пытаюсь удалить item {itemName} из инвентаря.");
		if (Inventory.Instance == null) {
			GD.PrintErr("Ошибка: Inventory.Instance == null!");
			return;
		}
		Inventory.Instance.DeleteItem(this);
		GD.Print($"Предмет {itemName} успшно удалён из инвентаря.");
		QueueFree();
	}
	public void DropFromInventory() {
		
	}
	public virtual void UseItem() {
		GD.Print("Попытка использовать предмет в инвентаре");
	}
}
