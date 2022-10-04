public class XoninStateFactory : CharacterStateFactory
{
    public XoninStateFactory(XoninStateMachine currentContext) : base(currentContext) { }

    public override CharacterState RunBack()
    {
        return new XoninRunBack(context, this);
    }

    public CharacterState StandLightAttack()
    {
        return new XoninStandLightAttack(context, this);
    }

    public CharacterState CrouchLightAttack()
    {
        return new XoninCrouchLightAttack(context, this);
    }

    public CharacterState AirLightAttack()
    {
        return new XoninAirLightAttack(context, this);
    }

}