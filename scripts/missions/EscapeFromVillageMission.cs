using Godot;
using System;

public partial class EscapeFromVillageMission : Mission
{
	public override void _Ready() {
		base._Ready();
		Id = 4;
		Name = "Побег";
		Description = "Найди машину, \nзаправив её, \nпосле уезжай из деревни.";
		MissionContains.Add(this);
	}
	public override void CompleteMission() {
		if (GameManager.Instance.IsEndHappy) {
			IsCompleted = true;
			GD.Print($"Миссия EscapeFromVillageMission выполнена!");
		}
	}
}
