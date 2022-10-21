namespace TeamRitual.Character {
public class CommonStateHurtSlide : CharacterState
{
    public CommonStateHurtSlide(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = false;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.CUSTOM;
        this.moveType = MoveType.LYING;
	    this.stateType = StateType.HURT;

        this.animationName = this.character.characterName + "_HurtBounce";
    }

    public override void EnterState() {
        base.EnterState();
        this.character.SetVelocity(this.character.lastContact.Slide,0);
    }

    public override void UpdateState() {
        base.UpdateState();

        if (this.character.lastContact.SlideTime > 0) {
            this.character.lastContact.SlideTime--;
        } else {
            this.character.SetVelocity(0,0);
            this.SwitchState(this.character.states.LyingDown());
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