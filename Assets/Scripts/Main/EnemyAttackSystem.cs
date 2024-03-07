using UnityEngine;

public class EnemyAttackSystem : MonoBehaviour
{
    [SerializeField] EnemyEquipmentSystem es;

    [SerializeField] Transform meleePoint;

    public float attackRange;
    public void Shoot(Vector2 direction)
    {
        if (es.CurrentWeapon != null && es.CurrentWeapon is Weapon)
        {

        }
    }
}
