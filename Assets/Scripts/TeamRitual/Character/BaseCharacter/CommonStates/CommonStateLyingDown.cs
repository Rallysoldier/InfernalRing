using UnityEngine;

namespace TeamRitual.Character {
public class CommonStateLyingDown : CharacterState
{
    public CommonStateLyingDown(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = false;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.CROUCH;
        this.moveType = MoveType.LYING;
	    this.stateType = StateType.HURT;

        this.animationName = this.character.characterName + "_LyingDown";
    }

    public override void EnterState() {
        base.EnterState();
        this.character.VelY(0);
        this.character.body.position = new Vector2(this.character.PosX(),0);
    }

    public override void UpdateState() {
        base.UpdateState();

        int downTime = this.character.lastContact.DownTime > 0 ? this.character.lastContact.DownTime : 20;
        if (stateTime > downTime && this.character.health > 0)
            this.SwitchState(this.character.states.JumpLand());
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