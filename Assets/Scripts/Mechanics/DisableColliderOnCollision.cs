using UnityEngine;

public class DisableColliderOnCollision : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out PlayerMovement player))
        {
            Invoke(nameof(Disable), 0.2f);
        }
    }

    void Disable()
    {
        coll.enabled = false;
    }
}
