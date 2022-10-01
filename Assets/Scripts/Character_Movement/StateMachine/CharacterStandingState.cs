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
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            ctx.IsJumpPressed = true;
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            ctx.WalkRightTime = Time.time;
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            ctx.WalkLeftTime = Time.time;
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
        else if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if(Time.time - ctx.WalkRightTime < ctx.doubleTapTime)
            {
                SwitchState(factory.DashForward());
            }
            else if(Time.time - ctx.WalkLeftTime < ctx.doubleTapTime)
            {
                SwitchState(factory.DashBack());
            }
            Debug.Log("switching from standing to walking");
            SwitchState(factory.Walking());
        }

    }
}