using TeamRitual.Core;
using UnityEngine;

namespace TeamRitual.Character{
public class XoninUltimate1Start : CharacterState {
    public XoninUltimate1Start(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = false;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.CUSTOM;
        this.moveType = MoveType.AIR;
	    this.stateType = StateType.ATTACK;

        this.attackPriority = AttackPriority.SUPER;
        this.hitsToCancel = int.MaxValue;
        this.EXFlash = true;
        this.scalingStep = 0;

        this.animationName = this.character.characterName + "_Ultimate1Start";
    }

    public override void EnterState() {
        base.EnterState();
        this.character.VelX(0);
        this.character.VelY(0);
        GameController.Instance.Pause(20);
        GameController.Instance.playerPaused = this.character.playerNumber;
        GameController.Instance.CameraFocusCharacter(this.character);
        GameController.Instance.SetCameraZoom(4f);
        GameController.Instance.SetCameraLerp(20f);
    }
    public override void UpdateState() {
        base.UpdateState();

        if (this.stateTime == 20) {
            this.character.VelX(40);
            GameController.Instance.ResetCameraLerp();
            GameController.Instance.CameraFocusReset();
            GameController.Instance.ResetCameraZoom();
        }
        if (this.stateTime > 20) {
            this.character.VelXDirect(this.character.VelX()*0.95f);
        }

        if (this.moveHit > 0) {
            this.SwitchState((this.factory as XoninStateFactory).Ultimate1Punching1());
        }

        if (this.character.anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) {
            this.SwitchState(this.factory.Airborne());
        }
    }
}
}