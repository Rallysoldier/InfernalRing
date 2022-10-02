using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateJumpStart : CharacterState
{
    public CommonStateJumpStart(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {

    }

    public override void EnterState() {
        base.EnterState();
        this.character.anim.SetTrigger("JumpStart");

        this.character.SetVelocity(0,5);
    }

    public override void UpdateState() {
        base.UpdateState();

        if (this.stateTime == 4) {
            this.SwitchState(this.character.states.Airborne());
        }
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