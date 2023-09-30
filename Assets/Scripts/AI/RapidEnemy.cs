using UnityEngine;

public class RapidEnemy : BaseAI
{
    [SerializeField] private float patrolSpeed = 6f;

    [Header("Attacking")]
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float stopDistance = 2.5f;
    [SerializeField] private float distanceToAttack = 3f;
    [SerializeField] private float attackDelay = 1f;

    public bool IsAttacking { get; set; } = false;
    public bool CanDealDamage { get; private set; } = true;

    public float DistanceToAttack => distanceToAttack;

    protected override void InitializeStateMachine()
    {
        base.InitializeStateMachine();

        EnemyCommonStates.Idle idle = new EnemyCommonStates.Idle();
        EnemyCommonStates.Patrol patrol = new EnemyCommonStates.Patrol(this, patrolSpeed, path);
        RapidEnemyStates.Chase chase = new RapidEnemyStates.Chase(this, runSpeed, stopDistance);
        RapidEnemyStates.Attack attack = new RapidEnemyStates.Attack(this);
        RapidEnemyStates.LookingForPlayer lookingForPlayer = new RapidEnemyStates.LookingForPlayer(this, attackDelay);

        StateMachine.AddTransition(patrol, chase, () => isPlayerDetected);
        StateMachine.AddTransition(chase, attack, () => isPlayerDetected && DistanceToPlayer <= distanceToAttack);
        StateMachine.AddTransition(chase, lookingForPlayer, () => !isPlayerDetected);
        StateMachine.AddTransition(attack, lookingForPlayer, () => !IsAttacking);
        StateMachine.AddTransition(lookingForPlayer, attack, () => isPlayerDetected && DistanceToPlayer <= distanceToAttack && lookingForPlayer.CanAttack);
        StateMachine.AddTransition(lookingForPlayer, chase, () => isPlayerDetected && DistanceToPlayer > distanceToAttack && lookingForPlayer.CanAttack);
        StateMachine.AddTransition(lookingForPlayer, patrol, () => !isPlayerDetected && lookingForPlayer.CanAttack);
 
        StateMachine.SwitchState(patrol);
    }

    protected override void Update()
    {
        base.Update();

        if (StateMachine.IsState<RapidEnemyStates.LookingForPlayer>())
        {
            detectionRayLength = startDetectionRayLength * 0.6f;
        }
        else
        {
            detectionRayLength = startDetectionRayLength;
        }
    }

    public void Attack()
    {
        print("attack");
        IsAttacking = false;
    }
}
