using Godot;
using System;
using System.Text.Json;
using System.Collections.Generic;

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
		
		var _gameManager = GameManager.Instance;
		CharacterBody3D _playerCharacterBody = _gameManager.Player.GetNode<CharacterBody3D>("CharacterBody");
		MeshInstance3D _playerCamera = _playerCharacterBody.GetNode<MeshInstance3D>("Head");
		CharacterBody3D _enemy = _gameManager.Enemy;
		List<string> _inventorySlotTexturePath = new List<string>();
		
		config.SetValue("Player", "player_position_x", _playerCharacterBody.GlobalPosition.X);
		config.SetValue("Player", "player_position_y", _playerCharacterBody.GlobalPosition.Y);
		config.SetValue("Player", "player_position_z", _playerCharacterBody.GlobalPosition.Z);
		config.SetValue("Player", "player_head_rotation_x", _playerCharacterBody.GlobalRotation.X);
		config.SetValue("Player", "player_head_rotation_y", _playerCharacterBody.GlobalRotation.Y);
		config.SetValue("Player", "player_head_rotation_z", _playerCharacterBody.GlobalRotation.Z);
		config.SetValue("Enemy", "enemy_position_x", _enemy.GlobalPosition.X);
		config.SetValue("Enemy", "enemy_position_y", _enemy.GlobalPosition.Y);
		config.SetValue("Enemy", "enemy_position_z", _enemy.GlobalPosition.Z);
		config.SetValue("Enemy", "enemy_states", _gameManager.SaveEnemyState);
		config.SetValue("Scene", "current_scene", GetTree().CurrentScene.Name);
		config.SetValue("CutSceneSeen", "is_begining_cut_scene_seen", _gameManager.IsBeginingCutSceneSeen);
		config.SetValue("CutSceneSeen", "is_awakening_cut_scene_seen", _gameManager.IsAwakeningCutSceneSeen);
		if (_gameManager.SavedSlots.Count >= 4) {
			for (int i = 0; i < _gameManager.SavedSlots.Count; i++) {
				config.SetValue("Inventory", $"inventory_slot_texture_path_{i}", _gameManager.SavedSlots[i]);
				GD.Print($"В конфиг добавлен inventory_slot_texture_path_{i} со значением: {_gameManager.SavedSlots[i]}");
			}
		} else {
			GD.PrintErr($"Не достаточно данных в листе _gameManager.SavedSlots. Необходимо 4+ значений, но их {_gameManager.SavedSlots.Count}");
		}
		if (_gameManager.SavedItems.Count >= 6) {
			for (int i = 0; i < _gameManager.SavedItems.Count; i++) {
				config.SetValue("Inventory", $"inventory_item_{i}", _gameManager.SavedItems[i]);
				GD.Print($"В конфиг добавлен inventory_item_{i} со значением: {_gameManager.SavedItems[i]}");
			}
		} else {
			GD.PrintErr($"Не достаточно данных в листе _gameManager.SavedItems. Необходимо 6+ значений, но их {_gameManager.SavedItems.Count}");
		}
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
