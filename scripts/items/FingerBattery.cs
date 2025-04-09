using Godot;
using System;

public partial class FingerBattery : Item
{
	public override void _Ready() {
		base._Ready();
		itemName = "Пальчиковая батарейка";
		itemDescription = "Заряди фонарик.";
		itemId = 6;
		inSlotTexturePath = ProjectSettings.GlobalizePath("res://textures/fingerBatterySlot.png");
		texturePath = ProjectSettings.GlobalizePath("res://textures/fingerBatteryObject.png");
		ScenePath = ProjectSettings.GlobalizePath("res://scenes/finger_battery.tscn");
		itemInHandScenePath = ProjectSettings.GlobalizePath("res://scenes/finger_battery_in_hand.tscn");
		itemInHandScene = (PackedScene)ResourceLoader.Load(itemInHandScenePath);
		itemTextureInSlot = (Texture2D)ResourceLoader.Load(inSlotTexturePath);
		itemTexture = (Texture2D)ResourceLoader.Load(texturePath);
		itemScene = (PackedScene)ResourceLoader.Load(ScenePath);
		IsUsableInInventory = true;
		itemContains.Add(itemName, this);
		ItemInWorldPath = ProjectSettings.GlobalizePath(GetPath());
		GD.Print("FingerBattery готов");
	}
	public override void _Process(double delta)
	{
		HandleInteraction();
	}
}
