using Godot;
using System;

public partial class GoSleepMission : Mission
{
	public override void _Ready() {
		base._Ready();
		Name = "Пора спать";
		Description = "На улице потемнело,\nиди-ка, поспи.";
		Id = 1;
		MissionContains.Add(this);
	}
	public override void CompleteMission() {
		if (GameManager.Instance.IsWentSleep) {
			IsCompleted = true;
		}
	}
}
