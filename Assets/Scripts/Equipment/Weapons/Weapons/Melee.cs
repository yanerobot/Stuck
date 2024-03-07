using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Melee : Item
{
    [Header("Melee")]
    [SerializeField] LayerMask hittableLayer;
    [SerializeField] int damage;
    [SerializeField] AudioClip onStaticHit, onHealthHit, onAirHit;
    [SerializeField, Range(0,1f)] float hitVolScale;
    [SerializeField] Transform attackPoint;



    AudioSource src;
    SpriteRenderer GFX;
    CapsuleCollider2D coll;

    protected override void Awake()
    {
        base.Awake();
        src = GetComponent<AudioSource>();
        GFX = GetComponentInChildren<SpriteRenderer>();
        coll = GFX.GetComponent<CapsuleCollider2D>();
    }
    public override void Use()
    {
        character.animator.SetTrigger("Attacked");
        src.PlayOneShot(onAirHit, 1);
    }
    protected override void OnEquip()
    {
        print("Set trigger to true");
        character.animator.SetBool("isHoldingMelee", true);
        coll.enabled = false;
    }

    protected override void OnToss()
    {
        print("Set trigger to false");
        character.animator.SetBool("isHoldingMelee", false);
        coll.enabled = true;
    }

    public void Attack()
    {
        var cols = Physics2D.OverlapCircleAll(attackPoint.position, 1f, hittableLayer);
        foreach(var col in cols)
        {
            print(col.gameObject);
            if (col.TryGetComponent(out Health health) && !health.isStatic)
            {
                print("Damaging " + col.gameObject);
                src.PlayOneShot(onHealthHit, hitVolScale);
                health.TakeDamage(damage);
            }
            else if (col != null)
            {
                print(col.gameObject.name);
                if (health != null && health.isStatic)
                {
                    health.TakeDamage(damage);
                }
                src.PlayOneShot(onStaticHit, hitVolScale);
            }
        }
    }
}
