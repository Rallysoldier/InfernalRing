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
        bool attacking = this.currentState.stateType == StateType.ATTACK && this.enemy.hitstun > 0;
        if (this.currentState.inputChangeState && changedInput || attacking) {
            if (this.currentState.moveType == MoveType.STAND || (this.currentState.moveType == MoveType.CROUCH && attacking && !this.inputHandler.held("D"))) {
                if (this.inputStr.EndsWith("L")) {
                    this.currentState.SwitchState((states as XoninStateFactory).StandLightAttack());
                } else if (this.inputStr.EndsWith("M")) {
                    this.currentState.SwitchState((states as XoninStateFactory).StandMediumAttack());
                }
            }
            if (this.currentState.moveType == MoveType.CROUCH || (this.currentState.moveType == MoveType.STAND && attacking && this.inputHandler.held("D"))) {
                if (this.inputStr.EndsWith("L")) {
                    this.currentState.SwitchState((states as XoninStateFactory).CrouchLightAttack());
                } else if (this.inputStr.EndsWith("M")) {
                    this.currentState.SwitchState((states as XoninStateFactory).CrouchMediumAttack());
                }
            }
            if (this.currentState.moveType == MoveType.AIR) {
                if (this.inputStr.EndsWith("L")) {
                    this.currentState.SwitchState((states as XoninStateFactory).AirLightAttack());
                } else if (this.inputStr.EndsWith("M")) {
                    this.currentState.SwitchState((states as XoninStateFactory).AirMediumAttack());
                } else if (this.inputStr.EndsWith("H")) {
                    this.currentState.SwitchState((states as XoninStateFactory).AirHeavyAttack());
                }
            }
        }
        base.changeStateOnInput();
    }
}
}