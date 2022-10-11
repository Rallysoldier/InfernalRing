namespace TeamRitual.Character {
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

    public CharacterState StandMediumAttack()
    {
        return new XoninStandMediumAttack(context, this);
    }

    public CharacterState CrouchMediumAttack()
    {
        return new XoninCrouchMediumAttack(context, this);
    }

    public CharacterState AirMediumAttack()
    {
        return new XoninAirMediumAttack(context, this);
    }

    public CharacterState StandHeavyAttack()
    {
        return new XoninStandHeavyAttack(context, this);
    }

    public CharacterState AirHeavyAttack()
    {
        return new XoninAirHeavyAttack(context, this);
    }
}
}