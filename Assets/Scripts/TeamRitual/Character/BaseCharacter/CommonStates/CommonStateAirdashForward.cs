namespace TeamRitual.Character {
public class CommonStateAirdashForward : CharacterState
{
    public CommonStateAirdashForward(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.CUSTOM;
        this.moveType = MoveType.AIR;
	    this.stateType = StateType.IDLE;

        this.animationName = this.character.characterName + "_RunForward";
    }

    public override void EnterState() {
        base.EnterState();

        this.character.airdashCount++;
        this.character.SetVelocity(this.character.velocityAirdashForward);
    }

    public override void UpdateState() {
        base.UpdateState();

        this.character.SetVelocity(this.character.velocityAirdashForward);

        if (this.stateTime > 6) {
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