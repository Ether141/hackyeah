using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAnimator : MonoBehaviour
{
    private PlayerController playerController;
    private Animator anim;

    private bool isGrounded = true;
    private float notGroundedTimer = 0f;
    private bool jumpCalled = false;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        FlipPlayer();
        HandleAnimator();
    }

    public void CallJump()
    {
        notGroundedTimer = 0.5f;
        isGrounded = false;
    }

    private void HandleAnimator()
    {
        if (jumpCalled)
        {
            isGrounded = playerController.IsGrounded;
        }
        else
        {
            if (isGrounded && !playerController.IsGrounded)
            {
                notGroundedTimer += Time.deltaTime;

                if (notGroundedTimer >= 0.076f)
                {
                    isGrounded = false;
                }
            }
            else
            {
                notGroundedTimer = 0f;
            }

            if (playerController.IsGrounded)
                isGrounded = true;
        }

        anim.SetFloat("x", playerController.IsMoving ? Mathf.Abs(playerController.HorizontalMovement) : 0f);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void FlipPlayer()
    {
        if (playerController.IsMoving)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, playerController.HorizontalMovement < 0f ? 180f : 0f, transform.eulerAngles.z);
    }
}