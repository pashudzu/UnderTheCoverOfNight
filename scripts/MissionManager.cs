using Godot;
using System;
using System.Linq;

public partial class MissionManager : Node
{
	private Label _missionName;
	private Label _missionDescription;
	private const string EmptyMissionText = "";
	public override void _Ready() {
		_missionName = GameManager.Instance.MissionNameLabel;
		_missionDescription = GameManager.Instance.MissionDescriptionLabel;
	}
	public override void _Process(double delta) {
		if (_missionName.Text == EmptyMissionText || _missionDescription.Text == EmptyMissionText) {
			SetMissionText(0);
		}
	}
	private void SetMissionText(int _key) {
		if (!Mission.MissionContains.Any()) {
			GD.PrintErr("MissionContains не имеет значенией.");
		} else {
			_missionName.Text = Mission.MissionContains[_key].Name;
			_missionDescription.Text = Mission.MissionContains[_key].Description;
		}
	}
}
