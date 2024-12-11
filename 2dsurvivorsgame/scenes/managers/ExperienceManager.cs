using Godot;
using System;

public partial class ExperienceManager : Node
{
	[Signal]
	public delegate void ExperienceUpdatedEventHandler(int currentExperience, int targetExperience);

	[Signal]
	public delegate void LevelUpEventHandler(int newLevel);

	private const int TARGET_EXPERIENCE_GROWTH = 5;

	private int currentExperience = 0;
	public int currentLevel = 1;
	public int targetExperience = 5;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var gameEvents = (GameEvents)GetNode("/root/GameEvents");
		gameEvents.ExperienceVialCollected += IncrementExperience;
	}

	public void IncrementExperience(int number)
	{		
		currentExperience = currentExperience + number;//Math.Min(currentExperience + number, targetExperience);
		EmitSignal(nameof(ExperienceUpdated), currentExperience, targetExperience);

		if(currentExperience == targetExperience)
		{
			currentExperience += 1;
			targetExperience += TARGET_EXPERIENCE_GROWTH;
			currentExperience = 0;
			EmitSignal(nameof(ExperienceUpdated), currentExperience, targetExperience);
			EmitSignal(nameof(LevelUp), currentLevel);
		}
	}
}
