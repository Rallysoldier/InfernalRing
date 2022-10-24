using UnityEngine;

namespace TeamRitual.Character {
public class XoninCrouchHeavyAttack : CharacterState
{
    public XoninCrouchHeavyAttack(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.CROUCH;
        this.moveType = MoveType.CROUCH;
	    this.stateType = StateType.ATTACK;

        this.attackPriority = AttackPriority.HEAVY;

        this.animationName = this.character.characterName + "_CrouchHeavyAttack";
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        base.UpdateState();

        if (this.moveHit > 0 && this.character.enemy.VelY() > 0) {
            if ((this.character.changedInput && (this.character.inputStr.EndsWith("U") ||
            this.character.inputStr.EndsWith("U,F") || this.character.inputStr.EndsWith("F,U") ||
            this.character.inputStr.EndsWith("U,B")  || this.character.inputStr.EndsWith("B,U")))
                     || this.character.inputHandler.held("U")) {
                    CommonStateJumpStart jumpStart = this.character.states.JumpStart() as CommonStateJumpStart;
                    Vector2 hitVelocity = this.character.enemy.lastContact.HitVelocity;
                    jumpStart.jumpVelocity = new Vector2(-hitVelocity.x, hitVelocity.y);
                    this.SwitchState(jumpStart);
                }
        }

        if (this.character.anim.GetCurrentAnimatorStateInfo(0).IsName(this.animationName)
            && this.character.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            this.SwitchState(this.character.states.Crouch());
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