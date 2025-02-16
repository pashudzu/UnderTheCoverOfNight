using Godot;
using System;
using System.Threading;

public partial class CharacterBody : CharacterBody3D {
	public const float DefaultSpeed = 2f;
	public const float JumpVelocity = 4.5f;
	private const float RunSpeed = 4.0f;
	private const float MaxPitch = 75.0f;
	private const float MinPitch = -20.0f;
	private float MinPitchRad = Mathf.DegToRad(MinPitch);
	private float MaxPitchRad = Mathf.DegToRad(MaxPitch);
	public const float Sens = 0.02f;
	private const float Friction = 3f;
	[Export]private float _runEnergy = 100f;
	private float _maxRunEnergy = 100f;
	private float _minRunEnergy = 0f;
	private float _dischargeRate = 10f;
	[Export]public float speed = DefaultSpeed;
	private bool _isRunning = false;
	private bool _isJumping = false;
	private bool _isWalking = false;
	private float _restTime = 0.0f;
	private float _cameraPitch = 0f;
	private float _rotationX = 0f;
	private float _rotationY = 0f;
	private bool _isPaused;
	private bool _firstgo = false;
	private MeshInstance3D _head;
	private Control _pauseMenu;
	private Control _inventory;
	private Area3D _checkSurfaceArea3D;
	private ProgressBar _runBar;
	private AnimationPlayer _animations;
	private AudioStreamPlayer3D _walkingSnowAudio;
	private AudioStreamPlayer3D _walkingWoodAudio;
	private bool _isDialogueGoing;
	private CollisionShape3D _characterBodyShape;
	
	public override void _Ready() {
		_characterBodyShape = GetNode<CollisionShape3D>("CollisionShape3D");
		_walkingSnowAudio = GetNode<AudioStreamPlayer3D>("Head/Camera3D/WalkingSnowAudio");
		_walkingWoodAudio = GetNode<AudioStreamPlayer3D>("Head/Camera3D/WalkingWoodAudio");
		_checkSurfaceArea3D = GetNode<Area3D>("CheckSurfaceArea3D");
		_animations = GetNode<AnimationPlayer>("Animations");
		if (GameManager.Instance != null) {
			GameManager.Instance.Player = GetParent<Node3D>();
			GD.Print("GameManager.Instance.Player инициализирован.");
		} else {
			GD.PrintErr("GameManager.Instance == null.");
		}
		GameManager.Instance.PlayerCharacterBody = this;
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
		_head = GetNodeOrNull<MeshInstance3D>("Head");
		_pauseMenu = GetNodeOrNull<Control>("PauseMenu");
		_inventory = GetNodeOrNull<Control>("Inventory");
		_runBar = GetNodeOrNull<ProgressBar>("RunBar");
		var _gameManager = GameManager.Instance;
		var _currentScene = GetTree().CurrentScene.Name;
		string _savedSceneName = _gameManager.SavedSceneName;
		if (_gameManager.SavedPlayerPosition != Vector3.Zero && _savedSceneName == _currentScene) {
			GlobalPosition = _gameManager.SavedPlayerPosition;
			GD.Print($"Позиция игрока возобновлена: {GlobalPosition}.");
		}
		if (_gameManager.SavedPlayerRotation != Vector3.Zero && _savedSceneName == _currentScene) {
			GlobalRotation = _gameManager.SavedPlayerRotation;
			GD.Print($"Поворот игрока возобновлён: {_head.GlobalRotation}");
		}
		_checkSurfaceArea3D.Connect("body_entered", new Callable(this, nameof(OnCheckSurfaceAreaEntered)));
	}
	
	public override void _Input(InputEvent @event) {
		if (Input.IsActionJustPressed("pause")) {
			Pause();
		}
		IsItPaused();
		if (_isPaused) {
			_inventory.Hide();
		}
		if (@event is InputEventMouseMotion mouseMotion) {
			if (IsSomeGuiOpen()) {
				return;
			}
			_rotationX += Mathf.DegToRad(mouseMotion.Relative.X) * Sens;
			_rotationY += mouseMotion.Relative.Y * Sens;
			
			Transform3D transform = Transform;
			transform.Basis = Basis.Identity;
			Transform = transform;
			
			_cameraPitch = Mathf.Clamp(Mathf.DegToRad(_rotationY), MinPitchRad, MaxPitchRad);
			
			RotateObjectLocal(Vector3.Down, _rotationX);
			_head.Rotation = new Vector3(-_cameraPitch, _head.Rotation.Y, Rotation.Z);
		}
	}
	
	public override void _PhysicsProcess(double delta) {
		MovePlayer(delta);
	}
	public override void _Process(double delta) {
		SetAnimation();
	}
	
	public void OnCheckSurfaceAreaEntered(Node body) {
		GD.Print($"body - {body.Name}");
		if (body.IsInGroup("Snow")) {
			_walkingSnowAudio.VolumeDb = 0f;
			_walkingWoodAudio.VolumeDb = -80f;
			GD.Print($"Игрок ходит по снегу, _walkingSnowAudio = {_walkingSnowAudio.VolumeDb}, _walkingWoodAudio = {_walkingWoodAudio.VolumeDb}");
		} else if (body.IsInGroup("Wood")) {
			_walkingSnowAudio.VolumeDb = -80f;
			_walkingWoodAudio.VolumeDb = 0f;
			GD.Print($"Игрок ходит по дереву, _walkingSnowAudio = {_walkingSnowAudio.VolumeDb}, _walkingWoodAudio = {_walkingWoodAudio.VolumeDb}");
		}
	}
	
	private void SetAnimation() {
		if (!IsOnFloor()) {
			_isJumping = true;
		} else {
			_isJumping = false;
		}
		if (_isRunning && !_isJumping) {
			_animations.Play("run");
		} else if (_isWalking && !_isJumping) {
			_animations.Play("walk");
		} else if (_isJumping) {
			_animations.Play("jump");
		} else {
			_animations.Play("idle");
		}
	}
	
	private void MovePlayer(double delta) {
		if (GameManager.Instance.IsEventAnimationIsOngoing) {
			_characterBodyShape.Disabled = true;
			return;
		}
		else {
			_characterBodyShape.Disabled = false;
		}
		Vector3 velocity = Velocity;
		_isDialogueGoing = GameManager.Instance.IsDialogueGoing;
		if (_isDialogueGoing) {
			return;
		}
		
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		_runBar.Value = _runEnergy;
		UpdateRunBar();
		
		bool isRunPressed = Input.IsActionPressed("run");
		bool isjumpPressed = Input.IsActionPressed("jump");
		
		if (isRunPressed && _runEnergy > 20f) {
			if (!_isRunning) {
				speed = RunSpeed;
				_isRunning = true;
			}
			_runEnergy -= _dischargeRate * (float)delta;
		}
		else {
			ChargeRecovery(delta);
		}

		if (isjumpPressed && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
		
		Vector2 inputDir = Input.GetVector("move_back", "move_forward", "move_left", "move_right");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
			_isWalking = true;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Friction * (float)delta);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Friction * (float)delta);
			_isWalking = false;
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}
	private void UpdateRunBar() {
		if (_runEnergy == 100f) {
			_runBar.Hide();
		} else {
			_runBar.Show();
		}
	}
	
	private void ChargeRecovery(double delta) {
		if (_isRunning) {
			speed = DefaultSpeed;
			_isRunning = false;
		}
		if (_runEnergy < 100f) {
			if (_restTime < 5f && _runEnergy < 20f) {
				_restTime += (float)delta + 0.1f;
			}
			else {
				_runEnergy += _dischargeRate * (float)delta;
				_restTime = 0f;
			}
		}
		if (_runEnergy > _maxRunEnergy) _runEnergy = 100;
	}
	
	private void Pause() {
		if (_isPaused) {
			Engine.TimeScale = 1;
			_pauseMenu.Hide();
			Input.SetMouseMode(Input.MouseModeEnum.Captured);
		}
		else {
			Engine.TimeScale = 0;
			_pauseMenu.Show();
			Input.SetMouseMode(Input.MouseModeEnum.Visible);
		}
		_firstgo = false;
	}
	
	private void IsItPaused() {
		if (Engine.TimeScale == 1) {
			_isPaused = false;
			_pauseMenu.Hide();
		}
		else {
			_isPaused = true;
		}
		if (_firstgo) {
			_isPaused = true;
		}
	}
	
	private bool IsInventoryVisible() {
		return _inventory.Visible;
	}
	private bool IsSomeGuiOpen() {
		return _isPaused || _firstgo || IsInventoryVisible() || _isDialogueGoing;
	}
}
