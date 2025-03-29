using Godot;
using System;
using System.Linq;

public partial class MissionManager : Node
{
	private TextureRect _popUpAd;
	private Label _popUpLabel;
	private Label _missionName;
	private Label _missionDescription;
	private AnimationPlayer animationPlayer;
	private const string EmptyMissionText = "";
	public override void _Ready() {
		_missionName = GameManager.Instance.MissionNameLabel;
		_missionDescription = GameManager.Instance.MissionDescriptionLabel;
		_popUpAd = GetNode<TextureRect>("PopUpAd");
		_popUpLabel = GetNode<Label>("PopUpAd/PopUpLabel");
		animationPlayer = GetNode<AnimationPlayer>("PopUpAdAnimationPlayer");
	}
	public override void _Process(double delta) {
		if (_missionName.Text == EmptyMissionText && _missionDescription.Text == EmptyMissionText) {
			Mission.CurrentMission = 0;
			SetMissionText(Mission.CurrentMission);
			GD.Print("Была поставленна первая миссия.");
		}
		if (Mission.MissionContains[Mission.CurrentMission].IsCompleted && Mission.MissionContains.Count - 1 > Mission.CurrentMission) {
			GD.Print($"Миссия {Mission.MissionContains[Mission.CurrentMission].Name} была поставлена игроку.");
			Mission.CurrentMission++;
			SetMissionText(Mission.CurrentMission);
		}
	}
	private void SetMissionText(int _key) {
		if (!Mission.MissionContains.Any()) {
			GD.PrintErr("MissionContains не имеет значенией.");
			return;
		} 
		if (_key >= 0 && _key < Mission.MissionContains.Count) {
			_missionName.Text = Mission.MissionContains[_key].Name;
			_missionDescription.Text = Mission.MissionContains[_key].Description;
			_popUpLabel.Text = $"Новая миссия:\n{Mission.MissionContains[_key].Name} \n{Mission.MissionContains[_key].Description}";
			animationPlayer.Play("advertisement");
		} else {
			GD.Print("Все миссии уже выполнены, или не проинициализированны.");
		}
	}
}
