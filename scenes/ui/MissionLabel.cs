using Godot;
using System;

public partial class MissionLabel : Label
{
	public override void _Ready() {
		GameManager.Instance.MissionLabel = this;
	}
}
