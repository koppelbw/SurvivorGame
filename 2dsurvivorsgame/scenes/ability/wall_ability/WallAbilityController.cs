using Godot;

public partial class WallAbilityController : Node
{
	[Export]
	private PackedScene WallAbility{get;set;}

	[Export]
	private GameItems GameItems{get;set;}

	public int maxWalls = 3;
	private int currentNumberOfWalls = 0;

	public override void _Ready()
	{
	}

	public void CreateWall()
	{
		GD.Print("controller create wall: " + GameItems.WallCount);
		if(GameItems.WallCount < maxWalls)
		{
			GD.Print("here");
			GameItems.WallCount++;

			var player = GetParent().GetParent() as Node2D;
			var newWall = WallAbility.Instantiate() as Node2D;
			newWall.GlobalPosition = player.GlobalPosition;
			player.GetParent().AddChild(newWall);
		}
	}
}
