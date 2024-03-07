using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class SimpleAIScientist : MonoBehaviour
{
    [SerializeField] public Transform mainTarget;
    [SerializeField] float minReachDistance;
    [SerializeField] float minRunAwayDistance;

    [SerializeField] float walkingSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float minRunSpeed = 4f;
    [SerializeField] Transform rotateWithMovement;

    public Collider2D coll;


    public Coroutine runAway;

    internal Rigidbody2D rb;
    internal Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        ScientistBehaivior();
    }

    void ScientistBehaivior()
    {
        rotateWithMovement.localEulerAngles = rotateWithMovement.localEulerAngles.WhereY(180);
        rb.velocity = transform.Direction(mainTarget.position) * walkingSpeed;
        animator.SetBool("walking", true);
        StartCoroutine(WalkTowardsLever());
    }

    IEnumerator WalkTowardsLever()
    {
        yield return new WaitUntil(() => Vector2.Distance(rb.position, mainTarget.position) < minReachDistance);
        rb.velocity = Vector2.zero;
        animator.SetTrigger("fixLever");
    }

    public IEnumerator RunAwayFrom(Vector2 position)
    {
        var dir = -transform.Direction(position).x;
        while (Vector2.Distance(transform.position, position) < minRunAwayDistance)
        {
            if (rb.velocity.x < minRunSpeed)
            {
                dir = -dir;
            }
            rb.velocity = new Vector2(dir, 0);
            animator.SetFloat("velocity", rb.velocity.x);
            yield return null;
        }
    }
}
