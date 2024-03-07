using UnityEngine;

public class SimpleGuardHealth : EnemyHealth
{
    [SerializeField] Transform Loot;
    EquipmentSystem es;

    SimpleGuardAI ai;
    protected override void Awake()
    {
        base.Awake();
        ai = GetComponent<SimpleGuardAI>();
        es = GetComponent<EquipmentSystem>();
    }

    public override void Die()
    {
        es.StopUsing();
        es.Toss();
        base.Die();
        ai.enabled = false;
        Loot.gameObject.SetActive(true);
        Loot.SetParent(null);
    }

}
