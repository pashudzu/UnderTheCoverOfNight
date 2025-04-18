using Godot;
using System;
using System.Collections.Generic;

public partial class MusAudioStreamPlayer : AudioStreamPlayer
{
	private new List<string> _scenesNameWithoutMusic = new() {"home_scene", "begining", "cutScene"};
	public override void _Process(double delta) {
		Interact();
	}
	private void Interact() {
		for (int i = 0; i < _scenesNameWithoutMusic.Count - 1; i++) {
			if (GetTree().CurrentScene.Name == _scenesNameWithoutMusic[i]) {
				Stop();
				return;
			}
		}
		if (!IsPlaying()) {
			Play();
		}
	}
}
