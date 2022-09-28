using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    //Fields adjustable in Unity
    [SerializeField] public float horizontalSpeed;
    [SerializeField] public float dashSpeed;
    [SerializeField] public float jumpSpeed;
    [SerializeField] public float doubleTapTime;

    //reference variable declaration
    Rigidbody2D body;
    Animator anim;
    bool isJumpPressed = false;
    bool isJumping;
    bool isGrounded;


    //state variables
    CharacterBaseState currentState;
    CharacterStateFactory states;


    //getters and setters, pascal case for variable name to use in other files, camel case for get/set
    //to reference from other files, call the variable by the pascal case (capitalize first letter of each word) ie. IsJumpPressed
    public CharacterBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    public CharacterStateFactory State { get { return states; } set { states = value; } }
    public Rigidbody2D Body { get { return body; } set { body = value; } }
    public Animator Anim { get { return anim; } set { anim = value; } }
    public bool IsJumpPressed {get {return isJumpPressed; } set {isJumpPressed = value; } }
    public bool IsJumping {get {return isJumping; } set { isJumping = value;} }
    public bool IsGrounded {get {return isGrounded; } set {isGrounded = value;}}
    public float JumpSpeed {get {return jumpSpeed;}}

    
    
    void Awake()
    {
        //Grab references for RigidBody and Animator component on startup
        Body = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();

        //setup state
        states = new CharacterStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();
    }

    //update function runs 60 times per second
    void Update()
    {
        currentState.UpdateState();
    }

    public void SwitchState(CharacterBaseState state)
    {

    }

}