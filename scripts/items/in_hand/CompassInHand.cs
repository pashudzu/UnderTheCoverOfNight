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
		} else {
			HidePlayerPointOnMap();
		}
	}
	public override void _ExitTree() {
		HidePlayerPointOnMap();
	}
	private void ShowPlayerPointOnMap() {
		CBPlayer.GetNode<SpotLight3D>("LightMarkOfPlayer").Visible = true;
	}
	private void HidePlayerPointOnMap() {
		CBPlayer.GetNode<SpotLight3D>("LightMarkOfPlayer").Visible = false;
	}
	
}
