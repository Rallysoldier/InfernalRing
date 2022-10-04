using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : ScriptableObject
{
    public string characterName;
    //Misc Variables
    public InputHandler inputHandler;
    public CharacterStateMachine enemy;

    //Animation Variables
    public Animator anim;
    public SpriteRenderer spriteRenderer;

    //Physics/Motion Variables
    public Rigidbody2D body;
    public int facing;

    public Vector2 velocityWalkForward = new Vector2(2,0);
    public Vector2 velocityWalkBack = new Vector2(-2,0);
    public Vector2 velocityRunForward = new Vector2(10,0);
    public Vector2 velocityRunBack = new Vector2(-10,0);
    public Vector2 velocityJumpNeutral = new Vector2(0,17);
    public Vector2 velocityJumpForward = new Vector2(6.5f,17f);
    public Vector2 velocityJumpBack = new Vector2(-6.5f,17f);
    public float standingFriction = 0.05f;
    public float crouchingFriction = 0.15f;
    public float gravity = 0.85f;

    //state variables
    public CharacterState currentState;
    public CharacterStateFactory states;
    public string inputStr = "";

    //getters and setters
    public CharacterState CurrentState { get { return currentState; } set { currentState = value; } }
    public CharacterStateFactory State { get { return states; } set { states = value; } }

    public CharacterStateMachine() {
        this.states = new CharacterStateFactory(this);
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

        body.gravityScale = 0.0f;
        switch (this.currentState.physicsType)
        {
            default:
                break;
            case PhysicsType.STAND:
                this.SetVelX(VelX()*this.standingFriction);
                break;
            case PhysicsType.CROUCH:
                this.SetVelX(VelX()*this.crouchingFriction);
                break;
            case PhysicsType.AIR:
                this.SetVelY(VelY()-this.gravity);
                break;
            case PhysicsType.CUSTOM:
                break;
        }

        if (this.currentState.inputChangeState) {
            if (this.currentState.moveType == MoveType.STAND) {
                if (inputStr.EndsWith("F,F")) {
                    this.currentState.SwitchState(states.RunForward());
                } else if (inputStr.EndsWith("D") || inputStr.EndsWith("D,F") || inputStr.EndsWith("F,D") || inputStr.EndsWith("D,B")  || inputStr.EndsWith("B,D")
                     || inputHandler.held("D")) {
                    this.currentState.SwitchState(states.CrouchTransition());
                } else if (inputStr.EndsWith("U") || inputStr.EndsWith("U,F") || inputStr.EndsWith("F,U") || inputStr.EndsWith("U,B")  || inputStr.EndsWith("B,U")
                     || inputHandler.held("U")) {
                    this.currentState.SwitchState(states.JumpStart());
                } else if (inputStr.EndsWith("F") || inputHandler.held(inputHandler.ForwardInput(this))) {
                    this.currentState.SwitchState(states.WalkForward());
                } else if (inputStr.EndsWith("B") || inputHandler.held(inputHandler.BackInput(this))) {
                    this.currentState.SwitchState(states.WalkBackward());
                }
            } else if (this.currentState.moveType == MoveType.CROUCH) {
                
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

    public void SetVelX(float velocity) {
        body.velocity = new Vector2(velocity,VelY());
    }

    public void SetVelY(float velocity) {
        body.velocity = new Vector2(VelX(),velocity);
    }

    public void correctFacing() {
        float oldFacing = this.facing;
        this.facing = this.PosX() < this.enemy.PosX() ? 1 : -1;

        this.spriteRenderer.flipX = this.facing == -1;
    }
}