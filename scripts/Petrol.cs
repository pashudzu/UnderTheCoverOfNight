using Godot;
using System;

public partial class Petrol : Item
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		itemName = "Бензин";
		itemId = 3;
		itemDescription = "Можешь заправить машину и уезжать.";
		inSlotTexturePath = "res://textures/petrolSlot.png";
		texturePath = "res://textures/petrol.png";
		ScenePath = "res://scenes/petrol.tscn";
		itemInHandScenePath = "res://scenes/petrolInHand.tscn";
		itemInHandScene = (PackedScene)ResourceLoader.Load(itemInHandScenePath);
		itemTextureInSlot = (Texture2D)ResourceLoader.Load(inSlotTexturePath);
		itemTexture = (Texture2D)ResourceLoader.Load(texturePath);
		itemScene = (PackedScene)ResourceLoader.Load(ScenePath);
	}
	
	public override void _Process(double delta)
	{
		HandleInteraction();
	}
}
