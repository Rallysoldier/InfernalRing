using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateHurtCrouch : CharacterState
{
    public CommonStateHurtCrouch(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.CROUCH;
        this.moveType = MoveType.CROUCH;
	    this.stateType = StateType.HURT;

        this.animationName = this.character.characterName + "_HurtCrouch";
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        base.UpdateState();

        if (stateTime > this.character.hitstun)
            this.SwitchState(this.character.states.Crouch());
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