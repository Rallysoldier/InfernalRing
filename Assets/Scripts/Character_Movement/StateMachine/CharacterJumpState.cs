using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJumpState : CharacterBaseState
{
    public CharacterJumpState(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {

    }

    public override void EnterState()
    {
        Jump();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        ctx.IsJumping = false;
        ctx.IsJumpPressed = false;
    }

    public override void InitializeSubState() { }

    public override void CheckSwitchState()
    {
        if(ctx.IsGrounded)
        {
            SwitchState(factory.Grounded());
        }
    }

    void Jump()
    {
        ctx.Body.velocity = new Vector2(ctx.Body.velocity.x, ctx.JumpSpeed);
        ctx.Anim.SetTrigger("Jump");
    }
}