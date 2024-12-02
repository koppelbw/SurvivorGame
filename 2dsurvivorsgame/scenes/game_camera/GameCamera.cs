using System;
using System.Linq;
using Godot;

public partial class GameCamera : Camera2D
{
	private Vector2 TargetPosition = Vector2.Zero;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MakeCurrent();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		SetTargetPosition();
		GlobalPosition = GlobalPosition.Lerp(TargetPosition, (float)(1.0 - Math.Exp(-delta * 10)));
	}

	private void SetTargetPosition()
	{
		var playerNode = GetTree().GetNodesInGroup("player").FirstOrDefault() as Node2D;
		TargetPosition = playerNode.GlobalPosition;
	}
}
