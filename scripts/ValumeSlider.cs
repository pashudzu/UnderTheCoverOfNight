using Godot;
using System;

public partial class ValumeSlider : HSlider
{
	[Export] public string busName;
	private int busIndex;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		busIndex = AudioServer.GetBusIndex(busName);
		Connect("value_changed", new Callable(this, nameof(OnValueChanged)));
	}
	
	private void OnValueChanged(float value) {
		AudioServer.SetBusVolumeDb(busIndex, Mathf.LinearToDb(value));
	}
}
