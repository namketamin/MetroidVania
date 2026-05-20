using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        
    }
    public void UpdateMovement(bool isGrounded, float xVelocity, float yVelocity,Vector2 moveInput)
    {
        animator.SetBool("isRunning", Mathf.Abs(moveInput.x) > 0.01f && isGrounded);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", yVelocity);
        animator.SetFloat("xVelocity", Mathf.Abs(moveInput.x));

        if (animator.GetBool("isDoubleJumping") && yVelocity < 0)
            animator.SetBool("isDoubleJumping", false);
    }
    public void PlayDoubleJump()
    {
        animator.SetBool("isDoubleJumping", true);
    }

    public void SetPushing(bool pushing)
    {
        animator.SetBool("isPushing", pushing);
    }

    public void PlayBeforeJumpDust(Animator dust)
    {
        dust.SetTrigger("isBeforeJumping");
    }

    public void PlayLandingDust(Animator dust)
    {
        dust.SetTrigger("isAfterJumping");
    }
}
