using Godot;
using System;

public partial class SaveChoice : Control
{
	private Button saveSlotButton;
	Godot.Collections.Dictionary gameData = new Godot.Collections.Dictionary();
	private float playerPositionX;
	private float playerPositionY;
	string currentScene;
	
	public override void _Ready()
	{
		saveSlotButton = GetNode<Button>("ColorRect/SaveSlotButton");
		saveSlotButton.Connect("pressed", Callable.From(OnSaveSlotButtonPressed));
	}
	
	public void OnSaveSlotButtonPressed() {
		GettingConfigData();
		ChangeScene();
		SetPastGameProgress();
	}
	private void ChangeScene() {
		string scenePath = "res://scenes/"+ currentScene +".tscn";
		GD.Print(scenePath);
		GameManager.Instance.DownloadableScene = scenePath;
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
		
<<<<<<< Updated upstream
		foreach (String Player in config.GetSections()) {
			playerPositionX = (float)config.GetValue(Player, "player_position_x");
			playerPositionY = (float)config.GetValue(Player, "player_position_y");
		}
		foreach (String Scene in config.GetSections()) {
			currentScene = (string)config.GetValue(Scene, "current_scene");
		}
=======
		_playerPositionX = (float)config.GetValue("Player", "player_position_x", 0f);
		_playerPositionY = (float)config.GetValue("Player", "player_position_y", 0f);
		_playerPositionZ = (float)config.GetValue("Player", "player_position_z", 0f);
		_currentScene = (string)config.GetValue("Scene", "current_scene");
		GameManager.Instance.IsBeginingCutSceneSeen = (bool)config.GetValue("CutSceneFlag", "is_begining_cut_scene_seen");
>>>>>>> Stashed changes
	}
	
	private void SetPastGameProgress() {
		CharacterBody3D playerCharacterBody = GameManager.Instance.Player.GetNode<CharacterBody3D>("CharacterBody");
		Vector3 newPosition = playerCharacterBody.GlobalPosition;
		newPosition.X = playerPositionX;
		newPosition.Y = playerPositionY;
		playerCharacterBody.GlobalPosition = newPosition;
	}
}
