using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DroppableLoot : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    void OnEnable()
    {
        rb.AddForce(new Vector2(Random.Range(-3, -9), 4), ForceMode2D.Impulse);
    }
}
