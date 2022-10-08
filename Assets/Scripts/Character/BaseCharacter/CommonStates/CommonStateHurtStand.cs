using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateHurtStand : CharacterState
{
    public CommonStateHurtStand(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.STAND;
        this.moveType = MoveType.STAND;
	    this.stateType = StateType.HURT;

        this.animationName = this.character.characterName + "_HurtStand";
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        base.UpdateState();

        if (stateTime >= this.character.hitstun)
            this.SwitchState(this.character.states.Stand());
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