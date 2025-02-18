using Godot;
using System;

public partial class VBoxContainer : Godot.VBoxContainer {
	public override void _Ready() {
		Button resume = GetNodeOrNull<Button>("Resume");
		Button settings = GetNodeOrNull<Button>("Settings");
		Button saveAndQuit = GetNodeOrNull<Button>("Save&Quit");
		
		resume.Connect("pressed", Callable.From(OnRusumeButtonPressed));
		settings.Connect("pressed", Callable.From(OnsettingsButtonPressed));
		saveAndQuit.Connect("pressed", Callable.From(OnSaveAndQuitButtonPresssed));
	}
	
	public override void _Input(InputEvent @event) {
		if (Input.IsActionJustPressed("pause")) {
			Resume();
		}
	}
	
	public void OnRusumeButtonPressed() {
		Resume();
	}
	public void OnsettingsButtonPressed() {
		GetTree().ChangeSceneToFile("res://scenes/ui/menu.tscn");
		Input.SetMouseMode(Input.MouseModeEnum.Visible);
	}
	public void OnSaveAndQuitButtonPresssed() {
		var config = new ConfigFile();
		
		CharacterBody3D _playerCharacterBody = GameManager.Instance.Player.GetNode<CharacterBody3D>("CharacterBody");
		MeshInstance3D _playerCamera = _playerCharacterBody.GetNode<MeshInstance3D>("Head");
		CharacterBody3D _enemy = GameManager.Instance.Enemy;
		
		config.SetValue("Player", "player_position_x", _playerCharacterBody.GlobalPosition.X);
		config.SetValue("Player", "player_position_y", _playerCharacterBody.GlobalPosition.Y);
		config.SetValue("Player", "player_position_z", _playerCharacterBody.GlobalPosition.Z);
		config.SetValue("Player", "player_head_rotation_x", _playerCharacterBody.GlobalRotation.X);
		config.SetValue("Player", "player_head_rotation_y", _playerCharacterBody.GlobalRotation.Y);
		config.SetValue("Player", "player_head_rotation_z", _playerCharacterBody.GlobalRotation.Z);
		config.SetValue("Enemy", "enemy_position_x", _enemy.GlobalPosition.X);
		config.SetValue("Enemy", "enemy_position_y", _enemy.GlobalPosition.Y);
		config.SetValue("Enemy", "enemy_position_z", _enemy.GlobalPosition.Z);
		config.SetValue("Enemy", "enemy_states", GameManager.Instance.SaveEnemyState);
		config.SetValue("Scene", "current_scene", GetTree().CurrentScene.Name);
		config.SetValue("CutSceneSeen", "is_begining_cut_scene_seen", GameManager.Instance.IsBeginingCutSceneSeen);
		config.SetValue("CutSceneSeen", "is_awakening_cut_scene_seen", GameManager.Instance.IsAwakeningCutSceneSeen);
		
		var dir = DirAccess.Open("user://");
		if (!dir.DirExists("user://configs")) {
			dir.MakeDir("user://configs");
		}
		config.Save("user://configs/save.cfg");
		
		GD.Print("Игра сохранена");
		
		GetTree().Quit();
	}
	private void Resume() {
		Engine.TimeScale = 1;
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
	}
}
