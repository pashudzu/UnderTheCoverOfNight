using Godot;
using System;

public partial class CompassArea : Item
{
	public override void _Ready() {
		base._Ready();
		itemName = "Компас";
		itemDescription = "В связке с картой прекрасный инструмент. Можно увидеть себя на карте и ориентироватсья по карте.";
		inSlotTexturePath = ProjectSettings.GlobalizePath("res://textures/CompassSlot.png");
		texturePath = ProjectSettings.GlobalizePath("res://textures/compassObject.png");
		ScenePath = ProjectSettings.GlobalizePath("res://scenes/compass_area.tscn");
		itemInHandScenePath = ProjectSettings.GlobalizePath("res://scenes/items/in_hand/compass_in_hand.tscn");
		itemInHandScene = (PackedScene)ResourceLoader.Load(itemInHandScenePath);
		itemTextureInSlot = (Texture2D)ResourceLoader.Load(inSlotTexturePath);
		itemTexture = (Texture2D)ResourceLoader.Load(texturePath);
		itemScene = (PackedScene)ResourceLoader.Load(ScenePath);
		IsUsableInInventory = false;
		itemContains.Add(itemName, this);
		ItemInWorldPath = ProjectSettings.GlobalizePath(GetPath());
		GD.Print("Scrap готов");
	}
	public override void _Process(double delta)
	{
		HandleInteraction();
	}
}
