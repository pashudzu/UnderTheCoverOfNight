using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class Enemy : CharacterBody3D
{
	private Node3D _player;
	private CharacterBody3D _playerCharacterBody;
	private NavigationRegion3D _navMap;
	private NavigationAgent3D _navAgent;
	private Area3D _enemyFOV;
	private Area3D _nearbyZone;
	private Area3D _screamerZone;
	private List<Vector3> _allPoints = new List<Vector3>();
	private int _nextPoint = 0;
	[Export]public bool inPatrollingGuard = true;
	[Export]public float SPEED = 50f;
	private bool _inPursuit = false;
	private bool _seenPlayer = false;
	private AnimationPlayer _animation;
	private AudioStreamPlayer3D _ghostSound;
	private bool _isPlayerNearby = false;
	private bool _enemyWalking = true;
	private bool _enemyHit = false;
	private bool _isAtPost = true;
	
	public override void _Ready() {
		_player = GameManager.Instance.Player;
		_playerCharacterBody = GameManager.Instance.PlayerCharacterBody;
		_enemyFOV = GetNode<Area3D>("Enemy_FOV");
		_nearbyZone = GetNode<Area3D>("NearbyZone");
		_screamerZone = GetNode<Area3D>("ScreamerZone");
		_navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
		_navMap = GetParent().GetNode<NavigationRegion3D>("NavigationRegion3D");
		_animation = GetNode<AnimationPlayer>("AnimationPlayer");
		_ghostSound = _player.GetNode<AudioStreamPlayer3D>("CharacterBody/Head/Camera3D/GhostSound");
		var patrolPoints = GetParent().GetNode("AllEnemyPatrolPoints").GetChildren();
		for (int i = 0; i < patrolPoints.Count; i++) {
			Marker3D patrolPoint = patrolPoints[i] as Marker3D;
			if (patrolPoint != null) {
				_allPoints.Add(patrolPoint.GlobalPosition);
			}
			else {
				GD.PrintErr($"patrolPoint {i} у Eminem равен null");
			}
		}
		_enemyFOV.Connect("body_entered", new Callable(this, nameof(OnFOVBodyEntered)));
		_enemyFOV.Connect("body_exited", new Callable(this, nameof(OnFOVBodyExited)));
		_nearbyZone.Connect("body_entered", new Callable(this, nameof(OnNearbyZoneEntered)));
		_nearbyZone.Connect("body_exited", new Callable(this, nameof(OnNearbyZoneExited)));
		_screamerZone.Connect("body_entered", new Callable(this, nameof(OnScremerZoneEntered)));
		GD.Print("Монстр готов.");
	}
	public override void _Process(double delta) {
		if (!_inPursuit && !inPatrollingGuard) {
			BackToPost(delta);
		}
		if (_inPursuit || inPatrollingGuard) {
			CheckAlertState(delta);
		}
		MoveAndSlide();
		if (_enemyWalking) {
			_animation.Play("ArmatureAction_003");
		}
		if (_isPlayerNearby && !_ghostSound.Playing) {
			_ghostSound.Play();
		}
	}
	private void CheckAlertState(double delta) {
		if (GameManager.Instance.IsEventAnimationIsOngoing) {
			EventPlayerAwaking();
		} else if (_enemyHit) {
			GameManager.Instance.playerCaughtUp = true;
		} else if (_inPursuit) {
			PursuitState(delta);
		} else if (inPatrollingGuard) {
			Patroling(delta);
		}
	}
	private void EventPlayerAwaking() {
		_screamerZone.Monitoring = false;
	}
	private void Patroling(double delta) {
		if (_inPursuit) {
			return;
		}
		_navAgent.TargetPosition = _allPoints[_nextPoint];
		Vector3 direction = (_navAgent.GetNextPathPosition() - GlobalPosition).Normalized();
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
		if (GlobalPosition.DistanceTo(_allPoints[_nextPoint]) < 2) {
			Random rnd = new Random();
			if (_allPoints != null) {
				_nextPoint = rnd.Next(1, 10);
				GD.Print($"Призрак идёт к точке {_nextPoint}");
			} else { 
				GD.Print("AllEnemyPatrolPoints == null");
			}
		}
	}
	private void PursuitState(double delta) {
		Vector3 playerPosition = _playerCharacterBody.GlobalPosition; 
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
		_isAtPost = false;
	}
	private void BackToPost(double delta) {
		if (!_isAtPost) {
			_navAgent.TargetPosition = _allPoints[0];
			Vector3 direction = (_navAgent.GetNextPathPosition() - GlobalPosition).Normalized();
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
			_inPursuit = true;
			_seenPlayer = true;
			_isAtPost = false;
			inPatrollingGuard = false;
		}
	}
	private void OnFOVBodyExited(Node body) {
		if (body.IsInGroup("Player")) {
			_seenPlayer = false;
			_inPursuit = false;
			inPatrollingGuard = true;
		}
	}
	private void OnNearbyZoneEntered(Node body) {
		if (body.IsInGroup("Player")) {
			_isPlayerNearby = true;
		}
	}
	private void OnNearbyZoneExited(Node body) {
		if(body.IsInGroup("Player")) {
			_isPlayerNearby = false;
		}
	}
	private void OnScremerZoneEntered(Node body) {
		if (body.IsInGroup("Player")) {
			Screamer();
		}
	}
	private void Screamer() {
		_enemyWalking = false;
		_enemyHit = true;
		_animation.Play("ArmatureAction_002");
		GD.Print("Игрок видит скример");
		Thread.Sleep(1000);
		string DownloadableScene = GameManager.Instance.DownloadableScene = "res://scenes/cutScene.tscn";
		PackedScene loadScene = (PackedScene)ResourceLoader.Load("res://scenes/loading_scene.tscn");
		CollisionsQueueFree();
		GetTree().ChangeSceneToPacked(loadScene);
	}
	private void GetNewTarget(Vector3 newTarget) {
		_navAgent.SetTargetPosition(newTarget);
	}
	private void CollisionsQueueFree() {
		_enemyFOV.CallDeferred("queue_free");
		_nearbyZone.CallDeferred("queue_free");
		_screamerZone.CallDeferred("queue_free");
	}
}
