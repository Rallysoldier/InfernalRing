using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDashBackState : CharacterBaseState
{
    public CharacterDashBackState(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {

    }

    public override void EnterState() { }

    public override void UpdateState()
	{
		CheckSwitchState();
	}
    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchState()
    {

    }
}