using Godot;

public partial class Player : Node2D
{
	private TileMapLayer tileMapLayer;
	private Sprite2D sprite2D;
	private RayCast2D rayCast2D;
	private WallAbilityController wallAbilityController;

	public float playerMovementSpeed = 2f;
	public bool isMoving = false;

	public override void _Ready()
	{
		tileMapLayer = GetParent().GetNode<TileMapLayer>("TileMapLayer");
		sprite2D = GetNode<Sprite2D>("Sprite2D");
		rayCast2D = GetNode<RayCast2D>("RayCast2D");
		wallAbilityController = GetNode<WallAbilityController>("AbilityManager/WallAbilityController");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if(!isMoving) return;
		
		sprite2D.GlobalPosition = sprite2D.GlobalPosition.MoveToward(GlobalPosition, playerMovementSpeed);

		if(GlobalPosition == sprite2D.GlobalPosition)
		{
			isMoving = false;
		}
	}

	public override void _Process(double delta)
	{
		// Tile based movement (https://www.youtube.com/watch?v=9u1Dq6h7sGU&t=46s)
		if(isMoving) return;
		
		if(Input.IsActionJustPressed("wall"))
		{
			wallAbilityController.CreateWall();
		}

		if(Input.IsActionPressed("move_up"))
		{
			if(Input.IsActionPressed("move_left"))
			{
				MovePlayer(new Vector2I(-1, -1));
			}
			else if(Input.IsActionPressed("move_right"))
			{
				MovePlayer(new Vector2I(1, -1));
			}
			else
			{
				MovePlayer(Vector2I.Up);
			}
		}
		else if(Input.IsActionPressed("move_down"))
		{
			if(Input.IsActionPressed("move_left"))
			{
				MovePlayer(new Vector2I(-1, 1));
			}
			else if(Input.IsActionPressed("move_right"))
			{
				MovePlayer(new Vector2I(1, 1));
			}
			else
			{
				MovePlayer(Vector2I.Down);
			}
		}
		else if(Input.IsActionPressed("move_left"))
		{
			if(Input.IsActionPressed("move_up"))
			{
				MovePlayer(new Vector2I(-1, -1));
			}
			else if(Input.IsActionPressed("move_down"))
			{
				MovePlayer(new Vector2I(-1, 1));
			}
			else
			{
				MovePlayer(Vector2I.Left);
			}
		}
		else if(Input.IsActionPressed("move_right"))
		{
			if(Input.IsActionPressed("move_up"))
			{
				MovePlayer(new Vector2I(1, -1));
			}
			else if(Input.IsActionPressed("move_down"))
			{
				MovePlayer(new Vector2I(1, 1));
			}
			else
			{
				MovePlayer(Vector2I.Right);
			}
		}
	}

	private void MovePlayer(Vector2I direction)
	{
		var currentTile = tileMapLayer.LocalToMap(GlobalPosition);
		var targetTile = new Vector2I()
		{
			X = currentTile.X + direction.X,
			Y = currentTile.Y + direction.Y
		};


		// TODO: Determine if ground is walkable or not
		// var tileData = tileMapLayer.GetCellTileData(targetTile);
		// var customData = tileData.GetCustomData("floor");		
		// Variant floorData; 
		// if (tileData.Custom.TryGetValue("floor", out floorData))
		// {
		// }

		// Use RayCast to check if player can move to target position
		rayCast2D.TargetPosition = direction * 16;
		rayCast2D.ForceRaycastUpdate();
		if(rayCast2D.IsColliding()) return;

		isMoving = true;
		GlobalPosition = tileMapLayer.MapToLocal(targetTile);
		sprite2D.GlobalPosition = tileMapLayer.MapToLocal(currentTile);
	}
}
