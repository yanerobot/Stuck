using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
public class EnemyAI : StateMachine
{
    [Header("References")]
    public EnemyEquipmentSystem equipmentSystem;
    public AIMovement movement;
    public DetectorAI detector;
    public Health health;

    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Vector2 initialPos;
    [HideInInspector] public GameObject weaponToPick;

    void Start()
    {
        initialPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        SetState(new Idle(this));
    }

    public bool CanShoot()
    {
        return equipmentSystem.CurrentWeapon != null && equipmentSystem.CurrentWeapon.CurrentBullets > 0;
    }
    public void CheckWeapons()
    {
        if (detector.weapons.Count > 0)
        {
            GameObject closestWeapon = detector.weapons.First();
            float closest = Mathf.Infinity;
            foreach (var weapon in detector.weapons)
            {
                var distance = transform.position.x - weapon.transform.position.x;
                if (distance > closest)
                    continue;

                closest = distance;
                closestWeapon = weapon;
            }
            weaponToPick = closestWeapon;
        }
    }

    public Vector2 UpdateTarget(GameObject target)
    {
        if (detector.playerHealth.gameObject == target.gameObject && detector.playerHealth != null)
        {
            return detector.playerHealth.transform.position;
        }
        if (target.gameObject == weaponToPick.gameObject)
        {
            CheckWeapons();
            if (weaponToPick.gameObject != null)
            {
                return weaponToPick.transform.position;
            }
        }

        return initialPos;
    }

    public float GetDirectionToTarget(Vector2 target)
    {
        return Mathf.Sign(target.x - transform.position.x);
    }
}