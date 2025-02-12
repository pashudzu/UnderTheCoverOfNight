using Godot;
using System;

public partial class LoadingScene : Control
{
	private ProgressBar _progressBar;
	private string _scenePath;
	
	public override void _Ready()
	{
		_scenePath = GameManager.Instance.DownloadableScene;
		ResourceLoader.LoadThreadedRequest(_scenePath);
		_progressBar = GetNode<ProgressBar>("ProgressBar");
	}
	public override void _Process(double delta) {
		var progress = new Godot.Collections.Array{};
		ResourceLoader.LoadThreadedGetStatus(_scenePath, progress);
		_progressBar.Value = (double)progress[0]; 
		if (_progressBar.Value >= 1.0) {
			GetTree().ChangeSceneToPacked((PackedScene)ResourceLoader.LoadThreadedGet(_scenePath));
		}
	}
}
