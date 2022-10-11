using TeamRitual.Core;
using UnityEngine;

namespace TeamRitual.Character {
public class XoninStateMachine : CharacterStateMachine
{
    public XoninStateMachine() : base() {
        this.states = new XoninStateFactory(this);
        this.characterName = "Xonin";

        velocityRunBack = new Vector2(-10,7);
    }

    public override ContactSummary UpdateStates()
    {
        ContactSummary summary =  base.UpdateStates();

        this.currentState.UpdateState();
        
        return summary;
    }

    public override void changeStateOnInput() {
        if (this.currentState.inputChangeState && changedInput) {
            if (this.currentState.moveType == MoveType.STAND) {
                if (this.inputStr.EndsWith("L")) {
                    this.currentState.SwitchState((states as XoninStateFactory).StandLightAttack());
                }
            } else if (this.currentState.moveType == MoveType.CROUCH) {
                if (this.inputStr.EndsWith("L")) {
                    this.currentState.SwitchState((states as XoninStateFactory).CrouchLightAttack());
                }
            } else if (this.currentState.moveType == MoveType.AIR) {
                if (this.inputStr.EndsWith("L")) {
                    this.currentState.SwitchState((states as XoninStateFactory).AirLightAttack());
                } else if (this.inputStr.EndsWith("H")) {
                    this.currentState.SwitchState((states as XoninStateFactory).AirHeavyAttack());
                }
            }
        }
        base.changeStateOnInput();
    }
}
}