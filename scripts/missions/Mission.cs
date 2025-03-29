using Godot;
using System;
using System.Collections.Generic;

public partial class Mission : Node
{
	public byte Id { get; protected set; }
	public string Name { get; protected set; }
	public string Description { get; protected set; }
	public bool IsCompleted { get; protected set; }
	public bool IsStarted { get; protected set; }
	public static List<Mission> MissionContains = new List<Mission>();
	public static int CurrentMission;
	
	public override void _Process(double delta) {
		
	}
}
