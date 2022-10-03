using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateCrouchTransition : CharacterState
{
    bool standToCrouch = true;
    public CommonStateCrouchTransition(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = true;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = true;

        this.physicsType = PhysicsType.CROUCH;
        this.moveType = MoveType.CROUCH;
	    this.stateType = StateType.IDLE;
    }

    public override void EnterState() {
        base.EnterState();
        this.character.anim.SetTrigger("CrouchTransition");

        if(this.character.currentState == this.character.states.Crouch())
            standToCrouch = false;
    }

    public override void UpdateState() {
        base.UpdateState();

        if (this.stateTime >= 3) {
            if (standToCrouch) {
                this.SwitchState(this.character.states.Crouch());
            } else {
                this.SwitchState(this.character.states.Stand());
            }
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