using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XoninStateMachine : CharacterStateMachine
{
    public XoninStateMachine() : base() {
        this.states = new XoninStateFactory(this);
        this.characterName = "Xonin";
    }

    public override void UpdateState()
    {
        if (this.currentState.inputChangeState) {
            if (this.currentState.moveType == MoveType.STAND) {
                if (this.inputStr.EndsWith("L")) {

                }
            } else if (this.currentState.moveType == MoveType.CROUCH) {
                
            } else if (this.currentState.moveType == MoveType.AIR) {
                
            }
        }

        base.UpdateState();
    }
}