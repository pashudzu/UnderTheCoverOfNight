using Godot;
using System;

public partial class MapInHand : Node3D
{
	private Node3D _world;
	private SubViewportContainer _miniMap;
	private SubViewportContainer _bigMap;
	
	public override void _Ready()
	{
		_world = GameManager.Instance.World;
		_miniMap = _world.GetNode<SubViewportContainer>("UI/SubViewportContainer1");
		_bigMap = _world.GetNode<SubViewportContainer>("UI/SubViewportContainer2");
		_bigMap.Hide();
		_miniMap.Show();
	}
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("big_map")) {
			if (_miniMap.Visible == true) {
				_miniMap.Hide();
				_bigMap.Show();
			} else {
				_miniMap.Show();
				_bigMap.Hide();
			}
		}
	}
	public override void _ExitTree() {
		_miniMap.Hide();
	}
}
