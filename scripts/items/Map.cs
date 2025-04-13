using Godot;
using System;

public partial class Map : Item
{
	public override void _Ready() {
		base._Ready();
		itemName = "Карта";
		itemId = 4;
		itemDescription = ProjectSettings.GlobalizePath("Ищи нужные места с помощью карты");
		inSlotTexturePath = ProjectSettings.GlobalizePath("res://textures/MapSlot.png");
		texturePath = ProjectSettings.GlobalizePath("res://textures/mapObject.png");
		ScenePath = ProjectSettings.GlobalizePath("res://scenes/map.tscn");
		itemInHandScenePath = ProjectSettings.GlobalizePath("res://scenes/items/in_hand/map_in_hand.tscn");
		itemInHandScene = (PackedScene)ResourceLoader.Load(itemInHandScenePath);
		itemTextureInSlot = (Texture2D)ResourceLoader.Load(inSlotTexturePath);
		itemTexture = (Texture2D)ResourceLoader.Load(texturePath);
		itemScene = (PackedScene)ResourceLoader.Load(ScenePath);
		IsUsableInInventory = false;
		itemContains.Add(itemName, this);
		ItemInWorldPath = ProjectSettings.GlobalizePath(GetPath());
		GD.Print($"Map готов");
	}
	public override void _Process(double delta)
	{
		HandleInteraction();
	}
}
