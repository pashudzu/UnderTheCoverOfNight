using Godot;
using System;

public partial class MissionName : Label
{
	public override void _Ready() {
		GameManager.Instance.MissionNameLabel = this;
	}
}
