using Godot;
using System;

public partial class SpotLight3d2 : SpotLight3D
{
	[Export] public float batteryCharge = 100.0f;
	private float _maxEnergy = 100.0f;
	private float _minEnergy = 0.0f;
	private float _dischargeRate = 0.1111f;
	private Node3D _player = GameManager.Instance.Player;
	private Sprite2D _chargeSprite;
	private Texture2D _fullCharge, _twoThirdsCharge, _oneThirdCharge, _emptyCharge;
	
	public override void _Ready(){
		_chargeSprite = _player.GetNode<Sprite2D>("CharacterBody/Charge");
		_fullCharge = (Texture2D)GD.Load("res://textures/FullCharge.png");
		_twoThirdsCharge = (Texture2D)GD.Load("res://textures/67%Battery.png");
		_oneThirdCharge = (Texture2D)GD.Load("res://textures/33%Battery.png");
		_emptyCharge = (Texture2D)GD.Load("res://textures/0%Battery.png");
		_chargeSprite.Show();
	}
	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("torch_power") && _maxEnergy != 0) {
			Visible = !Visible;
		}
		if (Visible) {
			Discharge(delta);
		}
	}
	private float TorchBatteryCharge {
		get => GameManager.Instance.TorchBatteryCharge;
		set => GameManager.Instance.TorchBatteryCharge = value;
	}
	private void Discharge(double delta) {
		if (TorchBatteryCharge <= _maxEnergy && TorchBatteryCharge > _minEnergy) {
			TorchBatteryCharge -= _dischargeRate * (float)delta;
		}
		if (TorchBatteryCharge <= _minEnergy) {
			Visible = false;
			TorchBatteryCharge = 0.0f;
		}
		UpdateChargeSprite();
	}
	private void UpdateChargeSprite() {
		if (TorchBatteryCharge > 67.0f) {
			_chargeSprite.Texture = _fullCharge;
		}
		else if (TorchBatteryCharge > 33.0f) {
			_chargeSprite.Texture = _twoThirdsCharge;
		}
		else if (TorchBatteryCharge > 0.0f) {
			_chargeSprite.Texture = _oneThirdCharge;
		}
		else {
			_chargeSprite.Texture = _emptyCharge;
			GD.Print("Фонарик разряжен.");
		}
	}
	public override void _ExitTree() {
		_chargeSprite.Hide();
	}
}
