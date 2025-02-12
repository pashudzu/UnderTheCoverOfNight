using Godot;
using System;

public partial class SaveChoice : Control
{
	private Button _saveSlotButton;
	private Godot.Collections.Dictionary _gameData = new Godot.Collections.Dictionary();
	private float _playerPositionX;
	private float _playerPositionY;
	private float _playerPositionZ;
	private string _currentScene;
	
	public override void _Ready()
	{
		_saveSlotButton = GetNode<Button>("ColorRect/SaveSlotButton");
		_saveSlotButton.Connect("pressed", Callable.From(OnSaveSlotButtonPressed));
	}
	
	public void OnSaveSlotButtonPressed() {
		GettingConfigData();
		SetPastGameProgress();
		ChangeScene();
	}
	private void ChangeScene() {
		string scenePath = "res://scenes/"+ _currentScene +".tscn";
		GD.Print(scenePath);
		GameManager.Instance.DownloadableScene = scenePath;
		PackedScene loadScene = (PackedScene)ResourceLoader.Load("res://scenes/loading_scene.tscn");
		GetTree().ChangeSceneToPacked(loadScene);
	}
	private void GettingConfigData() {
		var config = new ConfigFile();
		
		Error err = config.Load("res://configs/save.cfg");
		
		if (err != Error.Ok) {
			GD.Print("Ошибка загрузки конфига");
			return;
		}
		
		foreach (String Player in config.GetSections()) {
			_playerPositionX = (float)config.GetValue("Player", "player_position_x");
			_playerPositionY = (float)config.GetValue("Player", "player_position_y");
			_playerPositionZ = (float)config.GetValue("Player", "player_position_z");
		}
		foreach (String Scene in config.GetSections()) {
			_currentScene = (string)config.GetValue("Scene", "current_scene");
		}
		foreach (String CutSceneFlag in config.GetSections()) {
			GameManager.Instance.IsBeginingCutSceneSeen = (bool)config.GetValue("CutSceneFlag", "is_begining_cut_scene_seen");
		}
	}
	
	private void SetPastGameProgress() {
		Vector3 newPosition;
		newPosition.X = _playerPositionX;
		newPosition.Y = _playerPositionY;
		newPosition.Z = _playerPositionZ;
		GameManager.Instance.SavedPlayerPosition = newPosition;
		GD.Print($"После импорта данных из save.cfg позиция игрока: {GameManager.Instance.SavedPlayerPosition}");
	}
}
