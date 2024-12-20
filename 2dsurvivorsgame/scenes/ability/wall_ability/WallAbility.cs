using Godot;

public partial class WallAbility : Node2D
{
	[Export]
	private Timer Timer{get;set;}

	[Export]
	private GameItems GameItems {get;set;}

	public override void _Ready()
	{
		Timer.Timeout += OnTimerTimeout;
	}

	private void OnTimerTimeout()
	{
		GameItems.WallCount--;
		QueueFree();
	}
}
