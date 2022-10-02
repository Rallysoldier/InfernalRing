using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateJumpLand : CharacterState
{
    public CommonStateJumpLand(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {

    }

    public override void EnterState() {
        base.EnterState();
        this.character.anim.SetTrigger("JumpLand");
    }

    public override void UpdateState() {
        base.UpdateState();

        if (this.stateTime == 4) {
            this.character.body.MovePosition(
                new Vector2(this.character.body.position.x,(float)Math.Ceiling(this.character.body.position.y))
            );
            this.SwitchState(this.character.states.Stand());
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