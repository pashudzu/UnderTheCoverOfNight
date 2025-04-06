using Godot;
using System;

public partial class CompassInHand : Node3D
{
	CharacterBody3D CBPlayer;
	public override void _Ready() {
		CBPlayer = GameManager.Instance.PlayerCharacterBody;
	}
	public override void _Process(double delta) {
		if (GameManager.Instance.SavedItems[4] == "Карта" || GameManager.Instance.SavedItems[5] == "Карта") {
			ShowPlayerPointOnMap();
		}
	}
	private void ShowPlayerPointOnMap() {
		
	}
}
