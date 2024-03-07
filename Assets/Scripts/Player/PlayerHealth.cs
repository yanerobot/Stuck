using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RigidbodyMovement2D))]
public class PlayerHealth : Health
{
    [SerializeField] Timeloop timeloop;
    [SerializeField] AudioClip damageClip, dieClip;
    [SerializeField, Range(0,1)] float volumeScale;
    [SerializeField] Image healthUI;
    [SerializeField] ParticleSystem damageEffect;
    [SerializeField] Ragdoll2D ragdoll;
    [SerializeField] float timeBeforeRestart;





    Animator animator;
    AudioSource src;
    RigidbodyMovement2D movement;
    EquipmentSystem es;

    void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<RigidbodyMovement2D>();
        es = GetComponent<EquipmentSystem>();
        src = GetComponent<AudioSource>();
    }

    public override void Init()
    {
        base.Init();
        healthUI.fillAmount = (float)currentHealth / maxHealth;
        ragdoll.ActivateRagdoll(false);
    }

    public override void Die()
    {
        es.Toss();
        movement.Freeze();
   //     animator.SetBool("Dead", true);
        src.PlayOneShot(dieClip);
        ragdoll.ActivateRagdoll(true);
        timeloop.beforeRestart += ragdoll.ActivateRagdoll;
        Invoke(nameof(EndLoop), timeBeforeRestart);
    }

    public override void OnDamage()
    {
        animator.SetTrigger("Damaged");
        src.PlayOneShot(damageClip);
        healthUI.fillAmount = (float)currentHealth / maxHealth;
        damageEffect.Play();
    }

    public void EndLoop()
    {
        timeloop.EndLoop();
    }

}
