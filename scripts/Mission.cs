using Godot;
using System;
using System.Collections.Generic;

public abstract partial class Mission : Node
{
	public string Name { get; protected set; }
	public string Description { get; protected set; }
	public bool IsCompleted { get; protected set; }
	public bool IsStarted { get; protected set; }
	public static Dictionary<string, Mission> MissionContains = new Dictionary<string, Mission>();
	
	public abstract void StartMission();
	public abstract void CompleteMission();
	
	public override void _Ready() {
		StartMission();
	}
	public override void _Process(double delta) {}
}
