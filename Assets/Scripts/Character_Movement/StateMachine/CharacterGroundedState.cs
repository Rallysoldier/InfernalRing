using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroundedState : CharacterBaseState
{
    public CharacterGroundedState(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {

    }


    public override void EnterState()
    {
        ctx.Body.velocity = new Vector2(ctx.Body.velocity.x, ctx.jumpSpeed);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void ExitState() { }

    public override void InitializeSubState()
    {

    }

    public override void CheckSwitchState()
    {
        if(ctx.IsJumpPressed)
        {
            Debug.Log("Jumping");
            SwitchState(factory.Jump());
        }

    }
}