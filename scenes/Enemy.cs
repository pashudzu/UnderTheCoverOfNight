using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class Enemy : CharacterBody3D
{
	private Node3D player;
	private CharacterBody3D playerCharacterBody;
	private NavigationRegion3D navMap;
	private NavigationAgent3D navAgent;
	private Area3D enemyFOV;
	private Area3D nearbyZone;
	private Area3D screamerZone;
	private List<Vector3> allPoints = new List<Vector3>();
	int nextPoint = 0;
	[Export]public bool inPatrollingGuard = true;
	[Export]public float SPEED = 50f;
	private bool inPursuit = false; 
	private bool isAtPost = true;
	private bool seenPlayer = false;
	private AnimationPlayer animation;
	private AudioStreamPlayer3D ghostSound;
	private bool isPlayerNearby = false;
	private bool enemyWalking = true;
	private bool enemyHit = false;
	
	public override void _Ready() {
		player = GameManager.Instance.Player;
		playerCharacterBody = GameManager.Instance.PlayerCharacterBody;
		enemyFOV = GetNode<Area3D>("Enemy_FOV");
		nearbyZone = GetNode<Area3D>("NearbyZone");
		screamerZone = GetNode<Area3D>("ScreamerZone");
		navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		navMap = GetParent().GetNode<NavigationRegion3D>("NavigationRegion3D");
		animation = GetNode<AnimationPlayer>("AnimationPlayer");
		ghostSound = player.GetNode<AudioStreamPlayer3D>("CharacterBody/Head/Camera3D/GhostSound");
		var patrolPoints = GetParent().GetNode("AllEnemyPatrolPoints").GetChildren();
		for (int i = 0; i < patrolPoints.Count; i++) {
			Marker3D patrolPoint = patrolPoints[i] as Marker3D;
			if (patrolPoint != null) {
				allPoints.Add(patrolPoint.GlobalPosition);
			}
			else {
				GD.PrintErr($"patrolPoint {i} у Eminem равен null");
			}
		}
		enemyFOV.Connect("body_entered", new Callable(this, nameof(OnFOVBodyEntered)));
		enemyFOV.Connect("body_exited", new Callable(this, nameof(OnFOVBodyExited)));
		nearbyZone.Connect("body_entered", new Callable(this, nameof(OnNearbyZoneEntered)));
		nearbyZone.Connect("body_exited", new Callable(this, nameof(OnNearbyZoneExited)));
		screamerZone.Connect("body_entered", new Callable(this, nameof(OnScremerZoneEntered)));
		GD.Print("Монстр готов.");
	}
	public override void _Process(double delta) {
		if (!inPursuit && !inPatrollingGuard) {
			BackToPost(delta);
		}
		if (inPursuit || inPatrollingGuard) {
			CheckAlertState(delta);
		}
		MoveAndSlide();
		if (enemyWalking) {
			animation.Play("ArmatureAction_003");
		}
		if (isPlayerNearby && !ghostSound.Playing) {
			ghostSound.Play();
		}
	}
	private void CheckAlertState(double delta) {
		if (GameManager.Instance.IsEventAnimationIsOngoing) {
			EventPlayerAwaking();
		} else if (enemyHit) {
			GameManager.Instance.playerCaughtUp = true;
		} else if (inPursuit) {
			PursuitState(delta);
		} else if (inPatrollingGuard) {
			Patroling(delta);
		}
	}
	private void EventPlayerAwaking() {
		screamerZone.Monitoring = false;
	}
	private void Patroling(double delta) {
		if (inPursuit) {
			return;
		}
		navAgent.TargetPosition = allPoints[nextPoint];
		Vector3 direction = (navAgent.GetNextPathPosition() - GlobalPosition).Normalized();
		Vector3 currentVelocity = Velocity;
		Vector3 targetVelocity = direction * SPEED * (float)delta;
		Velocity = currentVelocity.Lerp(targetVelocity, 0.1f); 
		direction.Y = 0;
		Vector3 targetPosition = GlobalPosition - direction;
		if (!GlobalTransform.Origin.IsEqualApprox(targetPosition)) {
			LookAt(targetPosition);
		} else {
			GD.PrintErr($"Призрак не может смотреть в одиноковую позицию. Current Position: {GlobalTransform.Origin}, Target Position: {targetPosition}. Ошибка в функции патрулирования.");
		}
		if (GlobalPosition.DistanceTo(allPoints[nextPoint]) < 2) {
			Random rnd = new Random();
			if (allPoints != null) {
				nextPoint = rnd.Next(1, 10);
				GD.Print($"Призрак идёт к точке {nextPoint}");
			} else { 
				GD.Print("AllEnemyPatrolPoints == null");
			}
		}
	}
	private void PursuitState(double delta) {
		Vector3 playerPosition = playerCharacterBody.GlobalPosition; 
		Vector3 enemyPosition = GlobalPosition;
		
		float dirX = (playerPosition.X - enemyPosition.X);
		float dirZ = (playerPosition.Z - enemyPosition.Z);
		
		Vector3 directionToPlayer = new Vector3(dirX, 0, dirZ).Normalized();
		
		Velocity = directionToPlayer  * SPEED * (float)delta;
		
		if (enemyPosition.DistanceTo(playerPosition) < 0.1f) {
			Vector3 targetPosition = new Vector3(playerPosition.X, playerPosition.Y, playerPosition.Z);
			if (!GlobalTransform.Origin.IsEqualApprox(targetPosition)) {
				LookAt(targetPosition);
			} else {
				GD.PrintErr($"Призрак не может смотреть в одиноковую позицию. Current Position: {GlobalTransform.Origin}, Target Position: {targetPosition}. Ошибка в функции преследываения.");
			}
		}
		isAtPost = false;
	}
	private void BackToPost(double delta) {
		if (!isAtPost) {
			navAgent.TargetPosition = allPoints[0];
			Vector3 direction = (navAgent.GetNextPathPosition() - GlobalPosition).Normalized();
			Vector3 currentVelocity = Velocity;
			Vector3 targetVelocity = direction * SPEED * (float)delta;
			Velocity = currentVelocity.Lerp(targetVelocity, 0.1f); 
			direction.Y = 0;
			Vector3 targetPosition = GlobalPosition - direction;
			if (!GlobalTransform.Origin.IsEqualApprox(targetPosition)) {
				LookAt(targetPosition);
			} else {
				GD.PrintErr($"Призрак не может смотреть в одиноковую позицию. Current Position: {GlobalTransform.Origin}, Target Position: {targetPosition}. Ошибка в функции преследываения.");
			}
		}
	}
	private void OnFOVBodyEntered(Node body) {
		if (body.IsInGroup("Player")) {
			inPursuit = true;
			seenPlayer = true;
			isAtPost = false;
			inPatrollingGuard = false;
		}
	}
	private void OnFOVBodyExited(Node body) {
		if (body.IsInGroup("Player")) {
			seenPlayer = false;
			inPursuit = false;
			inPatrollingGuard = true;
		}
	}
	private void OnNearbyZoneEntered(Node body) {
		if (body.IsInGroup("Player")) {
			isPlayerNearby = true;
		}
	}
	private void OnNearbyZoneExited(Node body) {
		if(body.IsInGroup("Player")) {
			isPlayerNearby = false;
		}
	}
	private void OnScremerZoneEntered(Node body) {
		if (body.IsInGroup("Player")) {
			Screamer();
		}
	}
	private void Screamer() {
		enemyWalking = false;
		enemyHit = true;
		animation.Play("ArmatureAction_002");
		GD.Print("Игрок видит скример");
		Thread.Sleep(1000);
		string DownloadableScene = GameManager.Instance.DownloadableScene = "res://scenes/cutScene.tscn";
		PackedScene loadScene = (PackedScene)ResourceLoader.Load("res://scenes/loading_scene.tscn");
		CollisionsQueueFree();
		GetTree().ChangeSceneToPacked(loadScene);
	}
	private void GetNewTarget(Vector3 newTarget) {
		navAgent.SetTargetPosition(newTarget);
	}
	private void CollisionsQueueFree() {
		enemyFOV.CallDeferred("queue_free");
		nearbyZone.CallDeferred("queue_free");
		screamerZone.CallDeferred("queue_free");
	}
}
