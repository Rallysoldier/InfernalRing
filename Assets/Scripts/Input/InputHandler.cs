using System;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler {
    Dictionary<string,KeyCode> inputMapping = new Dictionary<string,KeyCode>();
    
    private CharacterStateMachine character;

    public string command = "";
    private int currentBufferTime;
    private const int MAX_BUFF_TIME = 30;

    public InputHandler(CharacterStateMachine character) {
    }

    public void mapP1Inputs() {
        inputMapping["U"] = KeyCode.W;
        inputMapping["D"] = KeyCode.S;
        inputMapping["B"] = KeyCode.A;
        inputMapping["F"] = KeyCode.D;

        inputMapping["L"] = KeyCode.U;
        inputMapping["M"] = KeyCode.I;
        inputMapping["H"] = KeyCode.O;

        inputMapping["S"] = KeyCode.J;
        inputMapping["E"] = KeyCode.K;
    }

    public void mapP2Inputs() {
        inputMapping["U"] = KeyCode.UpArrow;
        inputMapping["D"] = KeyCode.DownArrow;
        inputMapping["B"] = KeyCode.LeftArrow;
        inputMapping["F"] = KeyCode.RightArrow;

        inputMapping["L"] = KeyCode.Keypad4;
        inputMapping["M"] = KeyCode.Keypad5;
        inputMapping["H"] = KeyCode.Keypad6;

        inputMapping["S"] = KeyCode.Keypad1;
        inputMapping["E"] = KeyCode.Keypad2;
    }

    public void receiveInput(KeyCode keyCode) {
        string translatedInput = validateCode(keyCode);

        if (currentBufferTime >= 0 && translatedInput != "") {
            if (command.Length > 0) {
                command += ",";
            }
            command += translatedInput;
            currentBufferTime = MAX_BUFF_TIME;
        }
    }

    public string getCharacterInput() {
        //Invert left/right inputs if facing the negative x direction
        string inputStr = String.Copy(command);
        
        if (this.character != null && this.character.facing == -1) {
            inputStr.Replace("F","$");//Set forward to some unused meaningless char to swap B and F inputs correctly
            inputStr.Replace("B","F");
            inputStr.Replace("$","B");
        }

        return inputStr;
    }

    public void updateBufferTime() {
        if (currentBufferTime > 0) {
            currentBufferTime--;
        } else {
            command = "";
        }
    }

    private string validateCode(KeyCode keyCode) {
        foreach (KeyValuePair<string,KeyCode> kvp in inputMapping) {
            if (keyCode == kvp.Value) {
                return kvp.Key;
            }
        }

        return "";
    }
}