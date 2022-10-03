using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateJumpStart : CharacterState
{
    Vector2 jumpVelocity;

    public CommonStateJumpStart(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.STAND;
        this.moveType = MoveType.CROUCH;
	    this.stateType = StateType.IDLE;
    }

    public override void EnterState() {
        base.EnterState();
        this.character.anim.SetTrigger("JumpStart");

        if (this.character.inputHandler.heldKeys.Contains(this.character.inputHandler.ForwardInput(this.character))) {
            jumpVelocity = this.character.velocityJumpForward;
        } else if (this.character.inputHandler.heldKeys.Contains(this.character.inputHandler.BackInput(this.character))) {
            jumpVelocity = this.character.velocityJumpBack;
        } else {
            jumpVelocity = this.character.velocityJumpNeutral;
        }
    }

    public override void UpdateState() {
        base.UpdateState();

        if (this.stateTime == 4) {
            this.SwitchState(this.character.states.Airborne());
        }
    }

    public override void ExitState() {
        base.ExitState();

        this.character.SetVelocity(jumpVelocity);
    }

    public override void InitializeSubState() {
        base.InitializeSubState();
    }

    public override void CheckSwitchState() {
        base.CheckSwitchState();
    }
}