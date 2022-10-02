using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateWalkForward : CharacterState
{
    public CommonStateWalkForward(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        
    }

    public override void EnterState() {
        base.EnterState();
        this.character.anim.SetTrigger("WalkForward");
    }

    public override void UpdateState() {
        base.UpdateState();
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