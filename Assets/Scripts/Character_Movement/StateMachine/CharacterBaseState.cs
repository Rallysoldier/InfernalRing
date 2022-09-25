using UnityEngine;

public abstract class CharacterBaseState
{
	protected CharacterStateMachine ctx;
	protected CharacterStateFactory factory;
	public CharacterBaseState(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
	{
		ctx = currentContext;
		factory = CharacterStateFactory;
	}

	public abstract void EnterState();

	public abstract void UpdateState();

	public abstract void ExitState();

	public abstract void CheckSwitchState();

	public abstract void InitializeSubState();

	void UpdateStates()
	{
		CheckSwitchState();
	}


	protected void SwitchState(CharacterBaseState newState)
	{
		// exit current state
		ExitState();

		//enter new state
		newState.EnterState();

		//update context of state
		//ctx.currentState = newState;

	}


	protected void SetSuperState()
	{

	}

	protected void SetSubState()
	{

	}
}