using UnityEngine;

namespace TeamRitual.Character {
public class CommonStateJumpStart : CharacterState
{
    Vector2 jumpVelocity;
    StateType prevStateType;

    public CommonStateJumpStart(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.STAND;
        this.moveType = MoveType.STAND;
	    this.stateType = StateType.IDLE;

        this.animationName = this.character.characterName + "_JumpStart";
    }

    public override void EnterState() {
        base.EnterState();

        if (this.character.inputHandler.held(this.character.inputHandler.ForwardInput(this.character))) {
            jumpVelocity = this.character.velocityJumpForward;
        } else if (this.character.inputHandler.held(this.character.inputHandler.BackInput(this.character))) {
            jumpVelocity = this.character.velocityJumpBack;
        } else {
            jumpVelocity = this.character.velocityJumpNeutral;
        }

        float prevVelX = this.character.VelX()/this.character.standingFriction;
        if (Mathf.Sign(prevVelX * this.character.facing) == Mathf.Sign(jumpVelocity.x) && Mathf.Abs(prevVelX) > Mathf.Abs(jumpVelocity.x)) {
            jumpVelocity = new Vector2(prevVelX * this.character.facing, jumpVelocity.y);
        }

        prevStateType = this.character.currentState.stateType;
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
}