using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class RotatingDoor : Switcher
{

    [SerializeField] float speed = 2f;
    [SerializeField, Range(-360, 360)] float openZRotation, closedZRotation;
    [SerializeField] float timeToOpen;

    AudioSource src;

    void Awake()
    {
        src = GetComponent<AudioSource>();
    }
    public override void Activation()
    {
        StopAllCoroutines();
        if (IsActivated)
            StartCoroutine(RotateTowards(Quaternion.Euler(transform.localEulerAngles.WhereZ(openZRotation))));
        else
            StartCoroutine(RotateTowards(Quaternion.Euler(transform.localEulerAngles.WhereZ(closedZRotation))));
    }

    IEnumerator RotateTowards(Quaternion target)
    {
        src.Play();
        var time = 0f;
        var currentRotation = transform.rotation;
        while (time < timeToOpen)
        {
            transform.rotation = Quaternion.Slerp(currentRotation, target, time/ timeToOpen);
            time += Time.deltaTime * speed;
            yield return null;
        }
        transform.rotation = target;
        src.Stop();
    }
}

