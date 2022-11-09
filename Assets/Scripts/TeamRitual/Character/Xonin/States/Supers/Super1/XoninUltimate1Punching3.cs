using UnityEngine;

namespace TeamRitual.Character{
public class XoninUltimate1Punching3 : CharacterState {
    public XoninUltimate1Punching3(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
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

        this.animationName = this.character.characterName + "_Ultimate1Punching3";
    }

    public override void EnterState() {
        base.EnterState();
    }
    public override void UpdateState() {
        base.UpdateState();

        this.character.VelX(0f);
        this.character.VelY(0);

        if (this.stateTime > 200) {
            this.SwitchState((this.factory as XoninStateFactory).Ultimate1PunchingEnd());
        }
    }
}
}