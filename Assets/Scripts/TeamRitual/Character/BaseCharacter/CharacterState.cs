using UnityEngine;

namespace TeamRitual.Character {
public abstract class CharacterState
{
	protected CharacterStateMachine character;
	protected CharacterStateFactory factory;

	public int stateTime = -1;
	public string animationName;

	public int moveHit;
	public int moveContact;
	public int hitsToCancel = 1;

	//The variables below can be different for each state, and are only ever defined/mutated in the state's constructor.

	public bool inputChangeState = false;	//State will allow inputs to change state in the character's UpdateState() function.
	public bool faceEnemyStart = false;	//State will adjust the facing variable in a character only at the start of the state.
	public bool faceEnemyAlways = false;		//State will always adjust the facing variable in a character to face the correct direction.

	public AttackPriority attackPriority = AttackPriority.NONE;

	public PhysicsType physicsType = PhysicsType.CUSTOM;
	public MoveType moveType = MoveType.STAND;
	public StateType stateType = StateType.IDLE;

	public CharacterState(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
	{
		character = currentContext;
		factory = CharacterStateFactory;
	}

	public virtual void EnterState() {
		this.character.lastContact.HitFrame = -1;

		if (this.faceEnemyStart || this.faceEnemyAlways) {
			character.correctFacing();
		}
		string prevAnimName = this.character.GetCurrentAnimationName();
		if(!this.character.anim.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
            this.character.anim.Play(animationName);
        }
	}

	public virtual void UpdateState() {
		this.stateTime++;
		if (this.faceEnemyAlways) {
			character.correctFacing();
		}
		if(!this.character.anim.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
            this.character.anim.Play(animationName);
        }
	}

	public virtual void ExitState() {}

	public virtual void CheckSwitchState() {}

	public virtual void InitializeSubState() {}

	public virtual void SwitchState(CharacterState newState)
	{
		if (this.GetType() == newState.GetType())
			return;

		if (newState.stateType == StateType.ATTACK && this.stateType == StateType.ATTACK && moveHit >= hitsToCancel) {
			bool canCancelInto =
				((newState.attackPriority >= this.attackPriority && this.attackPriority <= AttackPriority.HEAVY)
				|| (newState.attackPriority > this.attackPriority && this.attackPriority > AttackPriority.HEAVY))
				&& !this.character.attackCancels.Contains(newState.GetType().Name);

			if (!canCancelInto) {
				bool exceptions = newState.moveType == MoveType.AIR
					|| (character.ReverseBeat() && this.attackPriority <= AttackPriority.HEAVY);
				if (!exceptions)
					return;
			}

			this.character.attackCancels.Add(newState.GetType().Name);
		}



		// exit current state
		ExitState();

		//enter new state
		newState.EnterState();

		//update context of state
		character.currentState = newState;

		//Debug.Log("Switched from " + this + " to " + newState);
	}

	protected void SetSuperState()
	{

	}

	protected void SetSubState()
	{

	}
}
}