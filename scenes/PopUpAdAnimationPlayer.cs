using Godot;
using System;

public partial class PopUpAdAnimationPlayer : AnimationPlayer
{
	public override void _Ready() {
		GameManager.Instance.PopUpMissionAdAnimaion = this;
		GD.Print("PopUpAdAnimationPlayer был инициализирован.");
	}
	public override void _ExitTree() {
		GameManager.Instance.PopUpMissionAdAnimaion = null;
	}
}
