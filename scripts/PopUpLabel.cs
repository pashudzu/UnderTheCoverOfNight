using Godot;
using System;

public partial class PopUpLabel : Label
{
	public override void _Ready() {
		GameManager.Instance.PopUpMissionLabel = this;
		GD.Print("PopUpLabel был инициализирован.");
	}
	public override void _ExitTree() {
		GameManager.Instance.PopUpMissionLabel = null;
	}
}
