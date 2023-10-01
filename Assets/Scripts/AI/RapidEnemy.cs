using UnityEngine;

public class RapidEnemy : BaseAI
{
    [SerializeField] private float patrolSpeed = 6f;

    [Header("Attacking")]
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float stopDistance = 2.5f;
    [SerializeField] private float distanceToAttack = 3f;
    [SerializeField] private float attackDelay = 1f;

    [Header("Misc")]
    [SerializeField] private GameObject boomEffect;

    public bool IsAttacking { get; set; } = false;
    public bool CanDealDamage { get; private set; } = true;
    public bool IsKnocked { get; private set; } = false;

    private Rigidbody2D rb2d;
    private Bone[] bones;

    public float DistanceToAttack => distanceToAttack;

    protected override void Start()
    {
        base.Start();
        bones = new Bone[transform.childCount];
        
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i] = transform.GetChild(i).GetComponent<Bone>();
        }

        rb2d = GetComponent<Rigidbody2D>();
    }

    protected override void InitializeStateMachine()
    {
        base.InitializeStateMachine();

        EnemyCommonStates.Idle idle = new EnemyCommonStates.Idle();
        EnemyCommonStates.Patrol patrol = new EnemyCommonStates.Patrol(this, patrolSpeed, path);
        RapidEnemyStates.Chase chase = new RapidEnemyStates.Chase(this, runSpeed, stopDistance);
        RapidEnemyStates.Attack attack = new RapidEnemyStates.Attack(this);
        RapidEnemyStates.LookingForPlayer lookingForPlayer = new RapidEnemyStates.LookingForPlayer(this, attackDelay);

        StateMachine.AddTransition(patrol, chase, () => isPlayerDetected && player.GetComponent<PlayerStats>().IsAlive);
        StateMachine.AddTransition(chase, attack, () =>
        {
            bool condition = isPlayerDetected && DistanceToPlayer <= distanceToAttack;

            if (condition)
            {
                anim.SetTrigger("attack");
            }

            return condition;
        });
        StateMachine.AddTransition(chase, lookingForPlayer, () => !isPlayerDetected);
        StateMachine.AddTransition(attack, lookingForPlayer, () => !IsAttacking);
        StateMachine.AddTransition(lookingForPlayer, attack, () =>
        {
            bool condition = isPlayerDetected && DistanceToPlayer <= distanceToAttack && lookingForPlayer.CanAttack;

            if (condition)
            {
                anim.SetTrigger("attack");
            }

            return condition;
        });
        StateMachine.AddTransition(lookingForPlayer, chase, () => isPlayerDetected && DistanceToPlayer > distanceToAttack && lookingForPlayer.CanAttack);
        StateMachine.AddTransition(lookingForPlayer, patrol, () => !isPlayerDetected && lookingForPlayer.CanAttack);
        StateMachine.AddTransition(lookingForPlayer, patrol, () => !player.GetComponent<PlayerStats>().IsAlive);
 
        StateMachine.SwitchState(patrol);
    }

    protected override void Update()
    {
        base.Update();

        AnimatorLogic();

        rb2d.velocity = Vector2.Lerp(rb2d.velocity, Vector2.zero, Time.deltaTime * 3f);
    }

    private void AnimatorLogic()
    {
        anim.SetBool("walk", StateMachine.IsState<EnemyCommonStates.Patrol>());
        anim.SetBool("chase", StateMachine.IsState<RapidEnemyStates.Chase>());
    }

    public void Attack()
    {
        IsAttacking = false;

        if (isPlayerDetected)
        {
            PlayerController.Player.GetComponent<PlayerStats>().TakeDamage(1);
        }
    }

    public override void OnDamage()
    {
        rb2d.AddForce(-transform.right * 30f, ForceMode2D.Impulse);
        IsKnocked = true;
        detectionRayLength *= 2f;
        this.WaitAndDo(() =>
        {
            IsKnocked = false;

            this.WaitAndDo(() => detectionRayLength = startDetectionRayLength, 1f);
        }, 1f);
    }

    public override void Kill()
    {
        foreach (Bone bone in bones)
        {
            if (bone != null)
            {
                bone.transform.SetParent(null);
                bone.gameObject.SetActive(true);
            }
        }

        boomEffect.transform.SetParent(null);
        boomEffect.SetActive(true);
        Destroy(gameObject);
    }
}
