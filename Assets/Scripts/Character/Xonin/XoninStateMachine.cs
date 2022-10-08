using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        /*
        
        
        old demo code


        if (this.currentState.stateType == StateType.ATTACK && this.currentState.stateTime == 6 && this.enemy.health > 0
            && Vector2.Distance(this.enemy.body.position,this.body.position) < 2) {
            this.enemy.health -= 100;
            if (this.enemy.health < 0) {
                this.enemy.health = 0;
            }

            if (this.enemy.health == 0) {
                this.enemy.SetVelocity(-5,12);
                this.enemy.currentState.SwitchState(this.enemy.states.HurtAir());
            } else {
                if (this.enemy.currentState.moveType == MoveType.STAND) {
                    this.enemy.currentState.SwitchState(this.enemy.states.HurtStand());
                    this.enemy.SetVelX(-3);
                } else if (this.enemy.currentState.moveType == MoveType.CROUCH) {
                    this.enemy.currentState.SwitchState(this.enemy.states.HurtCrouch());
                    this.enemy.SetVelX(-2);
                } else if (this.enemy.currentState.moveType == MoveType.AIR) {
                    this.enemy.currentState.SwitchState(this.enemy.states.HurtAir());
                    this.enemy.SetVelocity(-1,3);
                }
            }
        }*/

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
                }
            }
        }
        base.changeStateOnInput();
    }
}