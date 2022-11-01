using UnityEngine;

namespace TeamRitual.Character {
public class XoninRunBack : CharacterState
{
    public XoninRunBack(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.CUSTOM;
        this.moveType = MoveType.AIR;
	    this.stateType = StateType.IDLE;

        this.animationName = this.character.characterName + "_RunBack";
    }

    public override void EnterState() {
        base.EnterState();
        this.character.SetVelocity(this.character.velocityRunBack);
    }

    public override void UpdateState() {
        base.UpdateState();

        if (stateTime >= 1) {
            this.character.VelY(this.character.VelY()-0.5f);
        }

        if (this.stateTime == 4) {
            this.character.Flash(new Vector4(1.5f,1.5f,1.5f,1f),10);
            this.character.MakeInvincible();
        }

        if (this.character.body.position.y <= 0.2 && this.character.VelY() < 0) {
            this.character.VelY(0);
            this.character.ClearInvincibility();
            this.SwitchState(this.character.states.JumpLand());
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