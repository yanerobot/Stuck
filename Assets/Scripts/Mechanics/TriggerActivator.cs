using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    [SerializeField] Switcher[] Switchers;
    [SerializeField] bool keepState;
    [SerializeField] bool onlyPlayer;
    [SerializeField] bool enableRagdoll;


    private const int RAGDOLL_LAYER = 15;

    int rbCount;

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enableRagdoll && collision.gameObject.layer == RAGDOLL_LAYER)
            return;
        if (collision.TryGetComponent(out Rigidbody2D _))
        {
            if (onlyPlayer && !collision.TryGetComponent(out PlayerEquipmentSystem _))
                return;
            if (rbCount == 0)
            {
                ActivateAll(true);
                OnFisrtOneEnter(collision);
            }
            rbCount++;
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!enableRagdoll && collision.gameObject.layer == RAGDOLL_LAYER)
            return;
        if (collision.TryGetComponent(out Rigidbody2D _))
        {
            if (onlyPlayer && !collision.TryGetComponent(out PlayerEquipmentSystem _))
                return;
            if (rbCount == 1)
            {
                if (!keepState)
                    ActivateAll(false);
                OnLastOneExit();
            }
            rbCount--;
        }
    }

    void ActivateAll(bool value)
    {
        foreach (var switcher in Switchers)
        {
            switcher?.Activate();
        }
    }

    protected virtual void OnFisrtOneEnter(Collider2D collison) { }
    protected virtual void OnLastOneExit() { }
}
