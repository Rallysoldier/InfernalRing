namespace TeamRitual.Character {
public class CommonStateHurtAir : CharacterState
{
    public CommonStateHurtAir(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = true;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.AIR;
        this.moveType = MoveType.AIR;
	    this.stateType = StateType.HURT;

        this.animationName = this.character.characterName + "_HurtAir";
    }

    public override void EnterState() {
        base.EnterState();
    }

    public override void UpdateState() {
        base.UpdateState();

        if (this.character.health == 0) {
            if (this.stateTime < 3)
                this.character.SetVelocity(-4,7);
        } else {
            if (this.character.hitstun == 0)
                this.SwitchState(this.character.states.Airborne());
        }

        if (this.character.body.position.y <= 0 && this.character.VelY() < 0) {
            this.character.VelY(0);
            this.SwitchState(this.character.states.HurtBounce());
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