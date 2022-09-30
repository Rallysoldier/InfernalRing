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

    //setting default keybinds

    string _jump = "UpArrow";
    string _right = "RightArrow";
    string _left = "LeftArrow";
    string _down = "DownArrow";
    string _light_attack = "J";
    string _medium_attack = "K";
    string _heavy_attack = "L";


    //reference variable declaration
    Rigidbody2D body;
    Animator anim;
    bool isJumpPressed = false;
    bool isJumping;
    bool isStanding;
    bool isRunPressed;


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
    public bool IsGrounded {get {return isStanding; } set {isStanding = value;}}
    public bool IsRunPressed {get {return isRunPressed;} set {isRunPressed = value;}}

    //getters and setters for movement speed
    public float JumpSpeed {get {return jumpSpeed;}}
    public float HorizontalSpeed {get {return horizontalSpeed;}}


    //Getters and setters for keyboard input
    public string Jump {get {return _jump;}}
    public string Right {get {return _right;}}
    public string Left {get {return _left;}}
    public string Down {get {return _down;}}
    public string LightAttack {get {return _light_attack;}}
    public string MediumAttack{get {return _medium_attack;}}
    public string HeavyAttack {get {return _heavy_attack;}}
    
    void Awake()
    {
        //Grab references for RigidBody and Animator component on startup
        Body = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();

        //set initial values
        Body.velocity = new Vector2(0, 0);

        //setup state
        states = new CharacterStateFactory(this);
        currentState = states.Standing();
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision with ground");
        if(collision.gameObject.tag == "Ground")
        {
            IsGrounded = true;
        }
    }
}