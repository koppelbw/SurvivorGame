using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class UpgradeManager : Node
{
	[Export]
	public AbilityUpgrade[] UpgradePool {get; private set;}

	[Export]
	public ExperienceManager ExperienceManager {get; private set;}

	public Dictionary<string, int> CurrentUpgrades = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ExperienceManager.LevelUp += OnLevelUp;
	}

	private void OnLevelUp(int currentLevel)
	{
		Random random = new(); 
		var randIndex = random.Next(UpgradePool.Length);
		var chosenUpgrade = UpgradePool[randIndex];
		if(chosenUpgrade is null) return;

		var hasUpgrade = CurrentUpgrades.ContainsKey(chosenUpgrade.Id);
		if(!hasUpgrade)
		{
			CurrentUpgrades.Add(chosenUpgrade.Id, 1);
		}
		else
		{
			CurrentUpgrades[chosenUpgrade.Id] += 1;
		}

		foreach(var upgrade in CurrentUpgrades)
		{
			GD.Print(upgrade.Key + ":" + upgrade.Value);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
