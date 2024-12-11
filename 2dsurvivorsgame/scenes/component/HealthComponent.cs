using Godot;
using System;

public partial class HealthComponent : Node
{
	[Export]
	public int MaxHealth = 10;
	public int CurrentHealth;


	[Signal]
	public delegate void HealthChangedEventHandler(int change);

	[Signal]
	public delegate void DiedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
	}

	public void TakeDamage(int amount)
	{
		CurrentHealth -= amount;
		EmitSignal(nameof(HealthChanged), CurrentHealth);

		CallDeferred("CheckDeath");
	}	

	public void Heal(int amount)
	{
		CurrentHealth += amount;

		if(CurrentHealth > MaxHealth)
		{
			CurrentHealth = MaxHealth;
		}

		EmitSignal(nameof(HealthChanged), CurrentHealth);
	}

	public void CheckDeath()
	{
		if(CurrentHealth <= 0)
		{
			CurrentHealth = 0;
			EmitSignal(nameof(Died));
			Owner.QueueFree();
		}
	}
}
