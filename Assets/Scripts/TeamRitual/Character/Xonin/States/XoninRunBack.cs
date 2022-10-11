namespace TeamRitual.Character {
public class XoninRunBack : CharacterState
{
    public XoninRunBack(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = true;
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
            this.character.SetVelY(this.character.VelY()-0.5f);
        }

        if (this.character.body.position.y <= 0.2 && this.character.VelY() < 0) {
            this.character.SetVelY(0);
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