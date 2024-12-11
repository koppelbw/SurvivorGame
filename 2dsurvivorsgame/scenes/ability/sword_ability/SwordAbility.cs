using Godot;
using System;

public partial class SwordAbility : Node2D
{
	public HitboxComponent HitboxComponent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		HitboxComponent = GetNode<HitboxComponent>("HitboxComponent");
	}

	// // Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
