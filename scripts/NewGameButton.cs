using Godot;
using System;
using System.IO;

public partial class NewGameButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("pressed", Callable.From(OnNewGameButtonPressed));
	}
	
	public void OnNewGameButtonPressed() {
		DeleteExistConfig();
		GameManager.Instance.DownloadableScene = "res://scenes/begining.tscn";
		PackedScene _laodScene = (PackedScene)ResourceLoader.Load("res://scenes/loading_scene.tscn");
		GetTree().ChangeSceneToPacked(_laodScene);
	}
	public void DeleteExistConfig() {
		string _cofigFilePath = ProjectSettings.GlobalizePath($"user://configs/save{GameManager.Instance.SlotNoumber}.cfg");
		
		if (File.Exists(_cofigFilePath)) {
			File.Delete(_cofigFilePath);
			GD.Print($"Конфиг {GameManager.Instance.SlotNoumber} был успешно удалён.");
		}
	}
}
