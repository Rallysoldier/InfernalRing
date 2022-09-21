using UnityEngine;

public abstract class XoninBaseState
{
	protected XoninStateMachine ctx;
	protected XoninStateFactory factory;
	public XoninBaseState(XoninStateMachine currentContext, XoninStateFactory XoninStateFactory)
	{
		ctx = currentContext;
		factory = playerStateFactory;
	}
	
	public abstract void EnterState();
	
	public abstract void UpdateState();
	
	public abstract void ExitState();
	
	public abstract void CheckSwitchStates();
	
	public abstract void InitializeSubState();
	
	void UpdateStates() { }


	protected void SwitchState(XoninBaseState newState)
	{
		// exit current state
		ExitState();

		//enter new state
		newState.EnterState();

		//update context of state
		ctx.currentState = newState;
	}


	protected void SetSuperState()
	{

	}

	protected void SetSubState()
	{

	}
}