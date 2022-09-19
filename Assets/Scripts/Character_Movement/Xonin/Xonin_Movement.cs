using System.Security.Cryptography;
using UnityEngine;

public class Xonin_Movement : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float doubleTapTime;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;
    private float _lastForDashButtonTime;
    private float _lastBackDashButtonTime;
    private float dash_time;
    public bool Facing_Right;


    //Awake only runs when game first begins
    private void Awake()
    {
        //Grap references for Rigidbody and Animator component on startup
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Facing_Right = true;
    }

    //Update runs on every frame
    private void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        if (dash_time - Time.time < 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * horizontalSpeed, body.velocity.y);
        }

        //Dash Mechanic
        if(grounded == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && Facing_Right)//facing right click left
            {
                if (Is_Dash_Backward_Possible && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    Backward_Dash();
                }
                _lastBackDashButtonTime = Time.time;
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow) && !Facing_Right)//facing left click left
            {
                if (Is_Dash_Forward_Possible && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    Forward_Dash();
                }
                if (Is_Dash_Backward_Possible && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    Backward_Dash();
                }
                _lastForDashButtonTime = Time.time;

            }
            else if(Input.GetKeyDown(KeyCode.RightArrow) && Facing_Right)//facing right click right
            {
                if (Is_Dash_Forward_Possible && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    Forward_Dash();
                }
                if (Is_Dash_Backward_Possible && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    Backward_Dash();
                }
                _lastForDashButtonTime = Time.time;
            }
            else if(Input.GetKeyDown(KeyCode.RightArrow) && !Facing_Right) //facing left click right
            {
                if (Is_Dash_Backward_Possible && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    Backward_Dash();
                }
                _lastBackDashButtonTime = Time.time;
            }
        }

        //Basic attack mechanic
        if(Input.GetKeyDown(KeyCode.L) && !Input.GetKey(KeyCode.DownArrow))
        {
            Basic_Attack();
        }

        //Crouch and crouch attack mechanic
        if(Input.GetKey(KeyCode.DownArrow))
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                Crouch_Attack();
            }
            else
            {
            Crouch();
            }
        }
        else
        {
            Uncrouch();
        }

        //Changes direction character is facing
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
            Facing_Right = true;
        }
        else if(horizontalInput < -0.01f)
        {
            transform.localScale = new Vector2(-1, 1);
            Facing_Right = false;
        }



        //if up arrow is pressed and the player is on the ground, call jump function
        if(Input.GetKeyDown(KeyCode.UpArrow) && grounded)
        {
            if(horizontalInput > 0.1f) //moving right
            {
                Jump_Right();
            }
            else if(horizontalInput < -0.1f) //moving left
            {
                Jump_Left();
            }
            else
            {
                Jump();
            }
        }


        //Set animator parameters
        anim.SetBool("Run", horizontalInput != 0); //when moving left/right, start run animation
        anim.SetBool("grounded", grounded); //set grounded to true when player collides with ground
    }

    //Makes character jump and sets grounded to false until character lands
    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        anim.SetTrigger("Jump");
        grounded = false;
    }

    //animation for jumping to the right
    private void Jump_Right()
    {
        body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        anim.SetTrigger("Jump_Horizontal");
        grounded = false;
    }

    //animation for jumping to the left
    private void Jump_Left()
    {
        body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        anim.SetTrigger("Jump_Horizontal");
        grounded = false;
    }

    //animation for forward dash
    private void Forward_Dash()
    {
        body.velocity = new Vector2(dashSpeed, body.velocity.y);
        anim.SetTrigger("Dash_FWD");
        dash_time = Time.time;
    }

    //animation for backward dash
    private void Backward_Dash()
    {
        body.velocity = new Vector2(dashSpeed, jumpSpeed);
        anim.SetTrigger("Dash_BKWD");
        grounded = false;
        dash_time = Time.time;
    }

    //animation for basic attack
    private void Basic_Attack()
    {
        anim.SetTrigger("Basic_Attack");
    }

    //animation for crouching
    private void Crouch()
    {
        anim.SetBool("Crouched", true);
    }

    //animation for ending crouch
    private void Uncrouch()
    {
        anim.SetBool("Crouched", false);
    }

    //animation for crouch kick
    private void Crouch_Attack()
    {
        anim.SetTrigger("Crouch_Attack");
    }

    //checks if player is touching ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    //checks to see if dash forward button is double clicked
    bool Is_Dash_Forward_Possible
    {
        get
        {
            if(Time.time - _lastForDashButtonTime > doubleTapTime)
            {
                return false;
            }
            return true;
        }
    }

    //checks to see if dash backward button is double clicked
    bool Is_Dash_Backward_Possible
    {
        get
        {
            if (Time.time - _lastBackDashButtonTime > doubleTapTime)
            {
                return false;
            }
            return true;
        }
    }


}
