using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateHurtBounce : CharacterState
{
    public CommonStateHurtBounce(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = false;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.AIR;
        this.moveType = MoveType.LYING;
	    this.stateType = StateType.HURT;

        this.animationName = this.character.characterName + "_HurtBounce";
    }

    public override void EnterState() {
        base.EnterState();
        this.character.SetVelY(7);
    }

    public override void UpdateState() {
        base.UpdateState();

        if (this.character.body.position.y <= 0.2 && this.character.VelY() < 0) {
            this.character.SetVelY(0);
            this.SwitchState(this.character.states.LyingDown());
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