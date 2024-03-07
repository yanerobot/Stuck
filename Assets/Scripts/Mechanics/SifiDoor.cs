using UnityEngine;

public class SifiDoor : Switcher
{
    Animator animator;
    AudioSource src;


    void Awake()
    {
        animator = GetComponent<Animator>();
        src = GetComponent<AudioSource>();
    }
    public override void Activation()
    {
        src.Play();
        animator.SetBool("IsOpened", IsActivated);
    }
}
