using Godot;
using System;

public partial class EnemyManager : Node
{
	private const int SPAWN_RADIUS = 200;	// just outside the window. 10px outside window

	[Export]
	public PackedScene BasicEnemyScene{get;set;}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// var timer = GetNode<Timer>("Timer");
		// timer.Timeout += MyTimerTimeoutEventHandler;
	}

	private void MyTimerTimeoutEventHandler()
	{
		var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		if(player is null) return;

		var randomDirection = Vector2.Right.Rotated(new Random().Next(0, (int)Math.Tau));
		var spawnPosition = player.GlobalPosition + (randomDirection * SPAWN_RADIUS);
		
		// Snap the spawn position to the nearest 64-pixel grid point 
		spawnPosition = new Vector2(
			Mathf.Round(spawnPosition.X / 64) * 64, 
			Mathf.Round(spawnPosition.Y / 64) * 64 
			);

		var enemy = BasicEnemyScene.Instantiate() as Node2D;
		GetParent().AddChild(enemy);
		enemy.GlobalPosition = spawnPosition;
	}
}
