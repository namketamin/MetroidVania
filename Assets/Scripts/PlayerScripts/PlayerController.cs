using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("--Movement--")]
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private float moveSpeed=1f;
    [SerializeField] private float jumpForce = 1f;

    [SerializeField] public float knockbackForce = 1f;

    [SerializeField] private Animator dust;
    [SerializeField] private LayerMask enemyLayer;

    public bool isGrounded;
    public bool lastGrounded;
    public bool isPushing;
    public int jumpCount = 0;
    public bool isKnockback;

    Rigidbody2D rb;
    Animator anim;
    PlayerAudio playerAudio;
    GroundCheck groundCheck;
    private void Awake()
    {
        rb= GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerAudio=GetComponentInChildren<PlayerAudio>();
        groundCheck=GetComponentInChildren<GroundCheck>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        Debug.Log(isGrounded);
        HandleAnimation();
        Flip();
        Push();
    }
    void FixedUpdate()
    {
        Run();
        SetGrounded();
    }
    void HandleAnimation()
    {
        anim.SetBool("isRunning", Mathf.Abs(moveInput.x) > 0.01f&&isGrounded);

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetFloat("xVelocity", Mathf.Abs(moveInput.x));

        if (anim.GetBool("isDoubleJumping") && rb.linearVelocity.y < 0)
        {
            anim.SetBool("isDoubleJumping", false);
        }
    }
    void OnMove(InputValue value)
    {
        moveInput=value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            Jump();
        }
    }
    void Run()
    {
        if (isKnockback) return;
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }
    void Jump()
    {   
        if(jumpCount<2) playerAudio.PlaySFXClip(playerAudio.jumpClip);
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
            jumpCount = 1;
            isGrounded = false;
        }
        else if(jumpCount==1&&!isGrounded)
        {
            anim.SetBool("isDoubleJumping",true);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce);
            jumpCount++;
        }
    }
    void Flip()
    {
        if (moveInput.x < 0) transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (moveInput.x > 0) transform.localScale = new Vector3(1f, 1f, 1f);
    }
    void Push()
    {
        Vector2 dir=moveInput.x>0 ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            dir,
            0.3f,
            LayerMask.GetMask("Interactable"));

        anim.SetBool("isPushing", hit.collider != null);
        if (Mathf.Abs(rb.linearVelocity.x) < 0.01f) playerAudio.StopLoopClip();
        if (hit.collider != null)
        {
            if (!isPushing) 
            {
                playerAudio.PlayLoopClip(playerAudio.pushStoneClip);
            }
            isPushing = true;
        }
        else
        {
            if(isPushing) playerAudio.StopLoopClip();
            isPushing = false;
        }
    }
    public void ApplyKnockbackForce(Transform attacker)
    {
        isKnockback=true;
        float dir =Mathf.Sign(transform.position.x - attacker.position.x);
        rb.linearVelocity = new Vector2(knockbackForce * dir, rb.linearVelocity.y);
    }
    public void EndKnockback()
    {
        isKnockback = false;
    }
    public void SetGrounded()
    {
        bool grounded = groundCheck.IsGrounded();
        if (grounded&&!lastGrounded)
        {
            dust.SetTrigger("isAfterJumping");
            playerAudio.PlaySFXClip(playerAudio.landingClip);
            jumpCount = 0;
        }
        else if(!grounded&&lastGrounded)
        {
            if (rb.linearVelocity.y > 0)
            {
                dust.SetTrigger("isBeforeJumping");
            }
        }
        isGrounded = grounded;
        lastGrounded = grounded;
    }
    public void OnFootstep()
    {
        if (!isGrounded) return;
        playerAudio.PlayFootstep(groundCheck.GetGroundType());
    }
    public void OnLanding()
    {

    }
}
