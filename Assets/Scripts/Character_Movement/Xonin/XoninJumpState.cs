using UnityEngine;

public class XoninJumpState : XoninJumpState
{
    public XoninJumpState(XoninJumpState currentContext, XoninStateFactory xoninStateFactory) : base(currentContext, XoninStateFactory)
    {

    }

    public override void EnterState()
    {
        HandleJump();
    }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchState() { }

    void HandleJump()
    {
    }
}