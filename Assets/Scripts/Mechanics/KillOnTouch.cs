using UnityEngine;

public class KillOnTouch : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Health damagable))
        {
            damagable.TakeDamage(damagable.maxHealth);
        }
    }
}
