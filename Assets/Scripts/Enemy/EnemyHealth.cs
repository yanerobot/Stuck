using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyHealth : Health
{
    [SerializeField] ParticleSystem ps;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Collider2D coll;
    [SerializeField] Ragdoll2D ragdoll;



    internal Animator animator;
    

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void Die()
    {
        ragdoll.ActivateRagdoll(true);
        transform.rotation = Quaternion.identity;
    }

    public override void OnDamage()
    {
        animator.SetTrigger("Damaged");
        ps.Play();
    }
}
