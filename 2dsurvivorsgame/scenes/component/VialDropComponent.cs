using Godot;

public partial class VialDropComponent : Node
{
    [Export(PropertyHint.Range, "0,1")]
    private float DropPercent = 0.5f;

    [Export]
    private PackedScene VialScene;

    [Export]
    private HealthComponent HealthComponent;

    public override void _Ready()
	{
        HealthComponent.Died += OnDied;
    }

    private void OnDied()
    {
        if(GD.Randf() > DropPercent) return;
        if(VialScene is null) return;
        if(Owner is not Node2D) return;

        var spawnPosition = (Owner as Node2D).GlobalPosition;
        var vialInstance = VialScene.Instantiate() as Node2D;
        Owner.GetParent().AddChild(vialInstance);
        vialInstance.GlobalPosition = spawnPosition;
    }
}
