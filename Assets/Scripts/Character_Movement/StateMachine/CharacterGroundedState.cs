using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroundedState : CharacterBaseState
{
    public CharacterGroundedState(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {

    }


    public override void EnterState() { }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void InitializeSubState()
    {

    }

    public override void CheckSwitchState()
    {

    }
}