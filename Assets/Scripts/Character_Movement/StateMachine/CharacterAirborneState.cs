using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAirborneState : CharacterBaseState
{
    public CharacterAirborneState(CharacterStateMachine currentContext, CharacterStateFactory CharacterStateFactory)
    : base(currentContext, CharacterStateFactory)
    {

    }

    public override void EnterState()
    {
        Debug.Log("In Airborne state");
        ctx.Anim.SetBool("Airborne", true);
        ctx.IsGrounded = false;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        ctx.Anim.SetBool("Airborne", false);
    }

    public override void InitializeSubState() { }

    public override void CheckSwitchState()
    {
        if(ctx.IsGrounded == true)
        {
            ExitState();
            SwitchState(factory.Standing());
        }
    }

}