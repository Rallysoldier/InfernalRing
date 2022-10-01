public class CharacterStateFactory
{
    CharacterStateMachine context;

    public CharacterStateFactory(CharacterStateMachine currentContext)
    {
        context = currentContext;
    }


    public CharacterBaseState Walking()
    {
        return new CharacterWalkingState(context, this);
    }
    public CharacterBaseState Airborne()
    {
        return new CharacterAirborneState(context, this);
    }
    public CharacterBaseState Crouching()
    {
        return new CharacterCrouchingState(context, this);
    }
    public CharacterBaseState Standing()
    {
        return new CharacterStandingState(context, this);
    }
    public CharacterBaseState JumpStart()
    {
        return new CharacterJumpStartState(context, this);
    }
    public CharacterBaseState Landing()
    {
        return new CharacterLandingState(context, this);
    }
        public CharacterBaseState GaurdStanding()
    {
        return new CharacterGaurdStandingState(context, this);
    }
    public CharacterBaseState GaurdCrouching()
    {
        return new CharacterGaurdCrouchingState(context, this);
    }
    public CharacterBaseState AirDodge()
    {
        return new CharacterAirDodgeState(context, this);
    }
    public CharacterBaseState DashForward()
    {
        return new CharacterDashForwardState(context, this);
    }
    public CharacterBaseState DashBack()
    {
        return new CharacterDashBackState(context, this);
    }
}