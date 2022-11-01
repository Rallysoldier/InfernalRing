using UnityEngine;

namespace TeamRitual.Character {
public class CommonStateAirdashBack : CharacterState
{
    public Vector2 jumpVelocity = Vector2.zero;

    public CommonStateAirdashBack(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.AIR;
        this.moveType = MoveType.AIR;
	    this.stateType = StateType.IDLE;

        this.animationName = this.character.characterName + "_Airjump";
    }

    public override void EnterState() {
        base.EnterState();

        this.character.airdashCount++;
        this.character.SetVelocity(this.character.velocityAirdashBack);
        this.character.Flash(new Vector4(1.5f,1.5f,1.5f,1f),7);
        this.character.MakeInvincible();
    }

    public override void UpdateState() {
        base.UpdateState();

        this.character.SetVelocity(this.character.velocityAirdashBack);

        if (this.stateTime > 6) {
            this.character.ClearInvincibility();
            this.SwitchState(this.character.states.Airborne());
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