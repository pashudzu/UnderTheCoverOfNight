using Godot;
using System;

public partial class FingerBattery : Item
{
 	private const float _amountChargeBattery = 50;
	public override void _Ready() {
		base._Ready();
		itemName = "Пальчиковая батарейка";
		itemDescription = "Заряди фонарик.";
		inSlotTexturePath = ProjectSettings.GlobalizePath("res://textures/fingerBatterySlot.png");
		texturePath = ProjectSettings.GlobalizePath("res://textures/fingerBatteryObject.png");
		ScenePath = ProjectSettings.GlobalizePath("res://scenes/finger_battery.tscn");
		itemInHandScenePath = ProjectSettings.GlobalizePath("res://scenes/items/in_hand/finger_battery_in_hand.tscn");
		itemInHandScene = (PackedScene)ResourceLoader.Load(itemInHandScenePath);
		itemTextureInSlot = (Texture2D)ResourceLoader.Load(inSlotTexturePath);
		itemTexture = (Texture2D)ResourceLoader.Load(texturePath);
		itemScene = (PackedScene)ResourceLoader.Load(ScenePath);
		IsUsableInInventory = true;
		ItemInWorldPath = ProjectSettings.GlobalizePath(GetPath());
		int id = FindFreeKey(itemName);
		itemContains.Add(itemName + id, this);
		GD.Print("FingerBattery готов");
	}
	public override void _Process(double delta)
	{
		HandleInteraction();
	}
	public override void UseItem() {
		base.UseItem();
		if (IsTorchPresentInventory()) {
			GD.Print("Фонарик есть в инвентаре и сейчас батарейка будет добавлть ему заряда.");
			if ((GameManager.Instance.TorchBatteryCharge + _amountChargeBattery) > 100f) {
	 				GameManager.Instance.TorchBatteryCharge = 100f;
	 		} else {
	 			GameManager.Instance.TorchBatteryCharge = GameManager.Instance.TorchBatteryCharge + _amountChargeBattery;
	 		}
		}
	}
	private bool IsTorchPresentInventory() {
		for (int i = 0; i < GameManager.Instance.SavedItems.Count; i++) {
			if (GameManager.Instance.SavedItems[i] == "Фонарик") {
				return true;
			}
		}
		return false;
	}
}
