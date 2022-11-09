using TeamRitual.Core;
using TeamRitual.Input;
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
        string inputStr = this.GetInput();
        bool hittingEnemy = this.currentState.moveHit >= this.currentState.hitsToCancel
            && this.enemy.hitstun > 0 && this.enemy.currentState.stateTime > 3;

        if (this.currentState.inputChangeState || hittingEnemy) {
            if (hittingEnemy && InputHandler.IsAttackInput(inputStr)) {
                this.inputHandler.ClearInput();
            }

            bool standingAttack = this.currentState.moveType == MoveType.STAND || (this.currentState.moveType == MoveType.CROUCH && hittingEnemy && this.inputHandler.released("D"));
            bool crouchingAttack = this.currentState.moveType == MoveType.CROUCH || (this.currentState.moveType == MoveType.STAND && hittingEnemy && this.inputHandler.held("D"));
            bool airborneAttack = this.currentState.moveType == MoveType.AIR;

            //Air OK moves
            if (standingAttack || crouchingAttack || airborneAttack) {
                if (inputStr.EndsWith("D,F,D,F,L") || inputStr.EndsWith("D,F,D,F,M") || inputStr.EndsWith("D,F,D,F,H")) {
                    this.currentState.SwitchState((states as XoninStateFactory).Ultimate1Start());
                    return;
                }

                if (inputStr.EndsWith("D,F,L")) {
                    this.currentState.SwitchState((states as XoninStateFactory).Special1Light());
                    return;
                }
                if (inputStr.EndsWith("D,F,M")) {
                    this.currentState.SwitchState((states as XoninStateFactory).Special1Medium());
                    return;
                }
                if (inputStr.EndsWith("D,F,H")) {
                    if (this.GetEnergy() >= 500f) {
                        this.currentState.SwitchState((states as XoninStateFactory).Special1Heavy());
                    } else {
                        this.currentState.SwitchState((states as XoninStateFactory).Special1Medium());
                    }
                    return;
                }

                if (inputStr.EndsWith("D,B,L")) {
                    this.currentState.SwitchState((states as XoninStateFactory).Special2LightRise());
                    return;
                }
                if (inputStr.EndsWith("D,B,M")) {
                    this.currentState.SwitchState((states as XoninStateFactory).Special2MediumRise());
                    return;
                }
                if (inputStr.EndsWith("D,B,H")) {
                    if (this.GetEnergy() >= 500f) {
                        this.currentState.SwitchState((states as XoninStateFactory).Special2HeavyRise());
                    } else {
                        this.currentState.SwitchState((states as XoninStateFactory).Special2MediumRise());
                    }
                    return;
                }
            } 

            if (standingAttack) {
                if (inputStr.EndsWith("L")) {
                    this.currentState.SwitchState((states as XoninStateFactory).StandLightAttack());
                    return;
                } else if (inputStr.EndsWith("M")) {
                    this.currentState.SwitchState((states as XoninStateFactory).StandMediumAttack());
                    return;
                } else if (inputStr.EndsWith("H")) {
                    this.currentState.SwitchState((states as XoninStateFactory).StandHeavyAttack());
                    return;
                }
            }
            if (crouchingAttack) {
                if (inputStr.EndsWith("L")) {
                    this.currentState.SwitchState((states as XoninStateFactory).CrouchLightAttack());
                    return;
                } else if (inputStr.EndsWith("M")) {
                    this.currentState.SwitchState((states as XoninStateFactory).CrouchMediumAttack());
                    return;
                } else if (inputStr.EndsWith("H")) {
                    this.currentState.SwitchState((states as XoninStateFactory).CrouchHeavyAttack());
                    return;
                }
            }
            if (airborneAttack) {
                if (inputStr.EndsWith("L")) {
                    this.currentState.SwitchState((states as XoninStateFactory).AirLightAttack());
                    return;
                } else if (inputStr.EndsWith("M")) {
                    this.currentState.SwitchState((states as XoninStateFactory).AirMediumAttack());
                    return;
                } else if (inputStr.EndsWith("H")) {
                    this.currentState.SwitchState((states as XoninStateFactory).AirHeavyAttack());
                    return;
                }
            }
        }
        base.ChangeStateOnInput();
    }
}
}