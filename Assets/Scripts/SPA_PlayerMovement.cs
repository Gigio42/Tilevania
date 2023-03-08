using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SPA_PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] int numberOfJumps = 2;
    [SerializeField] Vector2 deathKick = new Vector2 (0f, 1f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    SpriteRenderer mySprite;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    float gravityScaleAtStart;
    bool isAlive = true;
    
    int availableJumps = 0;
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();  
        mySprite = GetComponent<SpriteRenderer>();  
        myAnimator = GetComponent<Animator>();  
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    void Update()
    {
        if(!isAlive) { return; }

        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove (InputValue value) 
    {
        if(!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump (InputValue value)
    {
        if(!isAlive) { return; }
        if(value.isPressed)
        {
            var isGrounded = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

            if(isGrounded)
            {
                availableJumps = numberOfJumps;
            }
            
            if (availableJumps > 0)
            {
                myRigidbody.velocity += new Vector2(myRigidbody.velocity.x, jumpSpeed);
                availableJumps--;
            }
        }
    }

    void OnFire (InputValue value)
    {
        if(!isAlive) { return; }

        Instantiate(bullet, gun.position, transform.rotation);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon; 

        if (playerHasHorizontalSpeed) 
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }  
    }

    void ClimbLadder()
    {
        var isClimbing = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        

        if(!isClimbing) 
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }
        
        Vector2 ClimbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = ClimbVelocity;
        myRigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;

        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);     
    }

    void Die() 
    {

        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathKick;
            mySprite.color = new Color32 (255, 0, 0, 255);
            FindObjectOfType<SPA_GameSession>().ProcessPlayerDeath();
        }
    }
}
