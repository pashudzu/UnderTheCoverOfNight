using Godot;
using System;

public partial class RunAwayMission : Mission
{
	public override void _Ready () {
		base._Ready();
		Name = "Беги от монстра!!!";
		Description = "";
		Id = 2;
		MissionContains.Add(this);
	}
	public void StartMission() {
		IsStarted = true;
	}
	public void CompleteMission() {
		if (!GameManager.Instance.IsPlayerNearMonster && GetTree().CurrentScene.Name == "home_scene") {
			IsCompleted = true;
		}
	}
	public override void _Process(double delta) {
		CompleteMission();
	}
}
