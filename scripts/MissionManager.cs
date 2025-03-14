using Godot;
using System;
using System.Linq;

public partial class MissionManager : Node
{
	private Label missionLabel;
	public override void _Ready() {
		missionLabel = GameManager.Instance.MissionLabel;
	}
	public override void _Process(double delta) {
		if (GameManager.Instance.MissionLabel == null) {
			GameManager.Instance.MissionLabel.Text = Mission.MissionContains.First().Key;
		}
	}
	private void ChangeMissionLabel() {
		missionLabel = GameManager.Instance.MissionLabel;
	}
}
