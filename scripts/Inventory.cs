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
			_slots.Add(GetNode<TextureButton>($"TextureRect/GridContainer/TextureButton{i}"));
			GD.Print($"slot {i} ready");
		}
		
		for (int i = 1; i < 5; i++) {
			_buttons.Add(GetNode<TextureButton>($"TextureRect/GridContainer/TextureButton{i}"));
			GD.Print($"button {i} ready");
		}
		_buttons.Add(GetNode<TextureButton>("TextureRect/LeftHandButton"));
		_buttons.Add(GetNode<TextureButton>("TextureRect/RightHandButton"));
		
		Action[] _buttonLMBMethods = { OnButton1LMBPressed, 
			OnButton2LMBPressed, OnButton3LMBPressed, OnButton4LMBPressed, 
			OnButton5LMBPressed, OnButton6LMBPressed 
		};
		for (int i = 0; i < _buttons.Count; i++) {
			if (_buttons[i] != null) {
				_buttons[i].Pressed += _buttonLMBMethods[i];
			} else {
				GD.PrintErr($"Кнопка с интендификкатором {i} имеет значение null(ЛКМ).");
			}
		}
		for (int i = 0; i < 6; i++) {
			_items.TryAdd(i, null);
			GameManager.Instance.SavedItems.TryAdd(i, null);
			GD.Print($"new item in slot {i}");
		}
	}
	private async void ResumeInventoryData(){
		if (GameManager.Instance.WasGameSaved) {
			if (GameManager.Instance.SavedButtons != null) {
				ResumeButtonsData();
			}
			ResumeSlotsData();
			await ToSignal(GetTree(), "process_frame");
			ResumeItemData();
		}
	}
	private void ResumeButtonsData() {
		for (int i = 0; i < GameManager.Instance.SavedButtons.Count; i++) {
			if (GameManager.Instance.SavedButtons.Count < _buttons.Count) {
				GameManager.Instance.SavedButtons.TryAdd(i, null);
			}
			else {
				GD.PrintErr($"_button{i} == null");
			}
		}
	}
	private void ResumeSlotsData() {
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
	private void ResumeItemData() {
		if (GameManager.Instance.SavedItems == null) {
			return;
		}
		foreach(var pair in GameManager.Instance.SavedItems) {
			if (pair.Value != "") {
				GD.Print($"📦 Размер itemContains перед foreach: {Item.itemContains.Count}");
				if (Item.itemContains.TryGetValue(pair.Value, out Item _foundItem)) {
					_items[pair.Key] = _foundItem;
					Area3D itemNode = GetNode<Area3D>(_foundItem.ItemInWorldPath);
					itemNode.QueueFree();
					if (pair.Key == 4) {
						ItemEquippedToHand(_foundItem, true);
					}
					if (pair.Key == 5) {
						ItemEquippedToHand(_foundItem, false);
					}
					GD.Print($"В словарь _items в значение, у которого ключ = {pair.Key} установлен предмет");
				} else {
					GD.PrintErr($"Не найден item для возобновление Item(а) в игру. Искомый Item имеет имя {pair.Value} и ключ {pair.Key}");
				}
			} else {
				GD.Print($"В сохранённом словаре в значении при ключе {pair.Key} значение = null");
				continue;
			}
		}
	}
	private void OnButton1LMBPressed() {
		_buttonIndex = 0;
		GD.Print($"Нажата кнопка {_buttonIndex} в инвентаре.");
		IsItemContains(_buttonIndex);
	}
	private void OnButton2LMBPressed() {
		_buttonIndex = 1;
		GD.Print($"Нажата кнопка {_buttonIndex} в инвентаре.");
		IsItemContains(_buttonIndex);
	}
	private void OnButton3LMBPressed() {
		_buttonIndex = 2;
		GD.Print($"Нажата кнопка {_buttonIndex} в инвентаре.");
		IsItemContains(_buttonIndex);
	}
	private void OnButton4LMBPressed() {
		_buttonIndex = 3;
		GD.Print($"Нажата кнопка {_buttonIndex} в инвентаре.");
		IsItemContains(_buttonIndex);
	}
	private void OnButton5LMBPressed() {
		_buttonIndex = 4;
		GD.Print($"Нажата кнопка {_buttonIndex} в инвентаре.");
		IsItemContains(_buttonIndex);
	}
	private void OnButton6LMBPressed() {
		_buttonIndex = 5;
		GD.Print($"Нажата кнопка {_buttonIndex} в инвентаре.");
		IsItemContains(_buttonIndex);
	}
	private void OnButton1RMBPressed() {
		ButtonRMBPressed(0);
	}
	private void OnButton2RMBPressed() {
		ButtonRMBPressed(1);
	}
	private void OnButton3RMBPressed() {
		ButtonRMBPressed(2);
	}
	private void OnButton4RMBPressed() {
		ButtonRMBPressed(3);
	}
	private void OnButton5RMBPressed() {
		ButtonRMBPressed(4);
	}
	private void OnButton6RMBPressed() {
		ButtonRMBPressed(5);
	}
	private void ButtonRMBPressed(int _buttonIndex) {
		GD.Print($"Была нажата правой кнопкой мышки кнопка {_buttonIndex + 1}.");
		if (_items[_buttonIndex] != null) {
			CreateItemPopupPanel(_buttonIndex);
		}
	}
	private void IsItemContains(int _buttonIndex) {
		GD.Print($"В инвентаре нажата кнопка номер {_buttonIndex}");
		if (!_isDragging) {
			GD.Print($"_isDragging = {_isDragging}");
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
			GameManager.Instance.SavedButtons[draggedButton] = button.TextureNormal.ResourcePath;
			GD.Print($"В GameManager сохранён путь к текстуре у кнопки {draggedButton}, путь {button.TextureNormal.ResourcePath}.");
			_items[_originalButton] = null;
			GameManager.Instance.SavedItems[_originalButton] = null;
			GameManager.Instance.SavedButtons[_originalButton] = null;
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
		GameManager.Instance.SavedSlots = _slots.Select(_slots => _slots.TextureNormal.ResourcePath).ToList();
	}
	private void StartDragging(TextureButton button, Item item, int draggedButton) {
		GD.Print("StartDragging вызван.");
		if (item == null) {
			GD.PrintErr("Попытка переместить в инвентаре не существующий объект.");
			return;
		}
		GD.Print($"В инвентаре перетаскивается предмет с именем {_items[draggedButton].itemName}");
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
		button.TextureNormal = (Texture2D)GD.Load(ProjectSettings.GlobalizePath("res://textures/slot.png"));
		_draggingSprite = item.itemTexture;
		_mousePosition = GetLocalMousePosition();
		draggingSpriteInstance = new Godot.Sprite2D();
		draggingSpriteInstance.Texture = _draggingSprite;
		draggingSpriteInstance.Position = _mousePosition;
		AddChild(draggingSpriteInstance);
		_originalButton = draggedButton;
		_isDragging = true;
	}
	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseButton mouseEvent) {
			CountRMBPressing(mouseEvent);
		}
	}
	private void CountRMBPressing(InputEventMouseButton mouseEvent) {
		if (mouseEvent.ButtonIndex == MouseButton.Right && mouseEvent.Pressed) {
			for (int i = 0; i < _buttons.Count; i++) {
				if (_buttons[i].GetGlobalRect().HasPoint(GetGlobalMousePosition())) {
					switch (i) {
						case 0: OnButton1RMBPressed(); break;
						case 1: OnButton2RMBPressed(); break;
						case 2: OnButton3RMBPressed(); break;
						case 3: OnButton4RMBPressed(); break;
						case 4: OnButton5RMBPressed(); break;
						case 5: OnButton6RMBPressed(); break;
					}
				}
			}
		}
	}
	private void CreateItemPopupPanel(int _buttonIndex) {
		string itemInSlotName = _items[_buttonIndex].itemName;
		string itemInSlotDescription = _items[_buttonIndex].itemDescription;
		PackedScene scene = (PackedScene)ResourceLoader.Load("res://scenes/ui/item_popup_panel.tscn");
		CanvasLayer instance = scene.Instantiate<CanvasLayer>();
		ColorRect _rect = instance.GetNode<ColorRect>("ColorRect");
		_rect.GlobalPosition = GetGlobalMousePosition();
		_rect.GetNode<Label>("ItemNameLabel").Text = itemInSlotName;
		_rect.GetNode<Label>("ItemDescriptionLabel").Text = itemInSlotDescription;
		GetTree().CurrentScene.AddChild(instance);
	}
	private void ChangeInventoryVisible() {
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
		ChangeInventoryVisible();
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
		Texture2D slotTexture = (Texture2D)GD.Load(ProjectSettings.GlobalizePath("res://textures/slot.png"));
		for (int i = 0; i < _slots.Count; i++) {
			if (_slots[i] == null) {
				GD.PrintErr($"Слот с индексом {i} не проинициализирован");
				continue;
			}
			if (_slots[i].TextureNormal == slotTexture) {
				_items[i] = item;
				GameManager.Instance.SavedItems[i] = item.itemName;
				_slots[i].TextureNormal = item.itemTextureInSlot;
				GD.Print($"Item с именем {item.itemName} добавлен в инвентарь");
				
				GameManager.Instance.SavedSlots = _slots.Select(_slots => _slots.TextureNormal.ResourcePath).ToList();
				return;
			}
		}
		foreach (var pair in _items) {
			GD.Print($"В инвентаре в слоте {pair.Key} есть предмет {pair.Value.itemName}.");
			GameManager.Instance.SavedItems[pair.Key] = pair.Value.itemName;
		}
	}
	public void ItemEquippedToHand(Item item, bool handIsLeft) {
		Node3D player = GameManager.Instance.Player;
		Node3D leftHand = player.GetNode<Node3D>("CharacterBody/LeftHand");
		Node3D rightHand = player.GetNode<Node3D>("CharacterBody/RightHand");
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
			GD.Print($"правая рука - {rightHand}");
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
