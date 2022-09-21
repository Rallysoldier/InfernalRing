using UnityEngine;

public class XoninAttackingState : XoninBaseState
{
    public XoninGroundedState(XoninGroundedState currentContext, XoninStateFactory xoninStateFactory) : base(currentContext, XoninStateFactory)
    {

    }


    public override void EnterState() { }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchState(ctx.IsJumpPressed)
    {
        if(ctx.IsJumpPressed)
        {
            SwitchState(factory.Jump());
        }
    }
}