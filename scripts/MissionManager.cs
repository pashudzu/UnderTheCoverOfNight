using Godot;
using System;

public partial class MissionManager : Node
{
	private Label missionLabel;
	public override void _Ready() {
		missionLabel = GameManager.Instance.MissionLabel;
	}
	public override void _Procces(double delta) {
		
	}
	private void ChangeMissionLabel() {
		
	}
}
