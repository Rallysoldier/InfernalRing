public class PlayerStateFactory
{
    XoninStateMachine context;

    public XoninStateFactory(PlayerStateMachine currentContext)
    {
        context = currentContext;
    }

    public XoninBaseState Idle()
    {
        return new XoninIdleState(context, this);
    }
    public XoninBaseStateRunning()
    {
        return new XoninRunningState(context, this);
    }
    public XoninBaseState InAir()
    {
        return new XoninInAirState(context, this);
    }
    public XoninBaseState Attacking()
    {
        return new XoninAttackingState(context, this);
    }
    public XoninBaseState Crouching()
    {
        return new XoninCrouchingState(context, this);
    }
    public XoninBaseState Grounded()
    {
        return new XoninGroundedState(context, this);
    }
}