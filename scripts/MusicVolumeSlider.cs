using Godot;
using System;

public partial class MusicVolumeSlider : ValumeSlider
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		busName = "Music";
		base._Ready();
	}
}
