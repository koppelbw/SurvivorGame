using Godot;

public partial class DiamondAbility : Node2D
{
	public HitboxComponent HitboxComponent;

	// // Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		HitboxComponent = GetNode<HitboxComponent>("HitboxComponent");		
	}
}
