using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    ParentConstraint parentConstraint;
    [Header("Item")]
    [SerializeField] float additionalTossForce;
    [SerializeField] float tossAngularVelocity;
    [SerializeField] float holdingOffsetX;



    protected Rigidbody2D rb;
    protected Collider2D col;
    protected EquipmentSystem character;
    internal GameObject hintObject;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }
    public void WasEquippedBy(EquipmentSystem character)
    {
        this.character = character;
        col.enabled = false;
        transform.SetParent(character.itemHolder.transform);
        transform.localPosition = Vector2.zero.WhereX(holdingOffsetX);
        transform.localEulerAngles = Vector2.zero;
        rb.isKinematic = true;
        rb.velocity *= 0;
        rb.angularVelocity *= 0;
        OnEquip();
    }
    public void WasTossedAway(float force)
    {
        OnToss();
        col.enabled = true;
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(transform.right * (force + additionalTossForce), ForceMode2D.Impulse);
        transform.rotation = Quaternion.identity;
        rb.angularVelocity = tossAngularVelocity;
        character = null;
    }
    protected virtual void OnEquip()
    {
        character.animator.SetBool("isHoldingBox", true);
    }
    protected virtual void OnToss()
    {
        character.animator.SetBool("isHoldingBox", false);
    }
    public virtual void Use() { }
    public virtual void StopUsing() { }

    public virtual void Aim(Vector2 aimDirection) { }
}
