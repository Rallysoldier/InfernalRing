using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XoninJumpState : XoninBaseState
{
    public XoninJumpState(XoninStateMachine currentContext, XoninStateFactory XoninStateFactory) 
    : base(currentContext, XoninStateFactory)
    {

    }

    public override void EnterState()
    {
        HandleJump();
    }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchState()
    {
    
    }

    void HandleJump()
    {
    }
}