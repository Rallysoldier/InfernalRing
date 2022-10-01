using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDashForwardState : CharacterBaseState
{
    public CharacterDashForwardState(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {

    }

    public override void EnterState()
    {
        Debug.Log("Entering DashForward State");
        ctx.Anim.SetTrigger("Dash_FWD");
        ctx.LastDashTime = Time.time;

    }

    public override void UpdateState()
	{
		CheckSwitchState();
	}
    public override void ExitState()
    {
    }

    public override void InitializeSubState() { }

    public override void CheckSwitchState()
    {

    }
}