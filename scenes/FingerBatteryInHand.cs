using Godot;
using System;

public partial class FingerBatteryInHand : Node3D
{
	private const float _amountChargeBattery = 50;
	public override void _Process(double delta) {
		if (Input.IsActionJustPressed("take_item")) {
			Interact();
		}
	}
	private void Interact() {
		if (GameManager.Instance.SavedItems[4] == "Фонарик" ||GameManager.Instance.SavedItems[5] == "Фонарик") (
			if ((GameManager.Instance.TorchBatteryCharge + _amountChargeBattery) > 100f) {
				GameManager.Instance.TorchBatteryCharge = 100f;
			} else {
				GameManager.Instance.TorchBatteryCharge = GameManager.Instance.TorchBatteryCharge + _amountChargeBattery;
			}
		)
		QueueFree();
	}
}
