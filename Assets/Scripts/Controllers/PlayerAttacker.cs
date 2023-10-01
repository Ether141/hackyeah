using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private List<EnemyStats> enemies = new List<EnemyStats>();

    private void Start()
    {
        enemies = FindObjectsOfType<EnemyStats>().ToList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyUp(KeyCode.LeftControl))
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (enemies.Count == 0)
        {
            return;
        }

        enemies.RemoveAll(e => e == null);
        EnemyStats closestEnemy = enemies.OrderBy(s => Vector2.Distance(transform.position, s.transform.position)).FirstOrDefault();
        closestEnemy?.Damage(1);
    }
}
