using Godot;

public partial class FreezeAbility : Node2D
{
	public override void _Ready()
	{
		var basicEnemy = GetParent().GetParent() as Node2D;
		GlobalPosition = basicEnemy.GlobalPosition;
	}
}
