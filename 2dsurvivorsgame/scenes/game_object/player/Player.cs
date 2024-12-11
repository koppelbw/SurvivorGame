using Godot;
using System;

public partial class Player : CharacterBody2D
{
	private const float MAX_SPEED = 125;
	private const float ACCELLERATION_SMOOTHING = 25;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var movementVector = GetMovementVector();
		var direction = movementVector.Normalized();
		var targetVelocity = direction * MAX_SPEED;

		Velocity = Velocity.Lerp(targetVelocity, (float)(1 - Math.Exp(-delta * ACCELLERATION_SMOOTHING)));

		MoveAndSlide();
	}

	private Vector2 GetMovementVector()
	{
		var xMovement = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
		var yMovement = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");

		return new Vector2(xMovement, yMovement);
	}
}
