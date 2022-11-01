using TeamRitual.Core;
using UnityEngine;

namespace TeamRitual.Character {
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
        if (this.character.lastContact.HitFall && this.character.lastContact.FallingGravity > 0) {
            this.character.gravity = this.character.lastContact.FallingGravity;
        }
    }

    public override void UpdateState() {
        base.UpdateState();

        if (this.character.health == 0) {
            if (this.stateTime < 3)
                this.character.SetVelocity(-4,7);
        } else if (this.character.hitstun == 0) {
            if (this.character.lastContact.FallRecover &&
                (this.character.inputHandler.held("L") || this.character.inputHandler.held("M") || this.character.inputHandler.held("H"))) {
                this.SwitchState(this.character.states.Recover());
                return;
            } else if (!this.character.lastContact.HitFall) {
                this.SwitchState(this.character.states.Airborne());
                return;
            }
        }

        if (this.character.body.position.y <= 0 && this.character.VelY() < 0) {
            this.character.VelY(0);
            if (this.character.lastContact.HitFall || this.character.health == 0) {
                this.SwitchState(this.character.states.HurtBounce());
            } else {
                this.SwitchState(this.character.states.JumpLand());
            }
        } else if (this.character.lastContact.WallBounceTime > 0 && this.stateTime > 5) {
            float posX = this.character.PosX();

            if (posX >= GameController.Instance.StageMaxBound() - 1.2f || posX <= GameController.Instance.StageMinBound() + 1.2f) {
                this.character.VelX(0);
                this.SwitchState(this.character.states.HurtWallBounce());
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