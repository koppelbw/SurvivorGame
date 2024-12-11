using Godot;
using System;

public partial class HurtBoxComponent : Area2D
{
	[Export]
	public HealthComponent HealthComponent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D incomingArea)
	{
		if(incomingArea is not HitboxComponent) return;
		if(HealthComponent is null) return;

		var hitboxComponent = incomingArea as HitboxComponent;
		HealthComponent.TakeDamage(hitboxComponent.Damage);
	}

	// // Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
