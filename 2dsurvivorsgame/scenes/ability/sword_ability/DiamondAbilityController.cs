using Godot;
using System;
using System.Linq;

public partial class DiamondAbilityController : Node
{
	private int damage = 2;
	private int damageDistance = 32;
	private int gridSize = 16;

	[Export]
	public Timer timer {get; private set;}

	[Export]
	public PackedScene DiamondAbility {get;private set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer.Timeout += OnTimerTimeOut;
	}

	private void OnTimerTimeOut()
	{
		var towerNode = GetTree().GetNodesInGroup("tower").FirstOrDefault() as Node2D;
		if(towerNode is null) return;

        var enemies = GetTree().GetNodesInGroup("enemy").OfType<Node2D>()
            .Where(enemy => IsWithinDiamondPattern(enemy.GlobalPosition, towerNode.GlobalPosition, damageDistance, gridSize))
            .ToList();
		if(enemies.Count == 0) return;

		foreach(var enemy in enemies)
		{
			var enemyPosition = enemy.GlobalPosition;
			var diamondAbilityInstance = DiamondAbility.Instantiate<DiamondAbility>();			
			
			towerNode.GetParent().AddChild(diamondAbilityInstance);

			diamondAbilityInstance.HitboxComponent.Damage = damage;
			diamondAbilityInstance.GlobalPosition = enemyPosition;
			diamondAbilityInstance.GlobalPosition += Vector2.Right.Rotated(new Random().Next(0, (int)Math.Tau) * 4);

			// Aim attack at enemy
			var enemyDirection = enemyPosition - diamondAbilityInstance.GlobalPosition;
			diamondAbilityInstance.Rotation = enemyDirection.Angle();
		}
	}

	private static bool IsWithinDiamondPattern(Vector2 enemyPosition, Vector2 towerPosition, float damageDistance, float gridSize)
    {
        // Calculate the difference in x and y directions
        var deltaX = Mathf.Abs(enemyPosition.X - towerPosition.X);
        var deltaY = Mathf.Abs(enemyPosition.Y - towerPosition.Y);

        // Convert the difference to the 64-pixel grid
        deltaX = Mathf.Round(deltaX / gridSize) * gridSize;
        deltaY = Mathf.Round(deltaY / gridSize) * gridSize;

        // Check if the enemy is within the diamond pattern
        return deltaX + deltaY <= damageDistance;
    }
}
