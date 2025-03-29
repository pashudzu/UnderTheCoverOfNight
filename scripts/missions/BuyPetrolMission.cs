using Godot;
using System;

public partial class BuyPetrolMission : Mission
{
	public override void _Ready() {
		base._Ready();
		Id = 3;
		Name = "Купи бензин";
		Description = "В деревне есть заправка. \nБензин будет нужен, \nчтобы уехать из деревни.";
		MissionContains.Add(this);
	}
	public void StartMission() {
		IsStarted = true;
	}
	public void CompleteMission() {
		if (GameManager.Instance.IsPetrolGotten) {
			IsCompleted = true;
		}
	}
	public override void _Process(double delta) {
		CompleteMission();
	}
}
