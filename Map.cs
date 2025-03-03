using Godot;
using System;

public partial class Map : Item
{
	public override void _Ready() {
		base._Ready();
		itemName = "Карта";
		itemId = 1;
		itemDescription = "Ищи с помощью карты";
		inSlotTexturePath = "res://textures/MapSlot.png";
		texturePath = "res://textures/mapObject.png";
		ScenePath = "res://scenes/map.tscn";
		itemInHandScenePath = "res://scenes/map_in_hand.tscn";
		itemInHandScene = (PackedScene)ResourceLoader.Load(itemInHandScenePath);
		itemTextureInSlot = (Texture2D)ResourceLoader.Load(inSlotTexturePath);
		itemTexture = (Texture2D)ResourceLoader.Load(texturePath);
		itemScene = (PackedScene)ResourceLoader.Load(ScenePath);
		itemContains.Add(itemName, this);
		ItemInWorldPath = ProjectSettings.GlobalizePath(GetPath());
		GD.Print($"Map готов");
	}
	public override void _Process(double delta)
	{
		HandleInteraction();
	}
}
