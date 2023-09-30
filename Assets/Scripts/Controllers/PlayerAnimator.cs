using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerAnimator : MonoBehaviour
{
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //anim = GetComponent<Animator>();
    }

    private void Update()
    {
        FlipPlayer();
        HandleAnimator();
    }

    private void HandleAnimator()
    {
        
    }

    private void FlipPlayer()
    {
        if (playerController.IsMoving)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, playerController.HorizontalMovement < 0f ? 180f : 0f, transform.eulerAngles.z);
    }
}