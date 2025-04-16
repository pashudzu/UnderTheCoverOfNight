using Godot;
using System;

public partial class LightStoveMission : Mission
{
	public override void _Ready() {
		base._Ready();
		Name = "Растопи печь";
		Description = "Дома очень холодно";
		Id = 0;
		MissionContains.Add(this);
		GD.Print("LightStoveMission был инициализирыван.");
	}
	public override void CompleteMission() {
		if (GameManager.Instance.IsStoveLighted) {
			IsCompleted = true;
			GD.Print($"Миссия LightStoveMission выполнена!");
		}
	}
}
