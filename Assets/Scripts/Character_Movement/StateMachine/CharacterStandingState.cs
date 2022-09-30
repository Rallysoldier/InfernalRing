using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStandingState : CharacterBaseState
{
    public CharacterStandingState(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {

    }


    public override void EnterState()
    {
        Debug.Log("in Standing state");
        ctx.Anim.SetBool("IsGrounded", true);
    }

    public override void UpdateState()
    {
        Debug.Log("Updating standing state");
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            ctx.IsJumpPressed = true;
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ctx.IsRunPressed = true;
        }
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
            ctx.IsJumpPressed = false;
            Debug.Log("switching from standing to Jumping");
            SwitchState(factory.JumpStart());
        }
        if(ctx.IsRunPressed)
        {
            Debug.Log("switching from standing to walking");
            SwitchState(factory.Walking());
        }
    }
}