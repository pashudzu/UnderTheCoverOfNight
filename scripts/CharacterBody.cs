using Godot;
using System;
using System.Threading;

public partial class CharacterBody : CharacterBody3D {
	public const float DEFAULT_SPEED = 2f;
	public const float JUMP_VELOITY = 4.5f;
	private const float RUN_SPEED = 4.0f;
	private const float MAX_PITCH = 75.0f;
	private const float MIN_PITCH = -20.0f;
	private float MIN_PITCH_RAD = Mathf.DegToRad(MIN_PITCH);
	private float MAX_PITCH_RAD = Mathf.DegToRad(MAX_PITCH);
	public const float SENS = 0.02f;
	private const float FRICTION = 3f;
	[Export]private float runEnergy = 100f;
	private float maxRunEnergy = 100f;
	private float minRunEnergy = 0f;
	private float dischargeRate = 10f;
	[Export]public float speed = DEFAULT_SPEED;
	private bool isRunning = false;
	private bool isJumping = false;
	private bool isWalking = false;
	private float restTime = 0.0f;
	private float cameraPitch = 0f;
	private float _rotationX = 0f;
	private float _rotationY = 0f;
	private bool isPaused;
	private bool firstgo = false;
	private MeshInstance3D head;
	private Control pauseMenu;
	private Control inventory;
	private Area3D checkSurfaceArea3D;
	private ProgressBar runBar;
	public Vector3 playerCurrentPosition;
	private AnimationPlayer animations;
	private AudioStreamPlayer3D walkingSnowAudio;
	private AudioStreamPlayer3D walkingWoodAudio;
	private bool IsDialogueGoing;
	private CollisionShape3D characterBodyShape;
	
	public override void _Ready() {
		characterBodyShape = GetNode<CollisionShape3D>("CollisionShape3D");
		walkingSnowAudio = GetNode<AudioStreamPlayer3D>("Head/Camera3D/WalkingSnowAudio");
		walkingWoodAudio = GetNode<AudioStreamPlayer3D>("Head/Camera3D/WalkingWoodAudio");
		checkSurfaceArea3D = GetNode<Area3D>("CheckSurfaceArea3D");
		animations = GetNode<AnimationPlayer>("Animations");
		if (GameManager.Instance != null) {
			GameManager.Instance.Player = GetParent<Node3D>();
			GD.Print("GameManager.Instance.Player инициализирован.");
		} else {
			GD.PrintErr("GameManager.Instance == null.");
		}
		GameManager.Instance.PlayerCharacterBody = this;
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
		head = GetNodeOrNull<MeshInstance3D>("Head");
		pauseMenu = GetNodeOrNull<Control>("PauseMenu");
		inventory = GetNodeOrNull<Control>("Inventory");
		runBar = GetNodeOrNull<ProgressBar>("RunBar");
		checkSurfaceArea3D.Connect("body_entered", new Callable(this, nameof(OnCheckSurfaceAreaEntered)));
	}
	
	public override void _Input(InputEvent @event) {
		if (Input.IsActionJustPressed("pause")) {
			Pause();
		}
		IsItPaused();
		if (isPaused) {
			inventory.Hide();
		}
		if (@event is InputEventMouseMotion mouseMotion) {
			if (IsSomeGuiOpen()) {
				return;
			}
			_rotationX += Mathf.DegToRad(mouseMotion.Relative.X) * SENS;
			_rotationY += mouseMotion.Relative.Y * SENS;
			
			Transform3D transform = Transform;
			transform.Basis = Basis.Identity;
			Transform = transform;
			
			cameraPitch = Mathf.Clamp(Mathf.DegToRad(_rotationY), MIN_PITCH_RAD, MAX_PITCH_RAD);
			
			RotateObjectLocal(Vector3.Down, _rotationX);
			head.Rotation = new Vector3(-cameraPitch, head.Rotation.Y, Rotation.Z);
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
			walkingSnowAudio.VolumeDb = 0f;
			walkingWoodAudio.VolumeDb = -80f;
			GD.Print($"Игрок ходит по снегу, walkingSnowAudio = {walkingSnowAudio.VolumeDb}, walkingWoodAudio = {walkingWoodAudio.VolumeDb}");
		} else if (body.IsInGroup("Wood")) {
			walkingSnowAudio.VolumeDb = -80f;
			walkingWoodAudio.VolumeDb = 0f;
			GD.Print($"Игрок ходит по дереву, walkingSnowAudio = {walkingSnowAudio.VolumeDb}, walkingWoodAudio = {walkingWoodAudio.VolumeDb}");
		}
	}
	
	private void SetAnimation() {
		if (!IsOnFloor()) {
			isJumping = true;
		} else {
			isJumping = false;
		}
		if (isRunning && !isJumping) {
			animations.Play("run");
		} else if (isWalking && !isJumping) {
			animations.Play("walk");
		} else if (isJumping) {
			animations.Play("jump");
		} else {
			animations.Play("idle");
		}
	}
	
	private void MovePlayer(double delta) {
		if (GameManager.Instance.IsEventAnimationIsOngoing) {
			characterBodyShape.Disabled = true;
			return;
		}
		else {
			characterBodyShape.Disabled = false;
		}
		Vector3 velocity = Velocity;
		IsDialogueGoing = GameManager.Instance.IsDialogueGoing;
		if (IsDialogueGoing) {
			return;
		}
		
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		runBar.Value = runEnergy;
		UpdateRunBar();
		
		bool isRunPressed = Input.IsActionPressed("run");
		bool isjumpPressed = Input.IsActionPressed("jump");
		
		if (isRunPressed && runEnergy > 20f) {
			if (!isRunning) {
				speed = RUN_SPEED;
				isRunning = true;
			}
			runEnergy -= dischargeRate * (float)delta;
		}
		else {
			ChargeRecovery(delta);
		}

		if (isjumpPressed && IsOnFloor())
		{
			velocity.Y = JUMP_VELOITY;
		}
		
		Vector2 inputDir = Input.GetVector("move_back", "move_forward", "move_left", "move_right");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
			isWalking = true;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, FRICTION * (float)delta);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, FRICTION * (float)delta);
			isWalking = false;
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}
	private void UpdateRunBar() {
		if (runEnergy == 100f) {
			runBar.Hide();
		} else {
			runBar.Show();
		}
	}
	
	private void ChargeRecovery(double delta) {
		if (isRunning) {
			speed = DEFAULT_SPEED;
			isRunning = false;
		}
		if (runEnergy < 100f) {
			if (restTime < 5f && runEnergy < 20f) {
				restTime += (float)delta + 0.1f;
			}
			else {
				runEnergy += dischargeRate * (float)delta;
				restTime = 0f;
			}
		}
		if (runEnergy > maxRunEnergy) runEnergy = 100;
	}
	
	private void Pause() {
		if (isPaused) {
			Engine.TimeScale = 1;
			pauseMenu.Hide();
			Input.SetMouseMode(Input.MouseModeEnum.Captured);
		}
		else {
			Engine.TimeScale = 0;
			pauseMenu.Show();
			Input.SetMouseMode(Input.MouseModeEnum.Visible);
		}
		firstgo = false;
	}
	
	private void IsItPaused() {
		if (Engine.TimeScale == 1) {
			isPaused = false;
			pauseMenu.Hide();
		}
		else {
			isPaused = true;
		}
		if (firstgo) {
			isPaused = true;
		}
	}
	
	private bool IsInventoryVisible() {
		return inventory.Visible;
	}
	private bool IsSomeGuiOpen() {
		return isPaused || firstgo || IsInventoryVisible() || IsDialogueGoing;
	}
}
