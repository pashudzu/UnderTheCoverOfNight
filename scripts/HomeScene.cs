using Godot;
using System;

public partial class HomeScene : Node3D
{
	private Node3D _player;
	
	public override void _Ready()
	{
		_player = GameManager.Instance.Player;
		GameManager.Instance.World = this;
	}

	public override void _PhysicsProcess(double delta) {
		if (_player != null) {
			GetTree().CallGroup("Enemy", "UpdateTargetLocation", _player.GlobalTransform.Origin);
		} else {
			GD.PrintErr("Игрок не инициализирован в игровом менеджере.");
			return;
		}
	}
}
