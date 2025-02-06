using Godot;
using System;

public partial class LoadingScene : Control
{
	private ProgressBar progressBar;
	private string scenePath;
	
	public override void _Ready()
	{
		scenePath = GameManager.Instance.DownloadableScene;
		ResourceLoader.LoadThreadedRequest(scenePath);
		progressBar = GetNode<ProgressBar>("ProgressBar");
	}
	public override void _Process(double delta) {
		var progress = new Godot.Collections.Array{};
		ResourceLoader.LoadThreadedGetStatus(scenePath, progress);
		progressBar.Value = (double)progress[0]; 
		if (progressBar.Value >= 1.0) {
			GetTree().ChangeSceneToPacked((PackedScene)ResourceLoader.LoadThreadedGet(scenePath));
		}
	}
}
