using Godot;
using System;

public partial class ContinueGameButton : Button
{
	private Vector3 _newPlayerPosition;
	private Vector3 _newPlayerRotation;
	private Vector3 _newEnemyPosition;
	private string _enemyState;
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
		
		_newPlayerPosition.X = (float)config.GetValue("Player", "player_position_x");
		_newPlayerPosition.Y = (float)config.GetValue("Player", "player_position_y");
		_newPlayerPosition.Z = (float)config.GetValue("Player", "player_position_z");
		_newPlayerRotation.X = (float)config.GetValue("Player", "player_head_rotation_x");
		_newPlayerRotation.Y = (float)config.GetValue("Player", "player_head_rotation_y");
		_newPlayerRotation.Z = (float)config.GetValue("Player", "player_head_rotation_z");
		_newEnemyPosition.X = (float)config.GetValue("Enemy", "enemy_position_x");
		_newEnemyPosition.Y = (float)config.GetValue("Enemy", "enemy_position_y");
		_newEnemyPosition.Z = (float)config.GetValue("Enemy", "enemy_position_z");
		_enemyState = (string)config.GetValue("Enemy", "enemy_states");
		_currentScene = (string)config.GetValue("Scene", "current_scene");
		
		GameManager.Instance.IsBeginingCutSceneSeen = (bool)config.GetValue("CutSceneSeen", "is_begining_cut_scene_seen");
		GameManager.Instance.IsAwakeningCutSceneSeen = (bool)config.GetValue("CutSceneSeen","is_awakening_cut_scene_seen");
	}
	
	private void SetPastGameProgress() {
		GameManager.Instance.SavedSceneName = _currentScene;
		GameManager.Instance.SavedPlayerPosition = _newPlayerPosition;
		GameManager.Instance.SavedPlayerRotation = _newPlayerRotation;
		GameManager.Instance.SavedEnemyPosition = _newEnemyPosition;
		GameManager.Instance.SaveEnemyState = _enemyState;
		GD.Print($"После импорта данных из save.cfg позиция игрока: {GameManager.Instance.SavedPlayerPosition}");
	}
}
