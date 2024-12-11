using Godot;

public partial class ExperienceBar : CanvasLayer
{
	[Export]
	public ExperienceManager ExperienceManager;

	private ProgressBar ProgressBar;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ProgressBar = (ProgressBar)GetNode("MarginContainer/ProgressBar");
		ProgressBar.Value = 0;
		ExperienceManager.ExperienceUpdated += OnExperienceUpdated;
	}

	private void OnExperienceUpdated(int currentExperience, int targetExperience)
	{
		float percent = (float)currentExperience / targetExperience;
		ProgressBar.Value = percent;
	}
}
