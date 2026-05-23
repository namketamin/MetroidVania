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

    [SerializeField] private float knockbackForce = 1f;

    [SerializeField] private Animator dust;
    [SerializeField] private LayerMask enemyLayer;

    public bool IsGrounded { get; private set; }
    public bool LastGrounded { get; private set; }
    public bool IsPushing { get; private set; }
    public int JumpCount { get; private set; } = 0;
    public bool IsKnockback { get; private set; }

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
        Debug.Log(IsGrounded);
        playerAnim.UpdateMovement(
            IsGrounded,
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
        if (IsKnockback) return;
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }
    void Jump()
    {   
        if(JumpCount<2) playerAudio.PlaySFXClip(PlayerSFX.Jump);
        if (IsGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
            JumpCount = 1;
            IsGrounded = false;
        }
        else if(JumpCount==1&&!IsGrounded)
        {
            playerAnim.PlayDoubleJump();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce);
            JumpCount++;
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
            if (!IsPushing) 
            {
                playerAudio.PlayLoop(PlayerSFX.PushStone);
            }
            IsPushing = true;
        }
        else
        {
            if(IsPushing) playerAudio.StopLoop();
            IsPushing = false;
        }
    }
    public void ApplyKnockbackForce(Transform attacker)
    {
        IsKnockback=true;
        float dir =Mathf.Sign(transform.position.x - attacker.position.x);
        rb.linearVelocity = new Vector2(knockbackForce * dir, rb.linearVelocity.y);
    }
    public void EndKnockback()
    {
        IsKnockback = false;
    }
    public void SetGrounded()
    {
        bool grounded = groundCheck.IsGrounded();
        if (grounded&&!LastGrounded)
        {
            playerAnim.PlayLandingDust(dust);
            playerAudio.PlaySFXClip(PlayerSFX.Land);
            JumpCount = 0;
        }
        else if(!grounded&&LastGrounded)
        {
            if (rb.linearVelocity.y > 0)
            {
                playerAnim.PlayBeforeJumpDust(dust);
            }
        }
        IsGrounded = grounded;
        LastGrounded = grounded;
    }
    public void OnFootstep()
    {
        if (!IsGrounded) return;
        playerAudio.PlayFootstep(groundCheck.GetGroundType());
    }
    public void OnLanding()
    {

    }
}
