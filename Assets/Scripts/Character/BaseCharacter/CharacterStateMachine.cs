using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : ScriptableObject
{
    //Misc Variables
    public InputHandler inputHandler;
    public CharacterStateMachine enemy;

    //Animation Variables
    public Animator anim;

    //Physics/Motion Variables
    public Rigidbody2D body;
    public int facing;

    public float standingFriction = 2.0f;
    public float crouchingFriction = 1.0f;
    public float gravity = 1.0f; //In caps, since technically "final" for each character.

    //state variables
    public CharacterState currentState;
    public CharacterStateFactory states;
    public string inputStr = "";

    //getters and setters
    public CharacterState CurrentState { get { return currentState; } set { currentState = value; } }
    public CharacterStateFactory State { get { return states; } set { states = value; } }

    public CharacterStateMachine() {
        this.states = new CharacterStateFactory(this);
        this.inputHandler = new InputHandler(this);
    }

    void Awake() {
        this.currentState = states.Stand();
    }

    void Update() {
    }

    public virtual void UpdateState() {
        if (enemy == null) {
            this.enemy = GameController.Instance.setEnemyStateMachine(this);
        }

        foreach (KeyCode kc in System.Enum.GetValues(typeof(KeyCode))) {
            if (Input.GetKeyDown(kc))
                this.inputHandler.receiveInput(kc);
        }
        //Gets new character input based on the direction they're facing.
        //Inverts F and B inputs if the character is facing the -x direction (facing == -1)
        this.inputStr = this.inputHandler.getCharacterInput();
        this.inputHandler.updateBufferTime();

        switch (this.currentState.physicsType)
        {
            default:
                break;
            case PhysicsType.STAND:
                body.gravityScale = 0.0f;
                this.SetVelocity(VelX()*this.standingFriction,0.0f);
                break;
            case PhysicsType.CROUCH:
                body.gravityScale = 0.0f;
                this.SetVelocity(VelX()*this.crouchingFriction,0.0f);
                break;
            case PhysicsType.AIR:
                body.gravityScale = this.gravity;
                break;
            case PhysicsType.CUSTOM:
                break;
        }

        if (this.currentState.inputChangeState) {
            if (this.currentState.moveType == MoveType.STAND) {
                if (inputStr.EndsWith("F,F")) {
                    this.currentState.SwitchState(states.RunForward());
                } else if (inputStr.EndsWith("F")) {
                    this.currentState.SwitchState(states.WalkForward());
                } else if (inputStr.EndsWith("B")) {
                    this.currentState.SwitchState(states.WalkBackward());
                } else if (inputStr.EndsWith("D")) {
                    this.currentState.SwitchState(states.CrouchTransition());
                } else if (inputStr.EndsWith("U") || inputStr.EndsWith("U,F") || inputStr.EndsWith("U,B")) {
                    if (inputStr.Length > 0) {
                        Debug.Log(inputStr);
                    }
                    this.currentState.SwitchState(states.JumpStart());
                }
            } else if (this.currentState.moveType == MoveType.CROUCH) {
                
            } else if (this.currentState.moveType == MoveType.AIR) {
                
            }
        }

        this.currentState.UpdateState();
    }

    public float PosX() {
        return this.body.velocity.x;
    }

    public float PosY() {
        return this.body.velocity.y;
    }

    public float VelX() {
        return this.body.velocity.x;
    }

    public float VelY() {
        return this.body.velocity.y;
    }

    //Always use this to set velocity. Changes velocity based on the "facing" variable.
    public void SetVelocity(float velx, float vely) {
        body.velocity = new Vector2(velx*facing,vely);
    }

    public void correctFacing() {
        if (this.enemy != null) {
            if (this.PosX() < this.enemy.PosX()) {
                this.facing = 1;
            } else if (this.PosX() > this.enemy.PosX()) {
                this.facing = -1;
            }
        }
    }
}