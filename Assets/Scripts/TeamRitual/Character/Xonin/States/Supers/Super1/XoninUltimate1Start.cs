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
    }
    public override void UpdateState() {
        base.UpdateState();

        if (this.stateTime > 40) {
            this.SwitchState((this.factory as XoninStateFactory).Ultimate1Punching1());
        }
    }
}
}