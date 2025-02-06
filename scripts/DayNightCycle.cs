using Godot;
using System;

public partial class DayNightCycle : Node3D
{
	DirectionalLight3D sun;
	
	public override void _Ready()
	{
		sun = GetNode<DirectionalLight3D>("Sun");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
