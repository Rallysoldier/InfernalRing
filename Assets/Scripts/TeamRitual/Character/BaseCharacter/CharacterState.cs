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
	public bool jumpCancel = false;
	public int scalingStep = 1;

	public bool EXFlash = false;

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

		string inputStr = this.character.GetInput();

		if (this.faceEnemyAlways) {
			character.correctFacing();
		}
		if(!this.character.anim.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
            this.character.anim.Play(animationName);
        }

		//Jump & Dash Cancels
		if (this.attackPriority > AttackPriority.NONE && this.attackPriority <= AttackPriority.HEAVY && this.moveHit >= this.hitsToCancel) {
			if (this.character.airjumpCount < this.character.maxAirjumps && this.character.airdashCount < this.character.maxAirdashes
				&& this.moveType == MoveType.AIR && 
				(inputStr.EndsWith("U") || inputStr.EndsWith("U,F") || inputStr.EndsWith("F,U") || inputStr.EndsWith("U,B")  || inputStr.EndsWith("B,U")|| this.character.inputHandler.held("U"))) {
				this.SwitchState(factory.AirjumpStart());
			}

			if (this.character.GetEnergy() >= 200 && inputStr.EndsWith("F,F")) {
				this.character.AddEnergy(-200);
				this.character.Flash(new Vector4(1.5f,1.5f,1.5f,1f),4);
				if (this.moveType == MoveType.AIR) {
					this.SwitchState(this.factory.AirdashForward());
				} else {
					this.SwitchState(this.factory.RunForward());
				}
			}
		}

		if (this.jumpCancel && this.character.enemy.VelY() > 0 && moveHit >= hitsToCancel) {
            if ((inputStr.EndsWith("U") || inputStr.EndsWith("U,F") || inputStr.EndsWith("F,U") || inputStr.EndsWith("U,B")  || inputStr.EndsWith("B,U"))
				|| this.character.inputHandler.held("U")) {
				CommonStateJumpStart jumpStart = this.character.states.JumpStart() as CommonStateJumpStart;
				Vector2 hitVelocity = this.character.enemy.lastContact.HitVelocity;					
				float yDistance = this.character.enemy.PosY() - this.character.PosY();

				Debug.Log(yDistance);

				jumpStart.jumpVelocity = new Vector2(-hitVelocity.x,  yDistance + hitVelocity.y);
				this.SwitchState(jumpStart);
			}
        }
	}

	public virtual void ExitState() {}

	public virtual void CheckSwitchState() {}

	public virtual void InitializeSubState() {}

	public virtual void SwitchState(CharacterState newState)
	{
		if (newState.stateType == StateType.ATTACK && this.stateType == StateType.ATTACK && moveHit >= hitsToCancel) {
			bool alreadyChained = this.character.attackCancels.Contains(newState.GetType().Name);
			bool canCancelInto =
				((newState.attackPriority >= this.attackPriority && this.attackPriority <= AttackPriority.HEAVY)
				|| (newState.attackPriority > this.attackPriority && this.attackPriority > AttackPriority.HEAVY))
				&& !alreadyChained;

			if (!canCancelInto && this.attackPriority <= AttackPriority.HEAVY) {
				bool exceptions = !alreadyChained && (newState.moveType == MoveType.AIR || character.ReverseBeat());
				if (!exceptions)
					return;
			}

			this.character.attackCancels.Add(newState.GetType().Name);
		}



		// exit current state
		ExitState();

		//enter new state
		newState.EnterState();

		//update context of state0
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