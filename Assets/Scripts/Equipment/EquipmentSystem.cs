using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public abstract class EquipmentSystem : MonoBehaviour, IFreezible
{
    [SerializeField] internal Transform itemHolder;
    [SerializeField] float tossForce = 2;

    [HideInInspector]
    public string holder;

    protected List<Item> itemsToPick;
    protected Item currentItem;

    internal Animator animator;

    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        Init();
    }
    public void Init()
    {
        itemsToPick = new List<Item>();
    }
    public virtual void Equip()
    {
        if (itemsToPick.Count == 0)
        {
            Debug.Log("Action cannot be performed! Item is not set!");
            return;
        }
        if (currentItem != null)
        {
            Toss();
        }

        currentItem = itemsToPick.Last();
        currentItem.WasEquippedBy(this);
    }

    public virtual void Toss()
    {
        if (currentItem == null)
        {
            Debug.Log("No item equipped!");
            return;
        }
        currentItem.WasTossedAway(tossForce);
        currentItem = null;
    }
    public void SetItem(Item item)
    {
        if (item == null)
            return;

        OnSetItem(item);
        itemsToPick.Add(item);
    }

    public void UnsetItem(Item item)
    {
        if (!itemsToPick.Contains(item))
            throw new System.DataMisalignedException($"{itemsToPick} doesn't contain an item: {item}. How it wasn't added to the list?");

        OnUnsetItem(item);
        itemsToPick.Remove(item);
    }

    public void Use()
    {
        if (currentItem == null) 
            return;

        currentItem.Use();
    }

    public void StopUsing()
    {
        if (currentItem == null) 
            return;

        currentItem.StopUsing();
    }

    public virtual void Aim(Vector2 aimDirection)
    {
        if (currentItem == null)
            return;

        currentItem.Aim(aimDirection);
    }

    protected virtual void OnSetItem(Item item) { }
    protected virtual void OnUnsetItem(Item item) { }

    public abstract void Freeze();
    public abstract void UnFreeze();

    public virtual void DisableItemPick() { }
    public virtual void EnableItemPick() { }
    
}