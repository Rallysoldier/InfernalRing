using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJumpStartState : CharacterBaseState
{
    public CharacterJumpStartState(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {

    }

    public override void EnterState()
    {
        Debug.Log("entering JumpStart state");
        HandleJumpStart();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        Debug.Log("exiting JumpStart state");
        ctx.IsJumping = false;
        ctx.IsJumpPressed = false;
        ctx.IsGrounded = false;
    }

    public override void InitializeSubState() { }

    public override void CheckSwitchState()
    {
        ExitState();
        if(ctx.IsGrounded)
        {
            SwitchState(factory.Standing());
        }
        else
        {
            SwitchState(factory.Airborne());
        }
    }

    void HandleJumpStart()
    {
        Debug.Log("in handle JumpStart");
        ctx.Body.velocity = new Vector2(ctx.Body.velocity.x, ctx.JumpSpeed);
        ctx.Anim.SetTrigger("Jump");
        ctx.Anim.SetBool("IsGrounded", false);
    }
}