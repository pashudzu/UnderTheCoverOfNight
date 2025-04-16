using Godot;
using System;

public partial class PopUpLabel : Label
{
	public override void _Ready() {
		GameManager.Instance.PopUpMissionLabel = this;
	}
	public override void _ExitTree() {
		GameManager.Instance.PopUpMissionLabel = null;
	}
}
