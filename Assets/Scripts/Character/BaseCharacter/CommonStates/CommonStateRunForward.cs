using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateRunForward : CharacterState
{
    public CommonStateRunForward(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = true;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.STAND;
        this.moveType = MoveType.STAND;
	    this.stateType = StateType.IDLE;
    }

    public override void EnterState() {
        base.EnterState();
        this.character.anim.SetTrigger("RunForward");

        this.character.SetVelocity(this.character.velocityRunForward);
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