using UnityEngine;

public class XoninAttackingState : XoninBaseState
{
    public XoninAttackingState(XoninAttackingState currentContext, XoninStateFactory xoninStateFactory) : base(currentContext, XoninStateFactory)
    {
        
    }

    public override void EnterState() { }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchState() { }
}