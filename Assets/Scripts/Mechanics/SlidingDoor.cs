using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SlidingDoor : Switcher
{
    [SerializeField] float speed;

    [SerializeField] Vector3 openOffset;
    Vector3 closedPosition, openPosition;

    AudioSource src;

    void Awake()
    {
        src = GetComponent<AudioSource>();
        closedPosition = transform.position;
        openPosition = transform.position + openOffset;
    }
    public override void Activation()
    {
        StopAllCoroutines();
        if (IsActivated)
            StartCoroutine(SlideTowards(openPosition));
        else
            StartCoroutine(SlideTowards(closedPosition));
    }

    IEnumerator SlideTowards(Vector3 target)
    {
        src.Play(); 
        var inititalPos = transform.localPosition;
        var smoothing = 0f;
        while (Vector3.Distance(transform.position, target) > 0)
        {
            transform.localPosition = Vector3.Lerp(inititalPos, target, smoothing);
            smoothing += Time.deltaTime * speed;
            yield return new WaitForFixedUpdate();
        }
        src.Stop();
    }
}
