using Godot;
using System;

public partial class GoSleepMission : Mission
{
	public override void _Ready() {
		base._Ready();
		Name = "Пора спать";
		Description = "Ты уже устал,\nиди-ка поспи.";
		Id = 1;
		MissionContains.Add(this);
	}
	public void StartMission() {
		IsStarted = true;
	}
	public void CompleteMission() {
		if (GameManager.Instance.IsWentSleep) {
			IsCompleted = true;
		}
	}
	public override void _Process(double delta) {
		CompleteMission();
	}
}
