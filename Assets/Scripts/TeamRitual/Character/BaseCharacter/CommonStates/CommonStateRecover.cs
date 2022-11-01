
using UnityEngine;

namespace TeamRitual.Character {
public class CommonStateRecover : CharacterState
{
    Color oldColor;

    public CommonStateRecover(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = true;

        this.physicsType = PhysicsType.CUSTOM;
        this.moveType = MoveType.AIR;
	    this.stateType = StateType.IDLE;

        this.animationName = this.character.characterName + "_Airborne";
    }

    public override void EnterState() {
        base.EnterState();

        Vector2 recoverVelocity = new Vector2(0,10);
        if (this.character.inputHandler.held(this.character.inputHandler.BackInput(this.character))) {
            recoverVelocity = new Vector2(-5,8);
        } else if (this.character.inputHandler.held(this.character.inputHandler.ForwardInput(this.character))) {
            recoverVelocity = new Vector2(5,8);
        }

        this.character.SetVelocity(recoverVelocity);
        this.character.MakeInvincible();
        oldColor = this.character.spriteRenderer.color;
    }

    public override void UpdateState() {
        base.UpdateState();

        this.character.spriteRenderer.color = Color.Lerp(new Vector4(20f,20f,20f,1f), oldColor, Time.deltaTime * 15);

        if (this.stateTime > 6) {
            this.character.ClearInvincibility();
            this.character.spriteRenderer.color = oldColor;
            if (this.character.body.position.y <= 0.2 && this.character.VelY() < 0) {
                this.character.VelY(0);
                this.SwitchState(this.character.states.JumpLand());
            } else {
                this.SwitchState(this.character.states.Airborne());
            }
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