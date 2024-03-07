using UnityEngine;
[RequireComponent(typeof(Animator), typeof(Health))]
public class PlayerMovement : RigidbodyMovement2D
{
    [Header("Player")]
    [SerializeField] float fallSpeedToDie = 30;

    Animator animator;
    InputMaster input;
    Health health;

    protected override void Awake()
    {
        base.Awake();
        input = new InputMaster();
        input.Main.Jump.performed += ctx => StartCoroutine(Jump());

        health = GetComponent<Health>();

        animator = GetComponent<Animator>();

        OnJump += SetJumpTrigger;
        OnLand += SetLandTrigger;
    }

    protected override void Update()
    {
        var locVel = transform.InverseTransformDirection(rb.velocity);

        var isRunning = isGrounded && Mathf.Abs(locVel.x) > minimumRunVelocity;
        var isFalling = !isGrounded;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isFalling", isFalling);
        base.Update();
    }
    public override void Freeze()
    {
        input.Disable();
    }

    public override void UnFreeze()
    {
        input.Enable();
    }

    protected override float GetHorizontalInput()
    {
        return input.Main.Run.ReadValue<float>();
    }

    void SetJumpTrigger()
    {
        animator.SetTrigger("Jumped");
    }

    void SetLandTrigger(float fallSpeed)
    {
        animator.SetTrigger("Landed");
        if (fallSpeed > fallSpeedToDie)
        {
            health.TakeDamage(health.maxHealth);
        }
    }
}
