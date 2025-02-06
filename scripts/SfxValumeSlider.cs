using Godot;
using System;

public partial class SfxValumeSlider : ValumeSlider
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		busName = "SFX";
		base._Ready();
	}
}
