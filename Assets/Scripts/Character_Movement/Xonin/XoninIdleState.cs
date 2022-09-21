using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class XoninIdleState : XoninBaseState
{
    public XoninIdleState(XoninIdleState currentContext, XoninStateFactory XoninStateFactory) : base (currentContext, XoninStateFactory)
    {
 
    }

    public override void EnterState() { }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void InitializeSubState() { }

    public override void CheckSwitchState() { }
}