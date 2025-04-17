using Godot;
using System;

public partial class PopUpAd : TextureRect
{
	public override void _Ready() {
		GameManager.Instance.PopUpMissionAd = this;
		GD.Print("PopUpAd был инициализирован.");
	}
	public override void _ExitTree() {
		GameManager.Instance.PopUpMissionAd = null;
	}
}
