using UnityEngine;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class PushButton : TriggerActivator
{
    Animator animator;

    [Header("Audio")]
    [SerializeField] float openPitch, closedPitch;
    [SerializeField] bool isBlue;



    AudioSource src;

    void Awake()
    {
        animator = GetComponent<Animator>();
        src = GetComponent<AudioSource>();
    }

    protected override void OnFisrtOneEnter(Collider2D collision)
    {
        src.pitch = openPitch;
        src.Play();
        animator.SetBool("Pressed", true);
    }

    protected override void OnLastOneExit()
    {
        src.pitch = closedPitch;
        src.Play();
        animator.SetBool("Pressed", false);
    }
}
