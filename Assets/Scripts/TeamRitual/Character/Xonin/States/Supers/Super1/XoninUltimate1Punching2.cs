using UnityEngine;

namespace TeamRitual.Character{
public class XoninUltimate1Punching2 : CharacterState {
    public XoninUltimate1Punching2(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = false;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.CUSTOM;
        this.moveType = MoveType.AIR;
	    this.stateType = StateType.ATTACK;

        this.attackPriority = AttackPriority.SUPER;
        this.hitsToCancel = int.MaxValue;
        this.scalingStep = 0;

        this.animationName = this.character.characterName + "_Ultimate1Punching2";
    }

    public override void EnterState() {
        base.EnterState();
    }
    public override void UpdateState() {
        base.UpdateState();

        this.character.VelX(0f);
        this.character.VelY(0);

        if (this.stateTime > 60) {
            this.SwitchState((this.factory as XoninStateFactory).Ultimate1Punching3());
        }
    }
}
}