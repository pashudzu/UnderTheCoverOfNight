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
<<<<<<< Updated upstream
		
		config.SetValue("Player", "player_position_x", GameManager.Instance.Player.GlobalPosition.X);
		config.SetValue("Player", "player_position_y", GameManager.Instance.Player.GlobalPosition.Y);
=======
		CharacterBody3D playerCharacterBody = GameManager.Instance.Player.GetNode<CharacterBody3D>("CharacterBody");
		config.SetValue("Player", "player_position_x", playerCharacterBody.GlobalPosition.X);
		config.SetValue("Player", "player_position_y", playerCharacterBody.GlobalPosition.Y);
		config.SetValue("Player", "player_position_z", playerCharacterBody.GlobalPosition.Z);
>>>>>>> Stashed changes
		config.SetValue("Scene", "current_scene", GetTree().CurrentScene.Name);
		
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
