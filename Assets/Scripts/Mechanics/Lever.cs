using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Lever : MonoBehaviour
{
    [SerializeField] Switcher[] switchers;
    [SerializeField] ParticleSystem ps;
    [SerializeField] GameObject scientistToActivate;

    bool Once;

    Animator animator;

    InputMaster input;
    bool isOpen;

    PlayerEquipmentSystem user;


    void OnEnable()
    {
        input = new InputMaster();
        input.Main.Interact.performed += _ => SwitchState();
        animator = GetComponent<Animator>();
    }

    void OnDisable()
    {
        input.Dispose();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out user))
        {
            user.DisableItemPick();
            input.Enable();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (user == null) return;

        if (collision.gameObject == user.gameObject)
        {
            user.EnableItemPick();
            input.Disable();
        }
    }

    void SwitchState()
    {
        if (isOpen) return;
        if (!ps.isPlaying)
            ps.Play();
        isOpen = !isOpen;
        animator.SetBool("isOpen", true);
        Invoke(nameof(Activate), 3f);
    }

    void Activate()
    {
        if (Once) return;

        Once = true;

        foreach (var switcher in switchers)
        {
            if (switcher is SifiDoor)
            {
                switcher?.Activate();

                scientistToActivate.SetActive(true);
                continue;
            }
            switcher?.Activate();
        }
    }
}
