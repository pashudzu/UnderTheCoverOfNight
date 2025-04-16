using Godot;
using System;
using System.Linq;

public partial class MissionManager : Node
{
	public static MissionManager Instance { get; private set; }
	private bool _IsMissionNotShown;
	private TextureRect _popUpAd;
	private Label _popUpLabel;
	private Label _missionName;
	private Label _missionDescription;
	private AnimationPlayer animationPlayer;
	private const string EmptyMissionText = "";
	
	public override void _Ready() {
		if (Instance != null) {
			QueueFree();
		}
		Instance = this;
		GD.Print("MissionManager инициализирован");
	}
	
	private void InitPopUpThings() {
		_missionName = GameManager.Instance.MissionNameLabel;
		_missionDescription = GameManager.Instance.MissionDescriptionLabel;
		_popUpAd = GameManager.Instance.PopUpMissionAd;//GetNode<TextureRect>("PopUpAd");
		_popUpLabel = GameManager.Instance.PopUpMissionLabel;//GetNode<Label>("PopUpAd/PopUpLabel");
		animationPlayer = GameManager.Instance.PopUpMissionAdAnimaion;//GetNode<AnimationPlayer>("PopUpAdAnimationPlayer");
	}
	
	public override void _Process(double delta) {
		if (GameManager.Instance.MissionNameLabel == null || GameManager.Instance.MissionDescriptionLabel == null) {
			return;
		}
		InitPopUpThings();
		SetNewMission();
		if (_IsMissionNotShown) {
			ShowUnshownAd(Mission.CurrentMission);
		}
	}
	
	private void SetCurrentMission() {
		SetMissionTextAndPopUpAd(Mission.CurrentMission);
	}
	
	private void SetNewMission() {
		if (Mission.MissionContains[Mission.CurrentMission].IsCompleted && Mission.MissionContains.Count - 1 > Mission.CurrentMission) {
			GD.Print($"Миссия {Mission.MissionContains[Mission.CurrentMission].Name} была поставлена игроку.");
			Mission.CurrentMission++;
			SetCurrentMission();
		} else if (Mission.CurrentMission == 0) {
			SetCurrentMission();
		}
	}
	
	private void ShowUnshownAd(int _key) {
		if (!GameManager.Instance.IsEventAnimationIsOngoing) {
			_popUpAd.Show();
			SetPopUpAd(_key);
			_IsMissionNotShown = false;
		} else {
			_popUpAd.Hide();
		}
	}
	
	private void SetMissionTextAndPopUpAd(int _key) {
		if (!Mission.MissionContains.Any()) {
			GD.PrintErr("MissionContains не имеет значенией.");
			return;
		} 
		if (GameManager.Instance.IsEventAnimationIsOngoing) {
			SetMissionText(_key);
			_IsMissionNotShown = true;
			GD.Print("В данный момент идёт катсцена, по этой причине оповещение игрока о новой миссии откладывается.");
			return;
		}
		if (_key >= 0 && _key < Mission.MissionContains.Count) {
			SetMissionText(_key);
			SetPopUpAd(_key);
		} else {
			GD.Print("Все миссии уже выполнены, или не проинициализированны.");
		}
	}
	
	private void SetMissionText(int _key) {
		_missionName.Text = Mission.MissionContains[_key].Name;
		_missionDescription.Text = Mission.MissionContains[_key].Description;
	}
	
	private void SetPopUpAd(int _key) {
		_popUpLabel.Text = $"Новая миссия:\n{Mission.MissionContains[_key].Name} \n{Mission.MissionContains[_key].Description}";
		animationPlayer.Play("advertisement");
	}
}
