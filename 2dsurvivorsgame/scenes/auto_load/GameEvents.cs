using Godot;

public partial class GameEvents : Node
{
	[Signal]
	public delegate void ExperienceVialCollectedEventHandler(int number);

	public void EmitSignalExperienceVialCollected(int number)
	{
		EmitSignal(nameof(ExperienceVialCollected), number);
	}
}
