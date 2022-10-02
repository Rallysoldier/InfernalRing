using UnityEngine;

public abstract class CharacterState
{
	protected CharacterStateMachine character;
	protected CharacterStateFactory factory;

	public int stateTime = 0;

	//The variables below can be different for each state, and are only ever defined/mutated in the state's constructor.

	public bool inputChangeState = false;	//State will allow inputs to change state in the character's UpdateState() function.
	public bool faceEnemyStart = false;	//State will adjust the facing variable in a character only at the start of the state.
	public bool faceEnemyAlways = false;		//State will always adjust the facing variable in a character to face the correct direction.

	public PhysicsType physicsType = PhysicsType.CUSTOM;
	public MoveType moveType = MoveType.STAND;
	public StateType stateType = StateType.IDLE;

	public CharacterState(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
	{
		character = currentContext;
		factory = CharacterStateFactory;
	}

	public virtual void EnterState() {
		if (this.faceEnemyStart || this.faceEnemyAlways) {
			character.correctFacing();
		}
	}

	public virtual void UpdateState() {
		this.stateTime++;
		if (this.faceEnemyAlways) {
			character.correctFacing();
		}
	}

	public virtual void ExitState() {}

	public virtual void CheckSwitchState() {}

	public virtual void InitializeSubState() {}

	public virtual void SwitchState(CharacterState newState)
	{
		// exit current state
		ExitState();

		//enter new state
		newState.EnterState();

		//update context of state
		character.currentState = newState;
	}

	protected void SetSuperState()
	{

	}

	protected void SetSubState()
	{

	}
}