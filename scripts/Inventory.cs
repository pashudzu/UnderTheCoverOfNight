using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Inventory : Control
{
	public static Inventory Instance { get; private set; }
	private List<TextureButton> _buttons = new List<TextureButton>();//в _buttons входят руки + инвентарь
	private List<TextureButton> _slots = new List<TextureButton>();  //слоты инвентаря
	private Dictionary<int, Item> _items = new Dictionary<int, Item>();
	private Vector2 _mousePosition;
	private Texture2D _draggingSprite = null;
	private const int MaxItems = 4;
	private int _originalButton = -1;
	private bool _isDragging = false;
	private int _buttonIndex;
	public Sprite2D draggingSpriteInstance; 
	public bool isInventoryVisible = false;
	public Node3D itemSceneInstance;
	private const int CountOfSlots = 4;
	
	public override void _Ready() {
		Instance = this;
		InitializeButtonsAndSlots();
		ResumeInventoryData();
	}
	private void InitializeButtonsAndSlots() {
		for (int i = 1; i < CountOfSlots + 1; i++) {
			_slots.Add(GetNodeOrNull<TextureButton>($"ColorRect/GridContainer/TextureButton{i}"));
			GD.Print($"slot {i} ready");
		}
		
		for (int i = 1; i < 5; i++) {
			_buttons.Add(GetNodeOrNull<TextureButton>($"ColorRect/GridContainer/TextureButton{i}"));
			GD.Print($"button {i} ready");
		}
		_buttons.Add(GetNodeOrNull<TextureButton>("ColorRect/LeftHandButton"));
		_buttons.Add(GetNodeOrNull<TextureButton>("ColorRect/RightHandButton"));
		
		string[] _buttonMethods = { "OnButton1Pressed", 
		"OnButton2Pressed", "OnButton3Pressed", "OnButton4Pressed", 
		"OnButton5Pressed", "OnButton6Pressed", };
		
		for (int i = 0; i < _buttons.Count; i++) {
			if (_buttons[i] != null) {
				_buttons[i].Connect("pressed", new Callable(this, _buttonMethods[i]));
			}
			else {
				GD.PrintErr($"_button{i} == null");
			}
		}
		for (int i = 0; i < 6; i++) {
			_items.TryAdd(i, null);
			GameManager.Instance.SavedItems.TryAdd(i, null);
			GD.Print($"new item in slot {i}");
		}
	}
	private void ResumeInventoryData(){
		if (GameManager.Instance.SavedSlots != null) {
			string texturePath;
			for (int i = 0; i < GameManager.Instance.SavedSlots.Count; i++) {
				texturePath = GameManager.Instance.SavedSlots[i];
				Texture2D texture = ResourceLoader.Load<Texture2D>(texturePath);
				if (texture != null) {
					_slots[i].TextureNormal = texture;
				}
			}
		}
	}
	private void OnButton1Pressed() {
		_buttonIndex = 0;
		IsItemContains(_buttonIndex);
	}
	private void OnButton2Pressed() {
		_buttonIndex = 1;
		IsItemContains(_buttonIndex);
	}
	private void OnButton3Pressed() {
		_buttonIndex = 2;
		IsItemContains(_buttonIndex);
	}
	private void OnButton4Pressed() {
		_buttonIndex = 3;
		IsItemContains(_buttonIndex);
	}
	private void OnButton5Pressed() {
		_buttonIndex = 4;
		IsItemContains(_buttonIndex);
	}
	private void OnButton6Pressed() {
		_buttonIndex = 5;
		IsItemContains(_buttonIndex);
	}
	private void IsItemContains(int _buttonIndex) {
		GD.Print($"В инвентаре нажата кнопка номер {_buttonIndex}");
		if (!_isDragging) {
			StartDragging(_buttons[_buttonIndex], _items[_buttonIndex], _buttonIndex);
		}
		else {
			GD.Print($"_isDragging = {_isDragging}");
			StopDragging(_buttons[_buttonIndex], _items[_originalButton], _buttonIndex);
		}
	}
	public void StopDragging(TextureButton button, Item item, int draggedButton) {
		if (_originalButton > -1 && _items[_originalButton] != null) {
			GameManager.Instance.SavedItems[draggedButton] = _items[_originalButton].itemName;
			_items[draggedButton] = _items[_originalButton];
			button.TextureNormal = _items[_originalButton].itemTextureInSlot;
			_items[_originalButton] = null;
			GameManager.Instance.SavedItems[_originalButton] = null;
			GD.Print("из _items удалён один item");
		} else {
			GD.Print("_originalButton < 0, или item == null!");
			return;
		}
		draggingSpriteInstance.QueueFree();
		_isDragging = false;
		GD.Print("Завершение перемещения item в инвентаре.");
		if (draggedButton == 4) {
			ItemEquippedToHand(item, true);
		}
		if (draggedButton == 5) {
			ItemEquippedToHand(item, false);
		}
	}
	private void StartDragging(TextureButton button, Item item, int draggedButton) {
		if (_items[draggedButton] == null) {
			return;
		}
		if (draggedButton == 4) {
			ItemDeleteFromHand(item, true);
		}
		if (draggedButton == 5) {
			ItemDeleteFromHand(item, false);
		}
		GD.Print($"Ключ {_buttonIndex} найден в словаре. Начинаем перетаскивание.");
		button.TextureNormal = (Texture2D)GD.Load("res://textures/slot.png");
		_draggingSprite = item.itemTexture;
		_mousePosition = GetLocalMousePosition();
		draggingSpriteInstance = new Godot.Sprite2D();
		draggingSpriteInstance.Texture = _draggingSprite;
		draggingSpriteInstance.Position = _mousePosition;
		AddChild(draggingSpriteInstance);
		_originalButton = draggedButton;
		_isDragging = true;
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
		if (_isDragging && draggingSpriteInstance != null) {
			_mousePosition = GetLocalMousePosition();
			draggingSpriteInstance.Position = _mousePosition;
		}
	}
	public void addItem(Item item) {
		if (item == null) {
			GD.Print("Попытка добавления пусого предмета в инвентарь");
			return;
		} else if (_items.ContainsValue(item)) {
			GD.Print($"Item с key {item.itemId} уже есть в инвентаре.");
			return;
		}
		Texture2D slotTexture = (Texture2D)GD.Load("res://textures/slot.png");
		for (int i = 0; i < _slots.Count; i++) {
			if (_slots[i].TextureNormal == slotTexture) {
				_items[i] = item;
				GameManager.Instance.SavedItems[i] = item.itemName;
				_slots[i].TextureNormal = item.itemTextureInSlot;
				GD.Print("Item добавлен в инвентарь");
				
				GameManager.Instance.SavedSlots = _slots.Select(_slots => _slots.TextureNormal.ResourcePath).ToList();
				return;
			}
		}
		foreach (var pair in _items) {
			GameManager.Instance.SavedItems[pair.Key] = pair.Value.itemName;
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
