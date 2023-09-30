using UnityEngine;

public class EnemyCommonStates
{
    public class Patrol : IState
    {
        private readonly BaseAI ai;
        private readonly Transform transform;
        private readonly PatrolPath path;
        private readonly float speed;

        private Vector2 targetPosition;

        public Patrol(BaseAI ai, float speed, PatrolPath path)
        {
            this.ai = ai;
            this.transform = ai.transform;
            this.speed = speed;
            this.path = path;
        }

        public void OnStateEnter() { }

        public void OnStateExit() { }

        public void Tick()
        {
            if (!path.IsOnPath(transform.position))
            {
                Vector2 targetPos = new Vector2(path.ClosestPoint(transform.position).x, transform.position.y);
                ai.FaceTowards(targetPos);
                transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
            }
            else
            {
                if (path.IsAtStart(transform.position))
                {
                    targetPosition = path.EndPosition;
                }
                else if (path.IsAtEnd(transform.position))
                {
                    targetPosition = path.StartPosition;
                }

                targetPosition = new Vector2(targetPosition.x, transform.position.y);
                ai.FaceTowards(targetPosition);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
            }
        }
    }

    public class Idle : IState
    {
        public void OnStateEnter()
        {

        }

        public void OnStateExit()
        {

        }

        public void Tick()
        {

        }
    }
}
