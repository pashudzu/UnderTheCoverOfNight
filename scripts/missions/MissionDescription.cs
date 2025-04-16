using Godot;
using System;

public partial class MissionDescription : Label
{
	public override void _Ready() {
		GameManager.Instance.MissionDescriptionLabel = this;
	}
}
