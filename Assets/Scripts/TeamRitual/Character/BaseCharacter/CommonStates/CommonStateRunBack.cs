using UnityEngine;

namespace TeamRitual.Character {
public class CommonStateRunBack : CharacterState
{
    public CommonStateRunBack(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = true;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.STAND;
        this.moveType = MoveType.STAND;
	    this.stateType = StateType.IDLE;

        this.animationName = this.character.characterName + "_RunBack";
    }

    public override void EnterState() {
        base.EnterState();

        this.character.SetVelocity(this.character.velocityRunBack);
    }

    public override void UpdateState() {
        base.UpdateState();

        this.character.SetVelocity(this.character.velocityRunBack);

        if (this.stateTime == 3) {
            this.character.Flash(new Vector4(1.5f,1.5f,1.5f,1f),6);
            this.character.MakeInvincible();
        }
        if (this.stateTime == 9) {
            this.character.ClearInvincibility();
        }

        if (!this.character.inputHandler.held(this.character.inputHandler.BackInput(this.character)) && this.stateTime > 4) {
            this.character.ClearInvincibility();
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
}