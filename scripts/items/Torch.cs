using Godot;
using System;

public partial class Torch : Item
{
	public override void _Ready() {
		base._Ready();
		itemName = "Фонарик";
		itemDescription = "Освети себе дорогу фонариком, но помни, энергия не бесконечна.";
		inSlotTexturePath = ProjectSettings.GlobalizePath("res://textures/torchInSlot.png");
		texturePath = ProjectSettings.GlobalizePath("res://textures/torch.png");
		ScenePath = ProjectSettings.GlobalizePath("res://scenes/torch.tscn");
		itemInHandScenePath = ProjectSettings.GlobalizePath("res://scenes/items/in_hand/torch_in_hand.tscn");
		itemInHandScene = (PackedScene)ResourceLoader.Load(itemInHandScenePath);
		itemTextureInSlot = (Texture2D)ResourceLoader.Load(inSlotTexturePath);
		itemTexture = (Texture2D)ResourceLoader.Load(texturePath);
		itemScene = (PackedScene)ResourceLoader.Load(ScenePath);
		IsUsableInInventory = false;
		ItemInWorldPath = ProjectSettings.GlobalizePath(GetPath());
		int id = FindFreeKey(itemName);
		itemContains.Add(itemName + id, this);
		GD.Print($"Torch готов");
	}
	public override void _Process(double delta)
	{
		HandleInteraction();
	}
}
