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

    public override void ChangeStateOnInput() {
        bool hittingEnemy = this.currentState.moveHit >= this.currentState.hitsToCancel && this.enemy.hitstun > 0;

        if (this.changedInput && (this.currentState.inputChangeState || hittingEnemy)) {
            if (this.currentState.moveType == MoveType.STAND || (this.currentState.moveType == MoveType.CROUCH && hittingEnemy && !this.inputHandler.held("D"))) {
                if (this.inputStr.EndsWith("L")) {
                    this.currentState.SwitchState((states as XoninStateFactory).StandLightAttack());
                } else if (this.inputStr.EndsWith("M")) {
                    this.currentState.SwitchState((states as XoninStateFactory).StandMediumAttack());
                } else if (this.inputStr.EndsWith("H")) {
                    this.currentState.SwitchState((states as XoninStateFactory).StandHeavyAttack());
                }
            }
            if (this.currentState.moveType == MoveType.CROUCH || (this.currentState.moveType == MoveType.STAND && hittingEnemy && this.inputHandler.held("D"))) {
                if (this.inputStr.EndsWith("L")) {
                    this.currentState.SwitchState((states as XoninStateFactory).CrouchLightAttack());
                } else if (this.inputStr.EndsWith("M")) {
                    this.currentState.SwitchState((states as XoninStateFactory).CrouchMediumAttack());
                } else if (this.inputStr.EndsWith("H")) {
                    this.currentState.SwitchState((states as XoninStateFactory).CrouchHeavyAttack());
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
        base.ChangeStateOnInput();
    }
}
}