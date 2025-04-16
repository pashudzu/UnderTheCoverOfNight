using Godot;
using System;

public partial class Scrap : Item
{
	public override void _Ready() {
		base._Ready();
		itemName = "Лом";
		itemDescription = "Сломай деревяшки и зайди в закрытые дома.";
		inSlotTexturePath = ProjectSettings.GlobalizePath("res://textures/scrapSlot.png");
		texturePath = ProjectSettings.GlobalizePath("res://textures/scrapObject.png");
		ScenePath = ProjectSettings.GlobalizePath("res://scripts/scrap.tscn");
		itemInHandScenePath = ProjectSettings.GlobalizePath("res://scenes/items/in_hand/scrap_in_hand.tscn");
		itemInHandScene = (PackedScene)ResourceLoader.Load(itemInHandScenePath);
		itemTextureInSlot = (Texture2D)ResourceLoader.Load(inSlotTexturePath);
		itemTexture = (Texture2D)ResourceLoader.Load(texturePath);
		itemScene = (PackedScene)ResourceLoader.Load(ScenePath);
		IsUsableInInventory = false;
		ItemInWorldPath = ProjectSettings.GlobalizePath(GetPath());
		int id = FindFreeKey(itemName);
		itemContains.Add(itemName + id, this);
		GD.Print("Scrap готов");
	}
	public override void _Process(double delta)
	{
		HandleInteraction();
	}
}
