using Godot;
using System;
using System.IO;

public partial class NewGameButton : TextureButton
{
	private ColorRect _warningMessage;
	private Button _sureButton;
	private Button _unsureButton;
	
	public override void _Ready()
	{
		_warningMessage = GetParent().GetParent().GetNode<ColorRect>("WarningMessage");
		HBoxContainer _buttonsContainer = _warningMessage.GetNode<HBoxContainer>("HBoxContainer");
		_sureButton = _buttonsContainer.GetNode<Button>("SureButton");
		_unsureButton = _buttonsContainer.GetNode<Button>("UnsureButton");
		Connect("pressed", Callable.From(OnNewGameButtonPressed));
		_sureButton.Connect("pressed", Callable.From(OnSureButtonPressed));
		_unsureButton.Connect("pressed", Callable.From(OnUnsureButtonPressed));
	}
	
	public void OnNewGameButtonPressed() {
		string _cofigFilePath = ProjectSettings.GlobalizePath($"user://configs/save{GameManager.Instance.SlotNoumber}.cfg");
		
		if (File.Exists(_cofigFilePath)) {
			ShowWarning();
		} else {
			StartGame();
		}
	}
	public void ShowWarning() {
		GetParent<Control>().Visible = false;
		_warningMessage.Visible = true;
	}
	public void HideWarning() {
		GetParent<Control>().Visible = true;
		_warningMessage.Visible = false;
	}
	public void DeleteConfig() {
		string _cofigFilePath = ProjectSettings.GlobalizePath($"user://configs/save{GameManager.Instance.SlotNoumber}.cfg");
		
		if (File.Exists(_cofigFilePath)) {
			File.Delete(_cofigFilePath);
			GD.Print($"Конфиг {GameManager.Instance.SlotNoumber} был успешно удалён.");
		}
	}
	private void StartGame() {
		GameManager.Instance.DownloadableScene = "res://scenes/begining.tscn";
		PackedScene _laodScene = (PackedScene)ResourceLoader.Load("res://scenes/loading_scene.tscn");
		GetTree().ChangeSceneToPacked(_laodScene);
	}
	private void OnSureButtonPressed() {
		DeleteConfig();
		StartGame();
	}
	private void OnUnsureButtonPressed() {
		HideWarning();
	}
}
