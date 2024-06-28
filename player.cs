using Godot;
using System;

public partial class player : CharacterBody2D
{
	public const float Speed = 60.0f;

	private NavigationAgent2D _navigationAgent;


	public override void _Ready()
	{
		_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
	}
	public override void _PhysicsProcess(double delta)
	{
		if(_navigationAgent.IsTargetReachable())
		{
			Velocity = GlobalTransform.Origin.DirectionTo(_navigationAgent.GetNextPathPosition())*Speed;
			MoveAndSlide();
		}

		Vector2 Direction = new Vector2(0,0);
		if(Input.IsActionPressed("ui_up"))
		{
			Direction+=new Vector2(0,-1);
		}
		if(Input.IsActionPressed("ui_down"))
		{
			Direction+=new Vector2(0,1);
		}
		if(Input.IsActionPressed("ui_left"))
		{
			Direction+=new Vector2(-1,0);
		}
		if(Input.IsActionPressed("ui_right"))
		{
			Direction+=new Vector2(1,0);
		}
		Direction = Direction.Normalized();
		Direction *= Speed;
		
		Vector2 Target_position = this.Position + Direction;
		_navigationAgent.TargetPosition = Target_position;
		
		
	}
}
