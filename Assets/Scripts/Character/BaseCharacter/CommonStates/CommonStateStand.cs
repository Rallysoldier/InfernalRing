using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateStand : CharacterState
{
    public CommonStateStand(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = true;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = true;

        this.physicsType = PhysicsType.STAND;
        this.moveType = MoveType.STAND;
	    this.stateType = StateType.IDLE;

        //this.character.anim.SetBool("key", bool);
    }

    public override void EnterState() {
        base.EnterState();
        this.character.anim.SetTrigger("Stand");
        this.character.SetVelocity(this.character.VelX(),0);
        this.character.body.position = new Vector2(this.character.PosX(),0);
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