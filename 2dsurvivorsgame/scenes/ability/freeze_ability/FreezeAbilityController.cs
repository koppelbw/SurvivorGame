using Godot;

public partial class FreezeAbilityController : Node
{
	[Export]
	public PackedScene FreezeAbility {get; private set;}

	private FreezeAbility freezeAbility;
	public bool isFrozen = false;

	public override void _Ready()
	{
	}

	public void PlayFreeze()
	{
		isFrozen = true;
		freezeAbility = FreezeAbility.Instantiate() as FreezeAbility;
		AddChild(freezeAbility);
	}

	public void UnFreeze()
	{
		isFrozen = false;
		freezeAbility.QueueFree();
	}
}
