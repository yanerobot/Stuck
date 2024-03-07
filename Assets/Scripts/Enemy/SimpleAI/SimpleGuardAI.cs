using UnityEngine;
using System.Collections;

public class SimpleGuardAI : MonoBehaviour
{
    [SerializeField] Transform rotateWithMovement;
    [SerializeField] EnemyEquipmentSystem es;
    [SerializeField] float shootDistance;
    [SerializeField] float runSpeed;
    [SerializeField] LayerMask shootable;
    [SerializeField] float activationDistance;
    [SerializeField] GameObject GFX;



    [Header("Auto")]
    [SerializeField] float timeToShoot = 3;
    [SerializeField] float delayBetweenBursts = 5;
    float timeShooting;
    float timeNotShooting = 1;
    bool shooting;
    [Header("Pistol")]


    Transform mainTarget;
    GameManager gameManager;
    Rigidbody2D rb;
    Animator animator;

    Vector2 currentGravity;

    bool isLookingLeft = true;

    void Awake()
    {
        rotateWithMovement.rotation = Quaternion.identity;
        isLookingLeft = false;
    }
    void Start()
    {
        GFX.SetActive(false);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
            throw new System.ArgumentException("Can't find gameManager");
        mainTarget = gameManager.player.transform;
        StartCoroutine(DistanceToTargetActivation());
    }

    void Update()
    {

        Flip();

        CheckGravity();

        if (es.CurrentWeapon != null && Vector2.Distance(transform.position, mainTarget.position) < shootDistance)
        {
            var hit = Physics2D.Raycast(transform.position, transform.Direction(mainTarget.position), shootDistance, shootable);

            if (hit.transform != mainTarget) return;

            Aim(mainTarget.position.AddTo(y: -0.2f));

            if (timeNotShooting > delayBetweenBursts)
            {
                if (!shooting)
                {
                    es.Use();
                    shooting = true;
                } else
                {
                    timeShooting += Time.deltaTime;
                    if (timeShooting > timeToShoot)
                    {
                        es.StopUsing();
                        shooting = false;
                        timeShooting = 0;
                        timeNotShooting = 0;
                    }
                }
            }
            else
            {
                timeNotShooting += Time.deltaTime;
            }
        }

    }

    IEnumerator DistanceToTargetActivation()
    {
        while(true)
        {
            GFX.SetActive(Vector2.Distance(transform.position, mainTarget.position) < activationDistance);
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    void CheckGravity()
    {
        if (currentGravity == Physics2D.gravity)
            return;
        currentGravity = Physics2D.gravity;
        print(currentGravity.y);
        if (currentGravity.y > 0)
            transform.rotation = Quaternion.Euler(180, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);

    }

    public void Flip()
    {
        var dirToTarget = Mathf.Sign(transform.Direction(mainTarget.position).x);
        if (dirToTarget < 0 && !isLookingLeft)
        {
            rotateWithMovement.localEulerAngles = rotateWithMovement.localEulerAngles.WhereY(180);
            isLookingLeft = true;
        }
        else if (dirToTarget > 0 && isLookingLeft)
        {
            rotateWithMovement.localEulerAngles = rotateWithMovement.localEulerAngles.WhereY(0);
            isLookingLeft = false;
        }
    }

    void Aim(Vector2 target)
    {
        if (target.x < transform.position.x)
        {
            var dir = target - (Vector2)transform.position;
            dir = Quaternion.Euler(0, -180, 0) * dir;
            target = dir + (Vector2)transform.position;
        }

        var aimDirection = (target - (Vector2)transform.position).normalized;

        es.Aim(aimDirection);
    }
}
