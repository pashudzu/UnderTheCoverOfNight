using Godot;
using System;

public partial class RunAwayMission : Mission
{
	public override void _Ready () {
		base._Ready();
		Name = "Беги от монстра!!!";
		Description = "Вряд ли если\nон тебя догонит\nбудет что-то хорошее...";
		Id = 2;
		MissionContains.Add(this);
		GD.Print("RunAwayMission был инициализирыван.");
	}
	public override void CompleteMission() {
		//GD.Print($"{!GameManager.Instance.IsPlayerNearMonster}, {GetTree().CurrentScene.Name == "home_scene"}");
		if (!GameManager.Instance.IsPlayerNearMonster && GetTree().CurrentScene.Name == "home_scene") {
			IsCompleted = true;
			GD.Print($"Миссия RunAwayMission выполнена!");
		}
	}
}
