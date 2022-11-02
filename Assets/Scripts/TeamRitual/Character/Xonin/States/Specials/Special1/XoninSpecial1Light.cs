namespace TeamRitual.Character{
public class XoninSpecial1Light : CharacterState {
    public XoninSpecial1Light(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        this.inputChangeState = false;
        this.faceEnemyStart = false;
        this.faceEnemyAlways = false;

        this.physicsType = PhysicsType.AIR;
        this.moveType = MoveType.AIR;
	    this.stateType = StateType.ATTACK;

        this.attackPriority = AttackPriority.SPECIAL;

        this.animationName = this.character.characterName + "_Special1Light";
    }

    public override void EnterState() {
        base.EnterState();

        this.character.SetVelocity(5,7);
    }

    public override void UpdateState() {
        base.UpdateState();

    }
}
}