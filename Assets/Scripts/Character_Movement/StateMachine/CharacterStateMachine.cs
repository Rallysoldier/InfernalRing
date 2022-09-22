public class CharacterStateFactory
{
    CharacterStateMachine context;

    public CharacterStateFactory(CharacterStateMachine currentContext)
    {
        context = currentContext;
    }
X
    public CharacterBaseState Idle()
    {
        return new CharacterIdleState(context, this);
    }
    public CharacterBaseState Running()
    {
        return new CharacterRunningState(context, this);
    }
    public CharacterBaseState InAir()
    {
        return new CharacterInAirState(context, this);
    }
    public CharacterBaseState Attacking()
    {
        return new CharacterAttackingState(context, this);
    }
    public CharacterBaseState Crouching()
    {
        return new CharacterCrouchingState(context, this);
    }
    public CharacterBaseState Grounded()
    {
        return new CharacterGroundedState(context, this);
    }
}