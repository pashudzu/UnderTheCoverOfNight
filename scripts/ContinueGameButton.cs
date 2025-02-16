using Godot;
using System;

public partial class ContinueGameButton : Button
{
	private Vector3 _newPosition;
	private Vector3 _newRotation;
	private string _currentScene;
	
	public override void _Ready() 
	{
		Connect("pressed", Callable.From(OnContinueGameButtonPressed));
	}
	
	public void OnContinueGameButtonPressed() {
		GettingConfigData();
		SetPastGameProgress();
		ChangeSceneToSaved();
	}
	private void ChangeSceneToSaved() {
		string _scenePath = "res://scenes/"+ _currentScene +".tscn";
		GD.Print(_scenePath);
		GameManager.Instance.DownloadableScene = _scenePath;
		PackedScene loadScene = (PackedScene)ResourceLoader.Load("res://scenes/loading_scene.tscn");
		GetTree().ChangeSceneToPacked(loadScene);
	}
	private void GettingConfigData() {
		var config = new ConfigFile();
		
		Error err = config.Load("user://configs/save.cfg");
		
		if (err != Error.Ok) {
			GD.Print("Ошибка загрузки конфига");
			return;
		}
		
		_newPosition.X = (float)config.GetValue("Player", "player_position_x");
		_newPosition.Y = (float)config.GetValue("Player", "player_position_y");
		_newPosition.Z = (float)config.GetValue("Player", "player_position_z");
		_newRotation.X = (float)config.GetValue("Player", "player_head_rotation_x");
		_newRotation.Y = (float)config.GetValue("Player", "player_head_rotation_y");
		_newRotation.Z = (float)config.GetValue("Player", "player_head_rotation_z");
		_currentScene = (string)config.GetValue("Scene", "current_scene");
		GameManager.Instance.IsBeginingCutSceneSeen = (bool)config.GetValue("CutSceneSeen", "is_begining_cut_scene_seen");
		GameManager.Instance.IsAwakeningCutSceneSeen = (bool)config.GetValue("CutSceneSeen","is_awakening_cut_scene_seen");
	}
	
	private void SetPastGameProgress() {
		GameManager.Instance.SavedSceneName = _currentScene;
		GameManager.Instance.SavedPlayerPosition = _newPosition;
		GameManager.Instance.SavedPlayerRotation = _newRotation;
		GD.Print($"После импорта данных из save.cfg позиция игрока: {GameManager.Instance.SavedPlayerPosition}");
	}
}
