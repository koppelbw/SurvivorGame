using Godot;
using System;
using System.Linq;

public partial class BasicEnemy : CharacterBody2D
{
	private const int MaxSpeed = 40;
	private HealthComponent HealthComponent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var direction = GetDirectionToPlayer();
		Velocity = direction * MaxSpeed;
		MoveAndSlide();
	}

	private Vector2 GetDirectionToPlayer()
	{
		var playerNode = GetTree().GetNodesInGroup("player").FirstOrDefault() as Node2D;
		var direction = Vector2.Zero;

		if(playerNode is not null)
		{
			direction = (playerNode.GlobalPosition - GlobalPosition).Normalized();
		}

		return direction;
	}
}
