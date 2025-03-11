using Godot;
using System;

public partial class LightStoveMission : Mission
{
	public override void _Ready() {
		base._Ready();
		Name = "Растопи печь";
		Description = "Дома очень холодно. Растопи печь, чтобы дома стало теплее.";
		MissionContains.Add(Name, this);
	}
	public override void StartMission() {
		IsStarted = true;
	}
	public override void CompleteMission() {
		if (GameManager.Instance.IsStoveLighted) {
			IsCompleted = true;
		}
	}
	public override void _Process() {
		CompleteMission();
	}
}
