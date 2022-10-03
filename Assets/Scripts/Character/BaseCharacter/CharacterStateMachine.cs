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
    public int side;
    public int facing;

    public Vector2 velocityWalkForward = new Vector2(3,0);
    public Vector2 velocityWalkBack = new Vector2(-3,0);
    public Vector2 velocityRunForward = new Vector2(6,0);
    public Vector2 velocityRunBack = new Vector2(-6,0);
    public Vector2 velocityJumpNeutral = new Vector2(0,6);
    public Vector2 velocityJumpForward = new Vector2(5,6);
    public Vector2 velocityJumpBack = new Vector2(-5,6);
    public float standingFriction = 0.05f;
    public float crouchingFriction = 0.15f;
    public float gravity = 0.8f;

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
        //Gets new character input based on the direction they're facing.
        //Inverts F and B inputs if the character is facing the -x direction (facing == -1)
        this.inputStr = this.inputHandler.getCharacterInput(this);
        this.inputHandler.updateBufferTime();

        switch (this.currentState.physicsType)
        {
            default:
                break;
            case PhysicsType.STAND:
                body.gravityScale = 0.0f;
                this.SetVelocity(VelX()*this.standingFriction,VelY());
                break;
            case PhysicsType.CROUCH:
                body.gravityScale = 0.0f;
                this.SetVelocity(VelX()*this.crouchingFriction,VelY());
                break;
            case PhysicsType.AIR:
                body.gravityScale = this.gravity;
                break;
            case PhysicsType.CUSTOM:
                break;
        }

        if (this.currentState.inputChangeState) {
            if (this.currentState.moveType == MoveType.STAND) {
                if (
                    (!inputHandler.heldKeys.Contains(inputHandler.ForwardInput(this)) && this.currentState is CommonStateWalkForward) ||
                    (!inputHandler.heldKeys.Contains(inputHandler.BackInput(this)) && this.currentState is CommonStateWalkBackward)
                ) {
                    this.currentState.SwitchState(states.Stand());
                }
                
                if (inputStr.EndsWith("F,F")) {
                    this.currentState.SwitchState(states.RunForward());
                } else if (inputStr.EndsWith("D") || inputStr.EndsWith("D,F") || inputStr.EndsWith("F,D") || inputStr.EndsWith("D,B")  || inputStr.EndsWith("B,D")
                     || inputHandler.heldKeys.Contains(inputHandler.inputMapping["D"])) {
                    this.currentState.SwitchState(states.CrouchTransition());
                } else if (inputStr.EndsWith("U") || inputStr.EndsWith("U,F") || inputStr.EndsWith("F,U") || inputStr.EndsWith("U,B")  || inputStr.EndsWith("B,U")
                     || inputHandler.heldKeys.Contains(inputHandler.inputMapping["U"])) {
                    this.currentState.SwitchState(states.JumpStart());
                } else if (inputStr.EndsWith("F") || inputHandler.heldKeys.Contains(inputHandler.ForwardInput(this))) {
                    this.currentState.SwitchState(states.WalkForward());
                } else if (inputStr.EndsWith("B") || inputHandler.heldKeys.Contains(inputHandler.BackInput(this))) {
                    this.currentState.SwitchState(states.WalkBackward());
                }
            } else if (this.currentState.moveType == MoveType.CROUCH) {
                if (!inputHandler.heldKeys.Contains(inputHandler.inputMapping["D"])
                 && (this.currentState is CommonStateCrouch || this.currentState is CommonStateCrouchTransition)) {
                    this.currentState.SwitchState(states.Stand());
                }
            } else if (this.currentState.moveType == MoveType.AIR) {
                
            }
        }

        this.currentState.UpdateState();
    }

    public float PosX() {
        return this.body.position.x;
    }

    public float PosY() {
        return this.body.position.y;
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

    public void SetVelocity(Vector2 velocity) {
        body.velocity = new Vector2(velocity.x*facing,velocity.y);
    }

    public void correctFacing() {
        this.facing = this.PosX() < this.enemy.PosX() ? 1 : -1;
    }
}