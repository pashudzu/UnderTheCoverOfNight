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
	[Export]public float SPEED = 50f;
	private AnimationPlayer _animation;
	private AudioStreamPlayer3D _ghostSound;
	[Flags]
	public enum EnemyStates {
		IsNone =            0b_0000_0000, //без действия
		IsPursuit =         0b_0000_0001, //приследует
		IsSeenPlayer =      0b_0000_0010, //видит игрока
		IsPlayerNearby =    0b_0000_0100, //игрок не подолёку
		IsEnemyWalking =    0b_0000_1000, //идёт
		IsEnemyHit =        0b_0001_0000, //ударил
		IsAtPost =          0b_0010_0000, //стоит на посту
		InPatrollingGuard = 0b_0100_0000  //патрулирует
	}
	public EnemyStates EnemyState;
	
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
		EnemyState = EnemyStates.IsEnemyWalking | EnemyStates.IsAtPost;
	}
	public override void _Process(double delta) {
		if ((EnemyState & (EnemyStates.IsPursuit | EnemyStates.InPatrollingGuard)) == 0) {
			BackToPost(delta);
		}
		if ((EnemyState & (EnemyStates.IsPursuit | EnemyStates.InPatrollingGuard)) != 0) {
			CheckAlertState(delta);
		}
		MoveAndSlide();
		if ((EnemyState & (EnemyStates.IsEnemyWalking)) == 0) {
			_animation.Play("ArmatureAction_003");
		}
		if ((EnemyState & EnemyStates.IsPlayerNearby) != 0 && !_ghostSound.Playing) {
			_ghostSound.Play();
		}
	}
	private void CheckAlertState(double delta) {
		if (GameManager.Instance.IsEventAnimationIsOngoing) {
			EventPlayerAwaking();
		} else if ((EnemyState & EnemyStates.IsEnemyHit) != 0) {
			GameManager.Instance.playerCaughtUp = true;
		} else if ((EnemyState & EnemyStates.IsPursuit) != 0) {
			PursuitState(delta);
		} else if ((EnemyState & EnemyStates.InPatrollingGuard) != 0) { 
			Patroling(delta);
		}
	}
	private void EventPlayerAwaking() {
		_screamerZone.Monitoring = false;
	}
	private void Patroling(double delta) {
		if ((EnemyState & EnemyStates.IsPursuit) != 0) { //_inPursuit
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
		EnemyState &= ~EnemyStates.IsAtPost;
	}
	private void BackToPost(double delta) {
		if ((EnemyState & EnemyStates.IsAtPost) == 0) {
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
			EnemyState |= EnemyStates.IsPursuit;
			EnemyState |= EnemyStates.IsSeenPlayer;
			EnemyState &= ~EnemyStates.IsAtPost;
			EnemyState &= ~EnemyStates.InPatrollingGuard;
		}
	}
	private void OnFOVBodyExited(Node body) {
		if (body.IsInGroup("Player")) {
			EnemyState &= ~EnemyStates.IsSeenPlayer;
			EnemyState &= ~EnemyStates.IsPursuit;
			EnemyState |= EnemyStates.InPatrollingGuard;
		}
	}
	private void OnNearbyZoneEntered(Node body) {
		if (body.IsInGroup("Player")) {
			EnemyState |= EnemyStates.IsPlayerNearby;
		}
	}
	private void OnNearbyZoneExited(Node body) {
		if(body.IsInGroup("Player")) {
			EnemyState &= ~EnemyStates.IsPlayerNearby;
		}
	}
	private void OnScremerZoneEntered(Node body) {
		if (body.IsInGroup("Player")) {
			Screamer();
		}
	}
	private void Screamer() {
		EnemyState &= ~EnemyStates.IsEnemyWalking;
		EnemyState |= EnemyStates.IsEnemyHit;
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
