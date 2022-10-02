public class CharacterStateFactory
{
    CharacterStateMachine context;

    public CharacterStateFactory(CharacterStateMachine currentContext)
    {
        context = currentContext;
    }

    public CharacterState Stand()
    {
        return new CommonStateStand(context, this);
    }
    public CharacterState WalkForward()
    {
        return new CommonStateWalkForward(context, this);
    }
    public CharacterState WalkBackward()
    {
        return new CommonStateWalkBackward(context, this);
    }
    public CharacterState RunForward()
    {
        return new CommonStateRunForward(context, this);
    }
    public CharacterState Airborne()
    {
        return new CommonStateAirborne(context, this);
    }
    public CharacterState CrouchTransition()
    {
        return new CommonStateCrouchTransition(context, this);
    }
    public CharacterState Crouch()
    {
        return new CommonStateCrouch(context, this);
    }
    public CharacterState JumpStart()
    {
        return new CommonStateJumpStart(context, this);
    }
    public CharacterState JumpLand()
    {
        return new CommonStateJumpLand(context, this);
    }
}