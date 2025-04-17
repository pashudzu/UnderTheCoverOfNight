using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class ContinueGameButton : TextureButton
{
	private Vector3 _newPlayerPosition;
	private Vector3 _newPlayerRotation;
	private Vector3 _newEnemyPosition;
	private List<string> _savedSlots = new List<string>();
	private Dictionary<int, string> _savedItems = new Dictionary<int, string>();
	private string _enemyState;
	private string _currentScene;
	private const int CountOfSlots = 4;
	private const int CountOfItems = 6;
	
	public override void _Ready() 
	{
		Connect("pressed", Callable.From(OnContinueGameButtonPressed));
	}
	
	public void OnContinueGameButtonPressed() {
		string _cofigFilePath = ProjectSettings.GlobalizePath($"user://configs/save{GameManager.Instance.SlotNoumber}.cfg");
		
		if (File.Exists(_cofigFilePath)) {
			GettingConfigData();
			SetPastGameProgress();
			ChangeSceneToSaved();
		} else {
			StartNewGame();
		}
	}
	private void ChangeSceneToSaved() {
		var config = new ConfigFile();
		GetConfig(config);
		
		string _scenePath = "res://scenes/"+ _currentScene +".tscn";
		GD.Print(_scenePath);
		GameManager.Instance.DownloadableScene = _scenePath;
		PackedScene loadScene = (PackedScene)ResourceLoader.Load("res://scenes/loading_scene.tscn");
		GetTree().ChangeSceneToPacked(loadScene);
	}
	private void GettingConfigData() {
		var config = new ConfigFile();
		GetConfig(config);
		
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
		for (int i = 0; i < CountOfSlots; i++) {
			_savedSlots.Add((string)config.GetValue("Inventory", $"inventory_slot_texture_path_{i}"));
		}
		for(int i = 0; i < CountOfItems; i++) {
			_savedItems[i] = (string)config.GetValue("Inventory", $"inventory_item_{i}");
		}
		Mission.CurrentMission = (int)config.GetValue("Missions", "current_mission");
		GameManager.Instance.IsBeginingCutSceneSeen = (bool)config.GetValue("CutSceneSeen", "is_begining_cut_scene_seen");
		GameManager.Instance.IsAwakeningCutSceneSeen = (bool)config.GetValue("CutSceneSeen","is_awakening_cut_scene_seen");
	}
	
	private void SetPastGameProgress() {
		var config = new ConfigFile();
		GetConfig(config);
		
		GameManager.Instance.WasGameSaved = true;
		GameManager.Instance.SavedSceneName = _currentScene;
		GameManager.Instance.SavedPlayerPosition = _newPlayerPosition;
		GameManager.Instance.SavedPlayerRotation = _newPlayerRotation;
		GameManager.Instance.SavedEnemyPosition = _newEnemyPosition;
		GameManager.Instance.SaveEnemyState = _enemyState;
		GameManager.Instance.SavedSlots = _savedSlots;
		GameManager.Instance.SavedItems = _savedItems;
	}
	private void GetConfig(ConfigFile config) {
		Error err = config.Load(ProjectSettings.GlobalizePath($"user://configs/save{GameManager.Instance.SlotNoumber}.cfg"));
		
		if (err != Error.Ok) {
			GD.Print("Ошибка загрузки конфига");
			return;
		}
	}
	private void StartNewGame() {
		GameManager.Instance.DownloadableScene = ProjectSettings.GlobalizePath("res://scenes/begining.tscn");
		PackedScene _laodScene = (PackedScene)ResourceLoader.Load(ProjectSettings.GlobalizePath("res://scenes/loading_scene.tscn"));
		GetTree().ChangeSceneToPacked(_laodScene);
	}
}
