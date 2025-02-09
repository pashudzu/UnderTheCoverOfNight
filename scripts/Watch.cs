using Godot;
using System;

public partial class Watch : Item
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		itemName = "Часы";
		itemId = 2;
		itemDescription = "Смотри за временем, остерегайся кровавой луны";
		inSlotTexturePath = "res://textures/watchSlot.png";
		texturePath = "res://textures/watch.png";
		string ScenePath = "res://scenes/watch.tscn";
		string watchInHandScenePath = "res://scenes/wath_in_hand.tscn";
		itemInHandScene = (PackedScene)ResourceLoader.Load(watchInHandScenePath);
		itemTextureInSlot = (Texture2D)ResourceLoader.Load(inSlotTexturePath);
		itemTexture = (Texture2D)ResourceLoader.Load(texturePath);
		itemScene = (PackedScene)ResourceLoader.Load(ScenePath);
		GD.Print("Watch is ready");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		HandleInteraction();
	}
}
