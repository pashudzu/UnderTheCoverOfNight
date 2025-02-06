using Godot;
using System;

public partial class MasterVolumeSlider : ValumeSlider
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		busName = "Master";
		base._Ready();
	}
}
