using Godot;
using System;
using System.Linq;

public partial class BasicEnemy : Node2D
{
	private const int moveDistance = 16;
	public int moveSpeed = 1500;
	private int frameCount = 0;
	private bool isFrozen = false;
	public float enemyMovementSpeed = 1f;
	private bool isMoving = false;

	private HealthComponent HealthComponent;
	private Area2D ClickableArea;
	private FreezeAbilityController FreezeAbilityController;
	private Sprite2D sprite2D;
	private TileMapLayer tileMapLayer;
	private RayCast2D rayCast2D;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite2D = GetNode<Sprite2D>("Sprite2D");
		tileMapLayer = GetParent().GetNode<TileMapLayer>("TileMapLayer");
		rayCast2D = GetNode<RayCast2D>("RayCast2D");

		FreezeAbilityController = GetNode<FreezeAbilityController>("FreezeAbilityController");
		ClickableArea = GetNode<Area2D>("ClickableArea");
		ClickableArea.InputEvent += OnMouseClick;
	}

	public override void _Process(double delta)
	{
		frameCount++;
		if(frameCount % moveSpeed == 0)
		{
			if(!isMoving && !isFrozen)
			{
				MoveOneTileToPlayer(delta);
			}
		}	
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if(!isMoving) return;
		
		sprite2D.GlobalPosition = sprite2D.GlobalPosition.MoveToward(GlobalPosition, enemyMovementSpeed);

		if(GlobalPosition == sprite2D.GlobalPosition)
		{
			isMoving = false;
		}
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
					sprite2D.Modulate = new Color(1.0f, 1.0f, 1.0f); // undo tint
				}
				else
				{
					FreezeAbilityController.PlayFreeze();
					sprite2D.Modulate = new Color(0.0f, 0.0f, 0.5f); // Blue tint
				}
				isFrozen = !isFrozen;
			}
		}
	}

	private void MoveOneTileToPlayer(double delta)
	{
		var player = GetTree().GetNodesInGroup("player").FirstOrDefault() as Node2D;
		Vector2 difference = player.GlobalPosition - GlobalPosition;

		var motionX = new Vector2I(Math.Sign(difference.X), 0);
		var motionY = new Vector2I(0, Math.Sign(difference.Y));
		
		// Determine the cardinal direction to move towards
		if (Math.Abs(difference.X) > Math.Abs(difference.Y))
		{
			if(!WillCollide(motionX))	// desired direction
			{
				MoveEnemy(motionX);
			}
			else
			{
				if(!WillCollide(motionY))	// try moving in other axis
				{
					MoveEnemy(motionY);
				}
				else if(!WillCollide(-motionY))
				{
					MoveEnemy(-motionY);
				}
				else if(!WillCollide(-motionX))
				{
					MoveEnemy(-motionX);	// lastly try to move back way we came
				}
				// else do not move
			}
		}
		else
		{
			if(!WillCollide(motionY))	// desired direction
			{
				MoveEnemy(motionY);
			}
			else
			{
				if(!WillCollide(motionX))	// try moving in other axis
				{
					MoveEnemy(motionX);
				}
				else if(!WillCollide(-motionX))
				{
					MoveEnemy(-motionX);
				}
				else if(!WillCollide(-motionY))
				{
					MoveEnemy(-motionY);	// lastly try to move back way we came
				}
				// else do not move
			}
		}
	}

	private void MoveEnemy(Vector2I direction)
	{		
		var currentTile = tileMapLayer.LocalToMap(GlobalPosition);
		var targetTile = new Vector2I()
		{
			X = currentTile.X + direction.X,
			Y = currentTile.Y + direction.Y
		};

		isMoving = true;
		GlobalPosition = tileMapLayer.MapToLocal(targetTile);
		sprite2D.GlobalPosition = tileMapLayer.MapToLocal(currentTile);
	}

	// Use RayCast to check if obj can move to target position
	private bool WillCollide(Vector2I direction)
	{
		rayCast2D.TargetPosition = direction * moveDistance;
		rayCast2D.ForceRaycastUpdate();

		return rayCast2D.IsColliding();		
	}
}
