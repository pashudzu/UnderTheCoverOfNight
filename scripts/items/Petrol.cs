using Godot;
using System;

public partial class Petrol : Item
{
	public override void _Ready()
	{
		base._Ready();
		itemName = "Бензин";
		itemDescription = "Можешь заправить машину и уезжать.";
		inSlotTexturePath = ProjectSettings.GlobalizePath("res://textures/petrolSlot.png");
		texturePath = ProjectSettings.GlobalizePath("res://textures/petrol.png");
		ScenePath = ProjectSettings.GlobalizePath("res://scenes/petrol.tscn");
		itemInHandScenePath = ProjectSettings.GlobalizePath("res://scenes/items/in_hand/petrolInHand.tscn");
		itemInHandScene = (PackedScene)ResourceLoader.Load(itemInHandScenePath);
		itemTextureInSlot = (Texture2D)ResourceLoader.Load(inSlotTexturePath);
		itemTexture = (Texture2D)ResourceLoader.Load(texturePath);
		itemScene = (PackedScene)ResourceLoader.Load(ScenePath);
		IsUsableInInventory = false;
		int id = FindFreeKey(itemName);
		itemContains.Add(itemName + id, this);
	}
	
	public override void _Process(double delta)
	{
		HandleInteraction();
	}
	public override void HandleInteraction() {
		base.HandleInteraction();
		if (Inventory.Instance.IsItemAssignedInInventory("Бензин")) {
			GameManager.Instance.IsPetrolGotten = true;
		}
	}
}
