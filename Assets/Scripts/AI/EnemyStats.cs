using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private int currentHealth = 2;

    public bool IsAlive => currentHealth > 0;

    public void Damage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (currentHealth == 0)
        {
            if (TryGetComponent(out BaseAI ai))
            {
                ai.Kill();
            }
        }
        else
        {
            if (TryGetComponent(out BaseAI ai))
            {
                ai.OnDamage();
            }
        }
    }
}
