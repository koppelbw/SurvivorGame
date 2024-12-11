using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SwordAbilityController : Node
{
	private const float MAX_RANGE = 150;
	private int Damage = 5;

	[Export]
	public PackedScene SwordAbility {get; private set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() 
	{ 
		var timer = GetNode<Timer>("Timer"); 
		timer.Connect("timeout", new Callable(this, nameof(OnTimerTimeout)));
	}
		 
	private void OnTimerTimeout() 
	{ 
		var playerNode = GetTree().GetNodesInGroup("player").FirstOrDefault() as Node2D;
		if(playerNode is null)
		{
			return;
		}

		// Finds enemies who are close enough to attack
		var enemies = GetTree().GetNodesInGroup("enemy").Select(x => x as Node2D)
			.Where(x => x.GlobalPosition.DistanceSquaredTo(playerNode.GlobalPosition) < Math.Pow(MAX_RANGE, 2)).ToList();
		if(enemies.Count == 0)
		{
			return;
		}

		// Sort to find closest enemy
		enemies.Sort(delegate(Node2D a, Node2D b)
		{
			var aDistance = a.GlobalPosition.DistanceSquaredTo(playerNode.GlobalPosition);
			var bDistance = b.GlobalPosition.DistanceSquaredTo(playerNode.GlobalPosition);
			return aDistance < bDistance ? 1 : 0;	
		});	

		var closestEnemyPosition = enemies.FirstOrDefault().GlobalPosition;

		// Spawn sword ability and attack closest enemy
		var swordInstance = SwordAbility.Instantiate() as SwordAbility;
		playerNode.GetParent().AddChild(swordInstance);
		swordInstance.HitboxComponent.Damage = Damage;
		
		swordInstance.GlobalPosition = closestEnemyPosition;
		swordInstance.GlobalPosition += Vector2.Right.Rotated(new Random().Next(0, (int)Math.Tau) * 4);

		// Aim sword at enemy
		var enemyDirection = closestEnemyPosition - swordInstance.GlobalPosition;
		swordInstance.Rotation = enemyDirection.Angle();
	}
}
