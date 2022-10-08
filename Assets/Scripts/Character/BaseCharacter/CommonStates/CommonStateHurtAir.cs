using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateHurtAir : CharacterState
{
    public CommonStateHurtAir(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.AIR;
        this.moveType = MoveType.AIR;
	    this.stateType = StateType.HURT;

        this.animationName = this.character.characterName + "_HurtAir";
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        base.UpdateState();

        if (stateTime > this.character.hitstun && this.character.health > 0)
            this.SwitchState(this.character.states.Airborne());

        if (this.character.body.position.y <= 0.2 && this.character.VelY() < 0) {
            this.character.SetVelY(0);
            this.SwitchState(this.character.states.HurtBounce());
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