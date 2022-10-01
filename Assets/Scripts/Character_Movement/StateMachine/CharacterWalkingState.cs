using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWalkingState : CharacterBaseState
{
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
    public override void ExitState()
    {

    }

    public override void InitializeSubState() { }

    public override void CheckSwitchState()
    {
        if(!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            ctx.Body.velocity = new Vector2(0, 0);
            ctx.Anim.SetBool("IsWalking", false);
            SwitchState(factory.Standing());
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            ctx.Anim.SetBool("IsWalking", false);
            HandleJumpHorizontal();
        }
        else
        {
            HandleWalk();
        }
    }

    public void HandleWalk()
    {
        //Make character face middle of the screen
        if(ctx.SpriteRenderer.transform.position.x > 0)
        {
            if(!ctx.FacingRight){Flip();}
        }
        else
        {
            if(ctx.FacingRight){Flip();}
        }

        //Handles dash
        if(CanDashLeft())
        {
            //ctx.Anim.SetBool("IsWalking", false);
            if(!ctx.FacingRight){SwitchState(factory.DashBack());}
            else{SwitchState(factory.DashForward());}
        }
        else if(CanDashRight())
        {
            //ctx.Anim.SetBool("IsWalking", false);
            if(ctx.FacingRight){SwitchState(factory.DashForward());}
            else{SwitchState(factory.DashBack());}
        }
        //Handles left/right walk
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            ctx.IsWalkLeftPressed = true;
            ctx.IsWalkRightPressed = false;      
            ctx.WalkLeftTime = Time.time;
            ctx.Body.velocity = new Vector2(ctx.HorizontalSpeed * -1, 0);
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            ctx.IsWalkRightPressed = true;
            ctx.IsWalkLeftPressed = false;
            ctx.WalkLeftTime = Time.time;
            ctx.Body.velocity = new Vector2(ctx.HorizontalSpeed, 0);
        }
    }

    public void HandleJumpHorizontal()
    {
        //facing right/left jump forward/back
        if(ctx.FacingRight)
        {
            if(Input.GetKey(KeyCode.RightArrow))
            {
                ctx.Anim.SetTrigger("Jump_Back");
                speed = ctx.HorizontalSpeed;
            }
            else
            {
                ctx.Anim.SetTrigger("Jump_Forward");
                speed = ctx.HorizontalSpeed * -1;
            }

        }
        else
        {
            if(Input.GetKey(KeyCode.RightArrow))
            {
                ctx.Anim.SetTrigger("Jump_Forward");
                speed = ctx.HorizontalSpeed;
            }
            else
            {
                ctx.Anim.SetTrigger("Jump_Back");
                speed = ctx.HorizontalSpeed * -1;
            }
        }
        ctx.Body.velocity = new Vector2(speed, ctx.JumpSpeed);
        SwitchState(factory.Airborne());
    }

    //Flips Character - currently based on middle of screen, will be based on other character
    public void Flip()
    {
        if(!ctx.FacingRight)
        {
            Debug.Log("Facing Right");
            ctx.SpriteRenderer.flipX = true;
            ctx.FacingRight = true;
        }
        else
        {
            Debug.Log("Facing Left");
            ctx.SpriteRenderer.flipX = false;
            ctx.FacingRight = false;
        }

    }

    public bool CanDashRight()
    {
        if(!Input.GetKeyDown(KeyCode.RightArrow)){return false;}
        else{ctx.WalkRightTime = Time.time;}
        if(Time.time - ctx.WalkRightTime > ctx.doubleTapTime){return false;}
        if(Time.time - ctx.LastDashTime < ctx.dashCoolDown){return false;}
        return true;
    }

    public bool CanDashLeft()
    {
        if(!Input.GetKeyDown(KeyCode.LeftArrow)){return false;}
        else{ctx.WalkLeftTime = Time.time;}
        if(Time.time - ctx.WalkLeftTime > ctx.doubleTapTime){return false;}
        if(Time.time - ctx.LastDashTime < ctx.dashCoolDown){return false;}
        return true;
    }
}