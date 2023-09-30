using System;
using System.Net;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public bool canMove = true;
    public bool canRun = true;
    public float walkSpeed = 4f;
    public float runSpeed = 7f;

    [Header("Grounded status")]
    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] private Vector3 topCheckOffset;
    [SerializeField] private float groundCheckRaysGap = 0.5f;

    [Header("Jumping")]
    public bool canJump = true;
    public float jumpForce = 8f;
    public float maxJumpTime = 1f;
    [Range(0f, 1f)] public float inAirControl = 1f;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;

    private Vector2 movementVelocity;
    private bool jumpWasCalled = false;
    private float jumpTimeCounter = 0f;
    private bool isGroundedInstant = false;
    private float notGroundedTimer = 0f;

    private bool forceMovement = false;
    private float forcedMovementSpeed = 2f;
    private Vector2 forcedPosition;

    private Rigidbody2D rb2d;
    private CapsuleCollider2D col;
    private PlayerAnimator animator;

    private const float CheckGroundRayLength = 0.75f;

    public static GameObject Player { get; private set; }

    public float HorizontalMovement { get; private set; } = 0f;
    public float VerticalMovement { get; private set; } = 0f;
    public float CurrentSpeed => canMove ? (!IsRunning ? walkSpeed * (!IsGrounded && jumpWasCalled ? inAirControl : 1f) : runSpeed) : 0f;
    public bool IsMoving => Mathf.Abs(HorizontalMovement) + Mathf.Abs(VerticalMovement) > 0.1f && canMove;
    public bool IsRunning => canRun && IsMoving && Input.GetButton("Run");
    public bool IsGrounded { get; private set; } = true;
    public Bounds CharacterBounds => col.bounds;
    public Vector2 CurrentVelocity => rb2d.velocity;
    public bool IsSomethingAbove
    {
        get
        {
            const float dis = 0.5f;

            RaycastHit2D leftHit = Physics2D.Raycast(transform.position + topCheckOffset + (Vector3.left * groundCheckRaysGap), Vector2.up, dis, groundLayer);
            RaycastHit2D centerHit = Physics2D.Raycast(transform.position + topCheckOffset, Vector2.up, dis, groundLayer);
            RaycastHit2D rightHit = Physics2D.Raycast(transform.position + topCheckOffset + (Vector3.right * groundCheckRaysGap), Vector2.up, dis, groundLayer);

            return leftHit.transform != null || centerHit.transform != null || rightHit.transform != null;
        }
    }

    public event Action OnLand;
    private bool onLandWasInvoked = false;

    private void Awake() => Player = gameObject;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        Debug.DrawLine(transform.position + topCheckOffset + (Vector3.left * groundCheckRaysGap), transform.position + topCheckOffset + (Vector3.left * groundCheckRaysGap) + (Vector3.up * 0.5f));
        Debug.DrawLine(transform.position + topCheckOffset, transform.position + topCheckOffset + (Vector3.up * 0.5f));
        Debug.DrawLine(transform.position + topCheckOffset + (Vector3.right * groundCheckRaysGap), transform.position + topCheckOffset + (Vector3.right * groundCheckRaysGap) + (Vector3.up * 0.5f));

        CollectInput();
        CheckGroundedState();
        JumpController();
        ForcePosition();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        movementVelocity = new Vector2(HorizontalMovement * CurrentSpeed, 0f);
        Vector2 resultVelocity;
        resultVelocity = new Vector2(movementVelocity.x, rb2d.velocity.y);
        rb2d.velocity = resultVelocity;
    }

    private void CollectInput()
    {
        HorizontalMovement = Input.GetAxisRaw("Horizontal");
        VerticalMovement = Input.GetAxisRaw("Vertical");

        if (IsMoving)
            forceMovement = false;
    }

    private void JumpController()
    {
        if (Input.GetButtonDown("Jump") && canJump && IsGrounded && canMove)
        {
            animator.CallJump();
            jumpWasCalled = true;
            rb2d.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetButton("Jump") && jumpWasCalled && jumpTimeCounter < maxJumpTime && !IsSomethingAbove)
        {
            jumpTimeCounter += Time.deltaTime;
            rb2d.velocity = Vector2.up * jumpForce;
        }
        else
        {
            jumpTimeCounter = 0f;
            jumpWasCalled = false;
        }

        if (Input.GetButtonUp("Jump"))
            jumpWasCalled = false;
    }

    private void CheckGroundedState()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position + groundCheckOffset + (groundCheckRaysGap * Vector3.left), Vector2.down, CheckGroundRayLength, groundLayer);
        RaycastHit2D centerHit = Physics2D.Raycast(transform.position + groundCheckOffset, Vector2.down, CheckGroundRayLength, groundLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + groundCheckOffset + (groundCheckRaysGap * Vector3.right), Vector2.down, CheckGroundRayLength, groundLayer);

        Debug.DrawLine(transform.position + groundCheckOffset + (groundCheckRaysGap * Vector3.left), transform.position + groundCheckOffset + (groundCheckRaysGap * Vector3.left) + (Vector3.down * CheckGroundRayLength), Color.green);
        Debug.DrawLine(transform.position + groundCheckOffset, transform.position + groundCheckOffset + (Vector3.down * CheckGroundRayLength), Color.green);
        Debug.DrawLine(transform.position + groundCheckOffset + (groundCheckRaysGap * Vector3.right), transform.position + groundCheckOffset + (groundCheckRaysGap * Vector3.right) + (Vector3.down * CheckGroundRayLength), Color.green);

        bool isCollision = leftHit.transform != null || centerHit.transform != null || rightHit.transform != null;
        bool prevState = IsGrounded;

        isGroundedInstant = isCollision;

        if (IsGrounded != isGroundedInstant)
        {
            notGroundedTimer += Time.deltaTime;

            if (notGroundedTimer > 0.2f)
            {
                IsGrounded = isGroundedInstant;
            }
        }
        else
        {
            notGroundedTimer = 0f;
        }

        if (!prevState && IsGrounded && !onLandWasInvoked)
        {
            onLandWasInvoked = true;
            OnLand?.Invoke();
        }

        if (IsGrounded)
        {
            onLandWasInvoked = false;
            jumpWasCalled = false;
        }
    }

    private void ForcePosition()
    {
        if (forceMovement)
            transform.position = Vector3.Lerp(transform.position, forcedPosition, Time.deltaTime * forcedMovementSpeed);
    }

    public void Death()
    {
        canMove = false;
        rb2d.velocity = Vector2.up * jumpForce;
        jumpWasCalled = true;
        col.enabled = false;
    }

    public void MoveTo(Vector2 pos, float speed)
    {
        forcedPosition = pos;
        forcedMovementSpeed = speed;
        forceMovement = true;
    }
}