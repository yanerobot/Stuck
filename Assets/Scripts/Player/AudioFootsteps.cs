using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(RigidbodyMovement2D))]
public class AudioFootsteps : MonoBehaviour
{
    [SerializeField] AudioClip footstepClip;
    [SerializeField] float stepOffsetSeconds;

    AudioSource src;
    RigidbodyMovement2D holder;

    bool started;
    bool landed;

    void Awake()
    {
        src = GetComponent<AudioSource>();
        holder = GetComponent<RigidbodyMovement2D>();
        holder.OnLand += _ => { src.PlayOneShot(footstepClip); landed = true;  };
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(holder.CurrentVelocity.x) > 3f && holder.isGrounded && !started && !landed)
        {
            started = true;
            StartCoroutine(PlayStepAudio());
        }
        else if ((Mathf.Abs(holder.CurrentVelocity.x) < 3f && started) || !holder.isGrounded)
        {
            StopAllCoroutines();
            started = false;
        }
        landed = false;
    }

    IEnumerator PlayStepAudio()
    {
        while(true)
        {
            src.pitch = 1 + Random.Range(-.1f, .1f);
            src.PlayOneShot(footstepClip);
            yield return new WaitForSeconds(stepOffsetSeconds);
        }
    }
}
