using Godot;
using System;

public partial class Torch : Item
{
	public override void _Ready() {
		base._Ready();
		itemName = "Фонарик";
		itemId = 1;
		itemDescription = "Освети себе дорогу фонариком, но помни, энергия не бесконечна.";
		inSlotTexturePath = "res://textures/torchInSlot.png";
		texturePath = "res://textures/torch.png";
		ScenePath = "res://scenes/torch.tscn";
		itemInHandScenePath = "res://scenes/torch_in_hand.tscn";
		itemInHandScene = (PackedScene)ResourceLoader.Load(itemInHandScenePath);
		itemTextureInSlot = (Texture2D)ResourceLoader.Load(inSlotTexturePath);
		itemTexture = (Texture2D)ResourceLoader.Load(texturePath);
		itemScene = (PackedScene)ResourceLoader.Load(ScenePath);
		GD.Print("Torch is ready");
	}
	public override void _Process(double delta)
	{
		HandleInteraction();
	}
}
