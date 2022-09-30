using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWalkingState : CharacterBaseState
{
    string direction = "right";
    float speed = 0;
    public CharacterWalkingState(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {

    }

    public override void EnterState()
    {
        ctx.Anim.SetBool("IsWalking", true);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchState()
    {
        if(!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            ctx.Anim.SetBool("IsWalking",false);
            ctx.IsRunPressed = false;
            ctx.Body.velocity = new Vector2(0, 0);
            SwitchState(factory.Standing());
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            HandleJumpHorizontal(direction);
        }
        else
        {
            HandleWalk();
        }
    }

    public void HandleWalk()
    {
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            ctx.Body.velocity = new Vector2(ctx.HorizontalSpeed * -1, 0);
            direction = "left";
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            direction = "right";
            ctx.Body.velocity = new Vector2(ctx.HorizontalSpeed, 0);
        }
    }

    public void HandleJumpHorizontal(string direction)
    {
        if(direction == "right"){speed = ctx.HorizontalSpeed;}
        else{speed = -1 * ctx.HorizontalSpeed;}
        ctx.Body.velocity = new Vector2(speed, ctx.JumpSpeed);
        ctx.Anim.SetTrigger("Jump_Horizontal");
        SwitchState(factory.Airborne());
    }
}