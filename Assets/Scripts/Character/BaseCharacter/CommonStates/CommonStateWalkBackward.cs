using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateWalkBackward : CharacterState
{
    public CommonStateWalkBackward(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = true;
        this.faceEnemyStart = false;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.STAND;
        this.moveType = MoveType.STAND;
	    this.stateType = StateType.IDLE;
    }

    public override void EnterState() {
        base.EnterState();
        this.character.anim.SetTrigger("WalkBackward");

        this.character.SetVelocity(this.character.velocityWalkBack);
    }

    public override void UpdateState() {
        base.UpdateState();
        
        this.character.SetVelocity(this.character.velocityWalkBack);
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