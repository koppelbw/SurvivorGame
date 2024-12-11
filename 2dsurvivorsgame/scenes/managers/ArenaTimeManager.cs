using Godot;
using System;

public partial class ArenaTimeManager : Node
{
	private Timer myTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		myTimer = GetNode<Timer>("Timer");
	}

	public double GetTimeElapsed()
	{
		return myTimer.WaitTime - myTimer.TimeLeft;
	}
}
