namespace TeamRitual.Character {
public class CommonStateHurtBounce : CharacterState
{
    public CommonStateHurtBounce(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = false;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.AIR;
        this.moveType = MoveType.AIR;
	    this.stateType = StateType.HURT;

        this.animationName = this.character.characterName + "_HurtBounce";
    }

    public override void EnterState() {
        base.EnterState();
        this.character.SetVelocity(this.character.lastContact.Bounce);
        if (this.character.lastContact.BounceGravity > 0) {
            this.character.gravity = this.character.lastContact.BounceGravity;
        }
    }

    public override void UpdateState() {
        base.UpdateState();

        if (this.character.body.position.y <= 0.2 && this.character.VelY() < 0) {
            this.SwitchState(this.character.states.HurtSlide());
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