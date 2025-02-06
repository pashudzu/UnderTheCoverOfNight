using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Control
{
	private List<TextureButton> buttons = new List<TextureButton>();//в buttons входят руки + инвентарь
	private List<TextureButton> slots = new List<TextureButton>();//слоты это инвентарь
	private Dictionary<int, Item> items = new Dictionary<int, Item>();
	private Vector2 mousePosition;
	private Texture2D draggingSprite = null;
	private const int MAX_ITEMS = 4;
	private int originalButton = -1;
	private bool isDragging = false;
	private int buttonIndex;
	public Sprite2D draggingSpriteInstance; 
	public static Inventory Instance { get; private set; }
	public bool isInventoryVisible = false;
	public Node3D itemSceneInstance;
	
	public override void _Ready() {
		Instance = this;
		initializeButtonsAndSlots();
	}
	private void initializeButtonsAndSlots() {
		slots.Add(GetNodeOrNull<TextureButton>("ColorRect/GridContainer/TextureButton1"));
		slots.Add(GetNodeOrNull<TextureButton>("ColorRect/GridContainer/TextureButton2"));
		slots.Add(GetNodeOrNull<TextureButton>("ColorRect/GridContainer/TextureButton3"));
		slots.Add(GetNodeOrNull<TextureButton>("ColorRect/GridContainer/TextureButton4"));
		buttons.Add(GetNodeOrNull<TextureButton>("ColorRect/GridContainer/TextureButton1"));
		buttons.Add(GetNodeOrNull<TextureButton>("ColorRect/GridContainer/TextureButton2"));
		buttons.Add(GetNodeOrNull<TextureButton>("ColorRect/GridContainer/TextureButton3"));
		buttons.Add(GetNodeOrNull<TextureButton>("ColorRect/GridContainer/TextureButton4"));
		buttons.Add(GetNodeOrNull<TextureButton>("ColorRect/LeftHandButton"));
		buttons.Add(GetNodeOrNull<TextureButton>("ColorRect/RightHandButton"));
		
		buttons[0].Connect("pressed", new Callable(this, nameof(OnButton1Pressed)));
		buttons[1].Connect("pressed", new Callable(this, nameof(OnButton2Pressed)));
		buttons[2].Connect("pressed", new Callable(this, nameof(OnButton3Pressed)));
		buttons[3].Connect("pressed", new Callable(this, nameof(OnButton4Pressed)));
		buttons[4].Connect("pressed", new Callable(this, nameof(OnButton5Pressed)));
		buttons[5].Connect("pressed", new Callable(this, nameof(OnButton6Pressed)));
		
		for (int i = 0; i < 6; i++) {
			items.TryAdd(i, null);
			GD.Print($"new item in slot {i}");
		}
	}
	private void OnButton1Pressed() {
		buttonIndex = 0;
		IsItemContains(buttonIndex);
	}
	private void OnButton2Pressed() {
		buttonIndex = 1;
		IsItemContains(buttonIndex);
	}
	private void OnButton3Pressed() {
		buttonIndex = 2;
		IsItemContains(buttonIndex);
	}
	private void OnButton4Pressed() {
		buttonIndex = 3;
		IsItemContains(buttonIndex);
	}
	private void OnButton5Pressed() {
		buttonIndex = 4;
		IsItemContains(buttonIndex);
	}
	private void OnButton6Pressed() {
		buttonIndex = 5;
		IsItemContains(buttonIndex);
	}
	private void IsItemContains(int buttonIndex) {
		GD.Print($"В инвентаре нажата кнопка номер {buttonIndex}");
		if (!isDragging) {
			StartDragging(buttons[buttonIndex], items[buttonIndex], buttonIndex);
		}
		else {
			GD.Print($"isDragging = {isDragging}");
			StopDragging(buttons[buttonIndex], items[originalButton], buttonIndex);
		}
	}
	public void StopDragging(TextureButton button, Item item, int draggedButton) {
		if (originalButton > -1 && items[originalButton] != null) {
			items[draggedButton] = items[originalButton];
			button.TextureNormal = items[originalButton].itemTextureInSlot;
			items[originalButton] = null;
			GD.Print("из items удалён один item");
		} else {
			GD.Print("originalButton < 0, или item == null!");
			return;
		}
		draggingSpriteInstance.QueueFree();
		isDragging = false;
		GD.Print("Завершение перемещения item в инвентаре.");
		if (draggedButton == 4) {
			ItemEquippedToHand(item, true);
		}
		if (draggedButton == 5) {
			ItemEquippedToHand(item, false);
		}
	}
	private void StartDragging(TextureButton button, Item item, int draggedButton) {
		if (items[draggedButton] == null) {
			return;
		}
		if (draggedButton == 4) {
			ItemDeleteFromHand(item, true);
		}
		if (draggedButton == 5) {
			ItemDeleteFromHand(item, false);
		}
		GD.Print($"Ключ {buttonIndex} найден в словаре. Начинаем перетаскивание.");
		button.TextureNormal = (Texture2D)GD.Load("res://textures/slot.png");
		draggingSprite = item.itemTexture;
		mousePosition = GetLocalMousePosition();
		draggingSpriteInstance = new Godot.Sprite2D();
		draggingSpriteInstance.Texture = draggingSprite;
		draggingSpriteInstance.Position = mousePosition;
		AddChild(draggingSpriteInstance);
		originalButton = draggedButton;
		isDragging = true;
	}
	public override void _Input(InputEvent @event)
	{
		if (Input.IsActionJustPressed("inventory") && Engine.TimeScale == 1) {
			if (isInventoryVisible) {
				Hide();
				Input.SetMouseMode(Input.MouseModeEnum.Captured);
			}
			else {
				Show();
				Input.SetMouseMode(Input.MouseModeEnum.Visible);
			}
			isInventoryVisible = !isInventoryVisible;
		}
	}
	public override void _Process(double delta) {
		if (isDragging && draggingSpriteInstance != null) {
			mousePosition = GetLocalMousePosition();
			draggingSpriteInstance.Position = mousePosition;
		}
	}
	public void addItem(Item item) {
		if (item == null) {
			GD.Print("Попытка добавления пусого предмета в инвентарь");
			return;
		}
		else if (items.ContainsValue(item)) {
			GD.Print($"Item с key {item.itemId} уже есть в инвентаре.");
			return;
		}
		Texture2D slotTexture = (Texture2D)GD.Load("res://textures/slot.png");
		for(int i = 0; i < slots.Count; i++) {
			if (slots[i].TextureNormal == slotTexture) {
				items[i] = item;
				slots[i].TextureNormal = item.itemTextureInSlot;
				GD.Print("Item добавлен в инвентарь");
				return;
			}
		}
	}
	public void ItemEquippedToHand(Item item, bool handIsLeft) {
		Node3D player = GameManager.Instance.Player;
		Node3D leftHand = player.GetNodeOrNull<Node3D>("CharacterBody/LeftHand");
		Node3D rightHand = player.GetNodeOrNull<Node3D>("CharacterBody/RightHand");
		if (handIsLeft) {
			if (leftHand == null){
				GD.PrintErr("leftHand == null");
				return;
			}
			GD.Print($"левая рука - {leftHand}");
		}
		else {
			if (rightHand == null){
				GD.PrintErr("rightHand == null");
				return;
			}
			GD.Print($"правая рука - {leftHand}");
		}
		if (item == null) {
			GD.PrintErr("item == null");
		}
		PackedScene itemInHandScene = item.itemInHandScene;
		itemSceneInstance = itemInHandScene.Instantiate<Node3D>();
		if (itemSceneInstance == null) {
			GD.PrintErr("itemSceneInstance == null");
			return;
		}
		if (handIsLeft) {
			leftHand.AddChild(itemSceneInstance);
			GameManager.Instance.LeftHandChild = item.itemName;
			GD.Print($"В левую руку добавлен предмет {item.itemName}");
		}
		else {
			rightHand.AddChild(itemSceneInstance);
			GameManager.Instance.RightHandChild = item.itemName;
			GD.Print($"В правую руку добавлен предмет {item.itemName}");
		}
		GD.Print($"В левую руку добавлен предмет {itemSceneInstance} с именем {item.itemName}.");
	}
	public void ItemDeleteFromHand(Item item, bool handIsLeft) {
		if (itemSceneInstance != null) {
			itemSceneInstance.QueueFree();
			if (handIsLeft) {
				GameManager.Instance.LeftHandChild = null;
				GD.Print("Правая рука теперь пуста.");
			}
			if (!handIsLeft) {
				GameManager.Instance.RightHandChild = null;
				GD.Print("Левая рука теперь пуста.");
			}
		} else {
			GD.PrintErr("itemSceneInstance равен null");
		}
	}
}
