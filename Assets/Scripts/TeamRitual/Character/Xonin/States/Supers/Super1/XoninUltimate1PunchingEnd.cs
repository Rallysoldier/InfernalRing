using TeamRitual.Core;
using UnityEngine;

namespace TeamRitual.Character{
public class XoninUltimate1PunchingEnd : CharacterState {
    public XoninUltimate1PunchingEnd(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = false;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.AIR;
        this.moveType = MoveType.AIR;
	    this.stateType = StateType.ATTACK;

        this.attackPriority = AttackPriority.SUPER;
        this.hitsToCancel = int.MaxValue;
        this.EXFlash = true;
        this.scalingStep = 0;

        this.animationName = this.character.characterName + "_Ultimate1End";
    }

    public override void EnterState() {
        base.EnterState();
        this.character.VelX(5);
        this.character.VelY(25);
    }
    public override void UpdateState() {
        base.UpdateState();

        if (this.moveHit == 1) {
            GameController.Instance.SetCameraZoom(4f);
        }

        if (this.character.anim.GetCurrentAnimatorStateInfo(0).IsName(this.character.characterName + "_Ultimate1End")
            && this.character.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) {
            GameController.Instance.ResetCameraZoom();
            this.SwitchState(this.factory.Airborne());
        }
    }
}
}