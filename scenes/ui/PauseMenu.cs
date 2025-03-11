using Godot;
using System;

public partial class PauseMenu : Control
{
	public override void _Ready() {
		GameManager.Instance.MissionLabel = GetNode("ColorRect/MissionLabel");
	}
}
