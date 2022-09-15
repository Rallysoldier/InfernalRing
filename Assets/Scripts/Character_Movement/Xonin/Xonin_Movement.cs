using System.Security.Cryptography;
using UnityEngine;

public class Xonin_Movement : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float jumpSpeed;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;

    //Awake only runs when game first begins
    private void Awake()
    {
        //Grap references for Rigidbody and Animator component on startup
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    //Update runs on every frame
    private void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");

        body.velocity = new Vector2(horizontalInput * horizontalSpeed, body.velocity.y);

        if(horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if(horizontalInput < -0.01f)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        //if up arrow is pressed and the player is on the ground, call jump function
        if(Input.GetKey(KeyCode.UpArrow) && grounded)
        {
            Jump();
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

    //checks if player is touching ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }
}
