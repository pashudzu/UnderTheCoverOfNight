using Godot;
using System;

public partial class PopUpAd : TextureRect
{
	public override void _Ready() {
		GameManager.Instance.PopUpMissionAd = this;
	}
	public override void _ExitTree() {
		GameManager.Instance.PopUpMissionAd = null;
	}
}
