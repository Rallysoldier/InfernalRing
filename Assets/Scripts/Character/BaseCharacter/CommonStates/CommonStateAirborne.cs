using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateAirborne : CharacterState
{
    public CommonStateAirborne(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = true;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = true;

        this.physicsType = PhysicsType.AIR;
        this.moveType = MoveType.AIR;
	    this.stateType = StateType.IDLE;
    }

    public override void EnterState() {
        base.EnterState();
        this.character.anim.SetTrigger("Airborne");
    }

    public override void UpdateState() {
        base.UpdateState();

        if (this.character.body.position.y <= 0) {
            this.SwitchState(this.character.states.JumpLand());
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