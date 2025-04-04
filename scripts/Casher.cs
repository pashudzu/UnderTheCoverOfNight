using Godot;
using System;

public partial class Casher : Skeleton3D
{
	public override void _Ready() {
		GameManager.Instance.CasherAnimationPlayer = GetNode<AnimationPlayer>("CasherAnimationPlayer");
		GD.Print("Анимация кассира записана в GameManager.");
	}
}
