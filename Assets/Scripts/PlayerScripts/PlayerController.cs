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

    private Rigidbody2D rb;
    private Animator anim;
    private PlayerAudio playerAudio;
    private PlayerAnimator playerAnim;
    private GroundCheck groundCheck;
    private void Awake()
    {
        rb= GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerAudio=GetComponentInChildren<PlayerAudio>();
        playerAnim=GetComponent<PlayerAnimator>();
        groundCheck=GetComponentInChildren<GroundCheck>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        Debug.Log(isGrounded);
        playerAnim.UpdateMovement(
            isGrounded,
            rb.linearVelocity.x,
            rb.linearVelocity.y,
            moveInput
        );
        Flip();
        Push();
    }
    void FixedUpdate()
    {
        Run();
        SetGrounded();
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
        if(jumpCount<2) playerAudio.PlaySFXClip(PlayerSFX.Jump);
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
            jumpCount = 1;
            isGrounded = false;
        }
        else if(jumpCount==1&&!isGrounded)
        {
            playerAnim.PlayDoubleJump();
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

        playerAnim.SetPushing(hit.collider != null);
        if (Mathf.Abs(rb.linearVelocity.x) < 0.01f) playerAudio.StopLoop();
        if (hit.collider != null)
        {
            if (!isPushing) 
            {
                playerAudio.PlayLoop(PlayerSFX.PushStone);
            }
            isPushing = true;
        }
        else
        {
            if(isPushing) playerAudio.StopLoop();
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
            playerAnim.PlayLandingDust(dust);
            playerAudio.PlaySFXClip(PlayerSFX.Land);
            jumpCount = 0;
        }
        else if(!grounded&&lastGrounded)
        {
            if (rb.linearVelocity.y > 0)
            {
                playerAnim.PlayBeforeJumpDust(dust);
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
