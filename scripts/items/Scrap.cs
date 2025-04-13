using Godot;
using System;

public partial class Scrap : Item
{
	public override void _Ready() {
		base._Ready();
		itemName = "Лом";
		itemId = 5;
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
		itemContains.Add(itemName, this);
		ItemInWorldPath = ProjectSettings.GlobalizePath(GetPath());
		GD.Print("Scrap готов");
	}
	public override void _Process(double delta)
	{
		HandleInteraction();
	}
}
