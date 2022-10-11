using System;
using System.Collections.Generic;
using UnityEngine;
using TeamRitual.Character;

namespace TeamRitual.Input {
public class InputHandler {
    public Dictionary<string,KeyCode> inputMapping = new Dictionary<string,KeyCode>();
    public List<string> releasedKeys = new List<string>();
    public List<string> heldKeys = new List<string>();
    public string command = "";
    private int currentBufferTime;
    private const int MAX_BUFF_TIME = 30;

    public InputHandler() {
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

    public void receiveInput(string input) {
        if (currentBufferTime >= 0 && input != "") {
            if (command.Length > 0) {
                command += ",";
            }
            command += input;
            currentBufferTime = MAX_BUFF_TIME;
        }
    }

    public void addReleasedKey(string input) {
        releasedKeys.Add(input);
    }
    
    public void addHeldKey(string input) {
        heldKeys.Add(input);
    }

    public bool released(string input) {
        return releasedKeys.Contains(input);
    }

    public bool held(string input) {
        return heldKeys.Contains(input);
    }

    public string getCharacterInput(CharacterStateMachine character) {
        //Invert left/right inputs if facing the negative x direction
        string inputStr = String.Copy(command);

        if (character != null && character.facing == -1) {
            inputStr = inputStr.Replace("F","$");//Set forward to some unused meaningless char to swap B and F inputs correctly
            inputStr = inputStr.Replace("B","F");
            inputStr = inputStr.Replace("$","B");
        }

        return inputStr;
    }


    public string ForwardInput(CharacterStateMachine character) {
        return character != null && character.facing == 1 ? "F" : "B";
    }

    public string BackInput(CharacterStateMachine character) {
        return character != null && character.facing == 1 ? "B" : "F";
    }

    public void updateBufferTime() {
        if (currentBufferTime > 0) {
            currentBufferTime--;
        } else {
            command = "";
        }
    }
}
}