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

        this.animationName = this.character.characterName + "_RunForward";
    }

    public override void EnterState() {
        base.EnterState();

        this.character.SetVelocity(this.character.velocityRunForward);
    }

    public override void UpdateState() {
        base.UpdateState();

        this.character.SetVelocity(this.character.velocityRunForward);

        if (!this.character.inputHandler.held(this.character.inputHandler.ForwardInput(this.character)) && this.stateTime > 4) {
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