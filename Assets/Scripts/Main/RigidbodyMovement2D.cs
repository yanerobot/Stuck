using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class RigidbodyMovement2D : MonoBehaviour, IFreezible
{
    [Header("Main")]
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector3 groundCheckSize;

    protected Rigidbody2D rb;
    protected Vector2 currentGravity;

    [Header("Movement")]
    [SerializeField] float maxVelocity;
    [SerializeField] float runSpeed;
    [SerializeField] internal Transform rotateWithMovement;
    [Range(0, 4)] public float minimumRunVelocity = 3f;
    [Range(0, 4)] public float minimunFallVelocity = 3f;
    internal bool isGrounded;
    float lastDir;
    
    [Header("Jump")]
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpSmoothingDelay = 0.2f;
    float fallSpeed;

    public Vector2 CurrentVelocity => rb.velocity;

    public delegate void Action();
    public delegate void ActionWithValue(float value);
    public event ActionWithValue OnLand;
    public event Action OnJump;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentGravity = Physics2D.gravity;
    }

    void OnEnable()
    {
        Init();
    }

    void OnDisable()
    {
        Terminate();
    }

    public void SetMaxSpeed(float speed = 0)
    {
        if (speed == 0)
        {
            maxVelocity = 13;
            return;
        }
        maxVelocity = speed;
    }

    protected virtual void Update()
    {
        isGrounded = GroundCheck();
        ForceRun();
        CheckGravity();
    }

    public void Init()
    {
        FreePosition();
        UnFreeze();
    }

    public void Terminate()
    {
        StopAllCoroutines();
        CancelInvoke();
        Freeze();
    }
    public void FixPosition()
    {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
    }
    public void FreePosition()
    {
        rb.isKinematic = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundChecker.position, groundCheckSize);
    }
    bool GroundCheck()
    {
        var isFlyingUp = Mathf.Abs(rb.velocity.y) > 1 && Mathf.Sign(Physics2D.gravity.y) != Mathf.Sign(rb.velocity.y);

        if (isFlyingUp)
            return false;

        RaycastHit2D[] hits = Physics2D.BoxCastAll (groundChecker.position, groundCheckSize, 0, -transform.up, 0, groundLayer);

        var isGroundedTemp = false;
        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.isTrigger == false)
            {
                isGroundedTemp = true;
                break;
            }
        }
        if (!isGrounded && isGroundedTemp) 
        {
            OnLand?.Invoke(fallSpeed);
        }

        fallSpeed = Mathf.Abs(rb.velocity.y);

        return isGroundedTemp;
    }
    void ForceRun()
    {
        var inputX = GetHorizontalInput();
        FlipTowards(inputX);

        if (rb.velocity.x > maxVelocity && inputX > 0 ||
            rb.velocity.x < -maxVelocity && inputX < 0)
            return;

        var dir = inputX;
        if (dir == 0 && rb.velocity.x != 0)
        {
            if (Mathf.Abs(rb.velocity.x) > 1f)
            {
                dir = Mathf.Sign(-rb.velocity.x);
            }
        }
        rb.AddForce(transform.right * dir * runSpeed, ForceMode2D.Impulse);
        if (dir == 0)
            rb.velocity = rb.velocity.WhereX(0);
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

    void FlipTowards(float inputX)
    {
        if (inputX == 0 || Mathf.Sign(lastDir) == Mathf.Sign(inputX))
            return;

        lastDir = inputX;
        rotateWithMovement.localEulerAngles = rotateWithMovement.localEulerAngles.AddTo(y: -180);
    }
    protected IEnumerator Jump()
    {
        if (!isGrounded)
        {
            yield return new WaitForSeconds(jumpSmoothingDelay);
            if (!isGrounded)
                yield break;
        }

        rb.AddForce(-Physics2D.gravity.normalized * jumpHeight, ForceMode2D.Impulse);
        OnJump?.Invoke();
    }
    public abstract void Freeze();

    public abstract void UnFreeze();

    protected abstract float GetHorizontalInput();
}
