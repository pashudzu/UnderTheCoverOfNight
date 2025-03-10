using Godot;
using System;

public partial class Petrol : Item
{
	public override void _Ready()
	{
		base._Ready();
		itemName = "Бензин";
		itemId = 3;
		itemDescription = "Можешь заправить машину и уезжать.";
		inSlotTexturePath = ProjectSettings.GlobalizePath("res://textures/petrolSlot.png");
		texturePath = ProjectSettings.GlobalizePath("res://textures/petrol.png");
		ScenePath = ProjectSettings.GlobalizePath("res://scenes/petrol.tscn");
		itemInHandScenePath = ProjectSettings.GlobalizePath("res://scenes/petrolInHand.tscn");
		itemInHandScene = (PackedScene)ResourceLoader.Load(itemInHandScenePath);
		itemTextureInSlot = (Texture2D)ResourceLoader.Load(inSlotTexturePath);
		itemTexture = (Texture2D)ResourceLoader.Load(texturePath);
		itemScene = (PackedScene)ResourceLoader.Load(ScenePath);
		itemContains.Add(itemName, this);
	}
	
	public override void _Process(double delta)
	{
		HandleInteraction();
	}
}
