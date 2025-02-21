using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }
	public Node3D World { get; set; }
	public Node3D Player { get; set; }
	public CharacterBody3D PlayerCharacterBody { get; set; }
	public CharacterBody3D Enemy;
	public string SavedSceneName;
	public string SaveEnemyState;
	public Dictionary<int, string> SavedItems = new Dictionary<int, string>();
	public List<string> SavedSlots { get; set; } = new List<string>();
	public Vector3 SavedPlayerPosition { get; set; } = Vector3.Zero;
	public Vector3 SavedPlayerRotation { get; set; } = Vector3.Zero;
	public Vector3 SavedEnemyPosition { get; set; } = Vector3.Zero;
	public Sprite2D PressESprite { get; set; }
	public bool IsDialogueGoing = false;
	public bool IsEventAnimationIsOngoing { get; set; } = false;
	public bool IsBeginingCutSceneSeen { get; set; } = false;
	public bool IsAwakeningCutSceneSeen { get; set; } = false;
	public bool IsEndHappy;
	public bool CarIsFueled;
	public bool PlayerCaughtUp { get; set; } = false;
	public bool WasGameSaved = false;
	public string LeftHandChild;
	public string RightHandChild;
	public string DownloadableScene;
	
	public override void _Ready()
	{
		if (Instance != null) {
			GD.PrintErr("GameManager уже существует.");
			QueueFree();
			return;
		}
		Instance = this;
	}
}
