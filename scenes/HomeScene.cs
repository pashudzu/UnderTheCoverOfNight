using Godot;
using System;

public partial class HomeScene : Node3D
{
	private Node3D player;
	
	public override void _Ready()
	{
		player = GameManager.Instance.Player;
		GameManager.Instance.World = this;
	}

	public override void _PhysicsProcess(double delta) {
		if (player != null) {
			GetTree().CallGroup("Enemy", "UpdateTargetLocation", player.GlobalTransform.Origin);
		} else {
			GD.PrintErr("Игрок не инициализирован в игровом менеджере.");
			return;
		}
	}
}
