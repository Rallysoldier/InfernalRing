using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStateWalkBackward : CharacterState
{
    public CommonStateWalkBackward(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory) : base(currentContext, CharacterStateFactory)
    {
        
    }

    public override void EnterState() {
        base.EnterState();
        this.character.anim.SetTrigger("WalkBackward");
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