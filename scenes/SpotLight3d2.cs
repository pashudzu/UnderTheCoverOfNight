using Godot;
using System;

public partial class SpotLight3d2 : SpotLight3D
{
	[Export] public float batteryCharge = 100.0f;
	private float maxEnergy = 100.0f;
	private float minEnergy = 0.0f;
	private float dischargeRate = 0.1111f;
	private Node3D player = GameManager.Instance.Player;
	private Sprite2D chargeSprite;
	private Texture2D fullCharge, twoThirdsCharge, oneThirdCharge, emptyCharge;
	
	public override void _Ready(){
		chargeSprite = player.GetNode<Sprite2D>("CharacterBody/Charge");
		fullCharge = (Texture2D)GD.Load("res://textures/FullCharge.png");
		twoThirdsCharge = (Texture2D)GD.Load("res://textures/67%Battery.png");
		oneThirdCharge = (Texture2D)GD.Load("res://textures/33%Battery.png");
		emptyCharge = (Texture2D)GD.Load("res://textures/0%Battery.png");
		chargeSprite.Show();
	}
	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("torch_power") && maxEnergy != 0) {
			Visible = !Visible;
		}
		if (Visible) {
			Discharge(delta);
		}
	}
	private void Discharge(double delta) {
		if (batteryCharge <= maxEnergy && batteryCharge > minEnergy) {
			batteryCharge -= dischargeRate * (float)delta;
		}
		if (batteryCharge <= minEnergy) {
			Visible = false;
			batteryCharge = 0.0f;
		}
		UpdateChargeSprite();
	}
	private void UpdateChargeSprite() {
		if (batteryCharge > 67.0f) {
			chargeSprite.Texture = fullCharge;
		}
		else if (batteryCharge > 33.0f) {
			chargeSprite.Texture = twoThirdsCharge;
		}
		else if (batteryCharge > 0.0f) {
			chargeSprite.Texture = oneThirdCharge;
		}
		else {
			chargeSprite.Texture = emptyCharge;
			GD.Print("Фонарик разряжен.");
		}
	}
	public override void _ExitTree() {
		chargeSprite.Hide();
	}
}
