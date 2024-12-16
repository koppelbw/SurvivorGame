using Godot;
using System;
using System.Linq;

public partial class BasicEnemy : CharacterBody2D
{
	private const int moveDistance = 64;
	private const int MaxSpeed = 40;
	private int frameCount = 0;
	private bool isFrozen = false;
	private HealthComponent HealthComponent;
	private Area2D ClickableArea;
	private FreezeAbilityController FreezeAbilityController;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		FreezeAbilityController = GetNode<FreezeAbilityController>("FreezeAbilityController");
		ClickableArea = GetNode<Area2D>("ClickableArea");
		ClickableArea.InputEvent += OnMouseClick;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		frameCount++;
		if(frameCount % 3000 == 0)
		{
			if(!isFrozen)
			{
				GD.Print("Move 1 tile");
				MoveOneTileToPlayer();
			}
		}
		
		// var direction = GetDirectionToPlayer();
		// Velocity = direction * MaxSpeed;
		// MoveAndSlide();		
	}

	void OnMouseClick(Node viewport, InputEvent @event, long shapeIdx)
	{
		if(@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.Pressed)
			{
				if(FreezeAbilityController.isFrozen)
				{
					FreezeAbilityController.UnFreeze();
				}
				else
				{
					FreezeAbilityController.PlayFreeze();
				}
				isFrozen = !isFrozen;
			}
		}
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

	private void MoveOneTileToPlayer()
	{
		var player = GetTree().GetNodesInGroup("player").FirstOrDefault() as Node2D;		

		// Get the difference in positions 
		Vector2 difference = player.GlobalPosition - GlobalPosition;

		var motionX = new Vector2(Math.Sign(difference.X) * moveDistance, 0);
		var motionY = new Vector2(0, Math.Sign(difference.Y) * moveDistance);
		
		// Determine the cardinal direction to move towards
		if (Math.Abs(difference.X) > Math.Abs(difference.Y))
		{			
			if(!WillCollide(motionX))	// desired direction
			{
				var x = MoveAndCollide(motionX);
				
			}
			else
			{
				GD.Print("Will collide X");
				if(!WillCollide(motionY))	// try moving in other axis
				{
					GD.Print("Move in Y");
					MoveAndCollide(motionY);
				}
				else if(!WillCollide(-motionY))
				{
					GD.Print("Move in -Y");
					MoveAndCollide(-motionY);
				}
				else
				{
					GD.Print("Move in -X");
					MoveAndCollide(-motionX);	// lastly try to move back way we came
				}
			}
			// GlobalPosition += motion;
		}
		else
		{
			if(!WillCollide(motionY))	// desired direction
			{
				MoveAndCollide(motionY);
			}
			else
			{
				GD.Print("Will collide Y");
				if(!WillCollide(motionX))	// try moving in other axis
				{
					GD.Print("Move in X");
					MoveAndCollide(motionX);
				}
				else if(!WillCollide(-motionX))
				{
					GD.Print("Move in -X");
					MoveAndCollide(-motionX);
				}
				else
				{
					GD.Print("Move in -Y");
					MoveAndCollide(-motionY);	// lastly try to move back way we came
				}
			}
			// GlobalPosition += motion;
		}
	}

	private bool WillCollide(Vector2 offset)
    {
        Vector2 futurePosition = GlobalPosition + offset;

        // Create a BoxShape2D at the future position to check for potential collisions
        var boxShape = new RectangleShape2D
        {
            Size = new Vector2(32, 32) // Adjust based on your enemy's collision shape size
        };

        var collisionParameters = new PhysicsShapeQueryParameters2D
        {
            Transform = new Transform2D(0, futurePosition),
            Shape = boxShape,
            // CollisionMask = 4,
			CollideWithAreas = true
			
        };

        // Use Physics2DDirectSpaceState to check for collisions
        var spaceState = GetWorld2D().DirectSpaceState;
        var result = spaceState.IntersectShape(collisionParameters);

        // Return true if there are any collisions detected
		GD.Print("collision detection: " + result.Count);
        return result.Count > 0;
    }
}
