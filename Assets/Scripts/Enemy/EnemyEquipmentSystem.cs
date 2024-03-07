using UnityEngine;

public class EnemyEquipmentSystem : EquipmentSystem
{
    [SerializeField] Weapon initialWeapon;

    SimpleGuardAI AI;
    public float attackRange;

    public Weapon CurrentWeapon => currentItem as Weapon;

    void Start()
    {
        AI = GetComponent<SimpleGuardAI>();

        if (initialWeapon == null) return;

        initialWeapon.gameObject.SetActive(true);
        SetItem(initialWeapon);
        Equip();
    }

    public override void Freeze()
    {
        
    }

    public override void UnFreeze()
    {
        
    }

    protected override void OnSetItem(Item item)
    {
        if (!(item is Weapon)) return;

        base.OnSetItem(item);
    }
}
