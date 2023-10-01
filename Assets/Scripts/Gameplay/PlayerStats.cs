using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth = 10;
    [SerializeField] private Image healthBar;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    private void Update()
    {
        healthBar.fillAmount = (float)CurrentHealth / maxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            print("dead");
        }
    }

    public void RegenerateHealth(int health)
    {
        if (CurrentHealth + health > maxHealth)
        {
            CurrentHealth = maxHealth;
        }
        else
        {
            CurrentHealth += health;
        }
    }
}
