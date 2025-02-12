using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }
	public Node3D World { get; set; }
	public Node3D Player { get; set; }
	public CharacterBody3D PlayerCharacterBody { get; set; }
	public Vector3 SavedPlayerPosition { get; set; } = Vector3.Zero;
	public Sprite2D PressESprite { get; set; }
	public bool IsDialogueGoing = false;
	public bool IsEventAnimationIsOngoing = false;
	public string LeftHandChild;
	public string RightHandChild;
	public bool CarIsFueled;
	public string DownloadableScene;
	public bool playerCaughtUp = false;
	public bool IsEndHappy;
	public bool IsBeginingCutSceneSeen = false;
	
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
