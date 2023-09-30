using System.Collections;
using UnityEngine;

public class RapidEnemyStates
{
    public class Chase : IState
    {
        private readonly RapidEnemy rapidEnemy;
        private readonly Transform transform;
        private readonly float runSpeed;
        private readonly float stopDistance;

        public Chase(RapidEnemy rapidEnemy, float runSpeed, float stopDistance)
        {
            this.rapidEnemy = rapidEnemy;
            this.transform = rapidEnemy.transform;
            this.runSpeed = runSpeed;
            this.stopDistance = stopDistance;
        }

        public void OnStateEnter()
        {
            
        }

        public void OnStateExit()
        {
            
        }

        public void Tick()
        {
            Vector3 targetPosition = new Vector3(PlayerController.Player.transform.position.x, transform.position.y);
            targetPosition += -transform.right * stopDistance;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * runSpeed);
        }
    }

    public class Attack : IState
    {
        private readonly RapidEnemy rapidEnemy;
        private readonly Transform transform;

        public Attack(RapidEnemy rapidEnemy)
        {
            this.rapidEnemy = rapidEnemy;
            this.transform = rapidEnemy.transform;
        }

        public void OnStateEnter()
        {
            rapidEnemy.IsAttacking = true;
            rapidEnemy.Attack();
        }

        public void OnStateExit()
        {

        }

        public void Tick()
        {

        }
    }

    public class LookingForPlayer : IState
    {
        private readonly RapidEnemy rapidEnemy;
        private readonly float attackDelay;
        private IEnumerator coroutine;

        public bool CanAttack { get; private set; } = false;

        public LookingForPlayer(RapidEnemy rapidEnemy, float attackDelay)
        {
            this.rapidEnemy = rapidEnemy;
            this.attackDelay = attackDelay;
        }

        public void OnStateEnter()
        {
            CanAttack = false;
            
            if (coroutine != null)
            {
                rapidEnemy.StopCoroutine(coroutine);
            }

            coroutine = rapidEnemy.WaitAndDo(() =>
            {
                CanAttack = true;
                coroutine = null;
            }, attackDelay);
        }

        public void OnStateExit()
        {
            
        }

        public void Tick()
        {
            
        }
    }
}
