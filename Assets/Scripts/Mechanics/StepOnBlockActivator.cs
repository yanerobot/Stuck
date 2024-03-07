using UnityEngine;

public class StepOnBlockActivator : MonoBehaviour
{
    [SerializeField] Switcher swicher;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out PlayerEquipmentSystem _))
        {
            swicher?.Activate();
        }
    }
}
