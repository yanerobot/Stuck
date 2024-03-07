using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthActivator : Health
{
    [SerializeField] Switcher[] switchers;
    [SerializeField] Animator animator;
    [SerializeField]  AudioSource src;

    public override void Die()
    {
        foreach(var switcher in switchers)
        {
            switcher.Activate();
        }
        animator.SetBool("isBroken", true);
        src.Play();
    }

    public override void OnDamage()
    {
    }
}
