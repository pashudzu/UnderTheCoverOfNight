using Godot;
using System;

public abstract class Mission : Node
{
	public string Name { get; protected set; }
	public string Description { get; protected set; }
	public bool IsCompleted { get; protected set; }
	public bool IsStarted { get; private set; }
	public static Dictionary<string, Mission> MissionContains = new Dictionary<string, Mission>();
	
	public abstract void StartMission();
	public abstract void CompleteMission();
	
	public override void _Ready() {
		StartMission();
	}
}
