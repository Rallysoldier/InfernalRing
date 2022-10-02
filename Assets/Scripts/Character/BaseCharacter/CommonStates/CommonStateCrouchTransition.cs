using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateCrouchTransition : CharacterState
{
    public CommonStateCrouchTransition(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {

    }

    public override void EnterState() {
        base.EnterState();
        this.character.anim.SetTrigger("CrouchTransition");
    }

    public override void UpdateState() {
        base.UpdateState();
    }

    public override void ExitState() {
        base.ExitState();
    }

    public override void InitializeSubState() {
        base.InitializeSubState();
    }

    public override void CheckSwitchState() {
        base.CheckSwitchState();
    }
}