using UnityEngine;

namespace TeamRitual.Character {
public class CommonStateAirjumpStart : CharacterState
{
    public Vector2 jumpVelocity = Vector2.zero;

    public CommonStateAirjumpStart(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.AIR;
        this.moveType = MoveType.AIR;
	    this.stateType = StateType.IDLE;

        this.animationName = this.character.characterName + "_Airjump";
    }

    public override void EnterState() {
        base.EnterState();

        this.character.airjumpCount++;

        if (jumpVelocity == Vector2.zero) {
            if (this.character.inputHandler.held(this.character.inputHandler.ForwardInput(this.character))) {
                jumpVelocity = this.character.velocityJumpForward;
            } else if (this.character.inputHandler.held(this.character.inputHandler.BackInput(this.character))) {
                jumpVelocity = this.character.velocityJumpBack;
            } else {
                jumpVelocity = this.character.velocityJumpNeutral;
            }
        }
    }

    public override void UpdateState() {
        base.UpdateState();

        if (this.stateTime == 4) {
            this.SwitchState(this.character.states.Airborne());
            this.character.SetVelocity(new Vector2(jumpVelocity.x,jumpVelocity.y/1.1f));
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
}