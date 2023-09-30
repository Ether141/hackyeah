using UnityEngine;

public abstract class BaseAI : MonoBehaviour
{
    [SerializeField] protected PatrolPath path;

    [Header("Detection")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] protected float detectionRayLength = 5f;
    [SerializeField] private float raysGap = 1f;

    protected bool isPlayerDetected = false;
    protected SpriteRenderer spriteRenderer;
    protected GameObject player;
    protected float startDetectionRayLength;

    public bool IsRightFaced => transform.eulerAngles.y == 0f;
    public float DistanceToPlayer => Vector2.Distance(transform.position, player.transform.position);
    public StateMachine StateMachine { get; private set; }

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = PlayerController.Player;
        startDetectionRayLength = detectionRayLength;

        InitializeStateMachine();
    }

    protected virtual void Update()
    {
        DetectPlayer();
        StateMachine.Update();
        print(StateMachine.CurrentState);
    }

    protected virtual void InitializeStateMachine() => StateMachine = new StateMachine();

    protected void Flip()
    { 
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, IsRightFaced ? 180f : 0f, transform.eulerAngles.z);
    }

    public bool IsInFront(Vector3 pos) => transform.InverseTransformPoint(pos).x > 0f;

    public void FaceTowards(Vector3 pos)
    {
        if (!IsInFront(pos))
            Flip();
    }

    private void DetectPlayer()
    {
        Vector3 originA = transform.position + (Vector3.up * raysGap);
        Vector3 originB = transform.position + (Vector3.down * raysGap);

        Debug.DrawLine(originA, originA + (transform.right * detectionRayLength), Color.blue);
        Debug.DrawLine(originA, originA + (-transform.right * detectionRayLength * 0.8f), Color.blue);
        Debug.DrawLine(originB, originB + (transform.right * detectionRayLength), Color.blue);
        Debug.DrawLine(originB, originB + (-transform.right * detectionRayLength * 0.8f), Color.blue);

        if (Physics2D.Raycast(originA, transform.right, detectionRayLength, playerLayer) || Physics2D.Raycast(originB, transform.right, detectionRayLength, playerLayer) ||
            Physics2D.Raycast(originA, -transform.right, detectionRayLength * 0.8f, playerLayer) || Physics2D.Raycast(originB, -transform.right, detectionRayLength * 0.8f, playerLayer))
        {
            if (!isPlayerDetected)
            {
                FaceTowards(player.transform.position);
            }

            isPlayerDetected = true;
        }
        else
        {
            isPlayerDetected = false;
        }
    }
}