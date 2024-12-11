using Godot;

public partial class ExperienceVial : Node2D
{
	private GameEvents gameEvents;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameEvents = (GameEvents)GetNode("/root/GameEvents");

		var myArea2D = GetNode<Area2D>("Area2D");
		myArea2D.AreaEntered += AreaEntered;
	}

	private void AreaEntered(Area2D area2D)
	{
		gameEvents.EmitSignalExperienceVialCollected(1);
		QueueFree();
	}
}
