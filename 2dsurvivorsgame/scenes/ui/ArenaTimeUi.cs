using System;
using Godot;

public partial class ArenaTimeUi : CanvasLayer
{
	
	[Export]
	private ArenaTimeManager ArenaTimeManager{get;set;}

	private Label myLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		myLabel = GetNode<Label>("MarginContainer/Label");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(ArenaTimeManager is null)
		{
			return;
		}
		
		var timeElapsed = ArenaTimeManager.GetTimeElapsed();
		var minutes = (int)(timeElapsed / 60);
		var remainingSeconds = (int)(timeElapsed % 60); 				
		myLabel.Text = $"{minutes}:{remainingSeconds:D2}";		
	}

}
