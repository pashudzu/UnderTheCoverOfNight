using Godot;
using System;
using System.Collections.Generic;

public abstract partial class Mission : Node
{
	public byte Id { get; protected set; }
	public string Name { get; protected set; }
	public string Description { get; protected set; }
	public bool IsCompleted { get; protected set; }
	public bool IsStarted { get; protected set; }
	public static List<Mission> MissionContains = new List<Mission>();
	public static int CurrentMission = 0;
	
	public override void _Process(double delta) {
		CompleteMission();
	}
	public abstract void CompleteMission();
}
