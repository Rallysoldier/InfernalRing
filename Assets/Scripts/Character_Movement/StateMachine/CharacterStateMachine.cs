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
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public float dashCoolDown;

    //setting default keybinds - didnt work as expected. cannot use string as key code

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
    float walkLeftTime;
    float walkRightTime;
    bool facingRight;
    bool isWalkRightPressed;
    bool isWalkLeftPressed;
    public float _dashCoolDown;

    //state variables
    CharacterBaseState currentState;
    CharacterStateFactory states;


    //getters and setters, pascal case for variable name to use in other files, camel case for get/set
    //to reference from other files, call the variable by the pascal case (capitalize first letter of each word) ie. IsJumpPressed
    public CharacterBaseState CurrentState { get { return currentState; } set { currentState = value; } }
    public CharacterStateFactory State { get { return states; } set { states = value; } }
    public Rigidbody2D Body { get { return body; } set { body = value; } }
    public Animator Anim { get { return anim; } set { anim = value; } }
    public SpriteRenderer SpriteRenderer {get {return spriteRenderer; } set {spriteRenderer = value;}}
    public float DashCoolDown {get {return dashCoolDown;}}
    public bool FacingRight {get {return facingRight; } set {facingRight = value;}}
    public bool IsJumpPressed {get {return isJumpPressed; } set {isJumpPressed = value; } }
    public bool IsJumping {get {return isJumping; } set { isJumping = value;} }
    public bool IsGrounded {get {return isStanding; } set {isStanding = value;}}
    public bool IsWalkRightPressed {get {return isWalkRightPressed;} set {isWalkRightPressed = value;}}
    public bool IsWalkLeftPressed {get {return isWalkLeftPressed; } set{ isWalkRightPressed = value; }}
    public float JumpSpeed {get {return jumpSpeed;}}
    public float HorizontalSpeed {get {return horizontalSpeed;}}
    public float WalkLeftTime {get {return walkLeftTime;} set {walkLeftTime = value;}}
    public float WalkRightTime {get {return walkRightTime;} set {walkRightTime = value;}}
    public float LastDashTime {get {return _dashCoolDown;} set {_dashCoolDown = value;}}

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
        SpriteRenderer = GetComponent<SpriteRenderer>();

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

    