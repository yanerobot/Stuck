using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public bool isStatic = false;
    public int maxHealth;
    protected int currentHealth;

    internal bool isDead;

    void OnEnable()
    {
        Init();
    }

    public virtual void Init()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth < 0) currentHealth = 0;

        OnDamage();

        if (currentHealth == 0)
        {
            Die();
            isDead = true;
        }
    }

    public abstract void OnDamage();
    public abstract void Die();
}

