using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameObj : MonoBehaviour
{
    public CharacterStateMachine stateMachine;
    public InputHandler inputHandler = new InputHandler();
    public int wins = 0;

    void Awake() {
        this.transform.localScale = new Vector2(3.0f,3.0f);
    }

    void Update() {
        this.inputHandler.releasedKeys.Clear();
        this.inputHandler.heldKeys.Clear();

        foreach (KeyValuePair<string,KeyCode> kvp in this.inputHandler.inputMapping) {
            if (Input.GetKeyDown(kvp.Value)) {
                this.inputHandler.receiveInput(kvp.Key);
            }
            if (Input.GetKeyUp(kvp.Value)) {
                this.inputHandler.addReleasedKey(kvp.Key);
            }
            if (Input.GetKey(kvp.Value)) {
                this.inputHandler.addHeldKey(kvp.Key);
            }
         }
    }
}