using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerEquipmentSystem : EquipmentSystem
{
    [Header("Sprite hint")]
    [SerializeField] Sprite buttonSprite;
    [SerializeField] Vector3 hintOffset;
    List<GameObject> allObjectEquipped;
    [Header("Finished Game")]
    [SerializeField] ParticleSystem loopEffect;
    [SerializeField] AudioClip loopEndClip;
    InputMaster input;
    internal FinishGame finishGameObject;

    RigidbodyMovement2D movement;

    public override void Awake()
    {
        base.Awake();
        input = new InputMaster();
        input.Enable();
        input.Main.Interact.performed += SwitchEquip;
        input.Main.Aim.performed += ctx => Aim(ctx.ReadValue<Vector2>());
        input.Main.Use.performed += ctx => Use();
        input.Main.Use.canceled += ctx => StopUsing();
        movement = GetComponent<RigidbodyMovement2D>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        allObjectEquipped = new List<GameObject>();
    }

    void OnDisable()
    {
        foreach(var obj in allObjectEquipped)
        {
            Destroy(obj);
        }
        allObjectEquipped = null;

        if (currentItem != null)
        {
            Destroy(currentItem.gameObject);
            currentItem = null;
        }
        itemsToPick.Clear();
    }


    public void SwitchEquip(InputAction.CallbackContext ctx)
    {
        if (itemsToPick.Count > 0)
            Equip();
        else
            Toss();
    }

    public override void DisableItemPick()
    {
        input.Main.Interact.performed -= SwitchEquip;
    }

    public override void EnableItemPick()
    {
        input.Main.Interact.performed += SwitchEquip;
    }

    public override void Freeze()
    {
        input.Disable();
    }

    public override void UnFreeze()
    {
        input.Enable();
    }

    public override void Equip()
    {
        base.Equip();

        if (currentItem != null) allObjectEquipped.Add(currentItem.gameObject);
        
        if (currentItem is Weapon || currentItem is Melee) return;
            movement.SetMaxSpeed(6);
    }

    public override void Toss()
    {
        base.Toss();

        movement.SetMaxSpeed();
    }

    public override void Aim(Vector2 mousePos)
    {
        if (currentItem == null) return;

        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos).WhereZ(0);

        if (mouseWorldPos.x < transform.position.x)
        {
            var dir = mouseWorldPos - transform.position;
            dir = Quaternion.Euler(0, -180, 0) * dir;
            mouseWorldPos = dir + transform.position;
        }

        var aimDirection = (mouseWorldPos - transform.position).normalized;

        base.Aim(aimDirection);
    }
    Item itemWithHint;
    protected override void OnSetItem(Item item)
    {
        if (itemWithHint != null && itemWithHint.hintObject != null)
        {
            Destroy(itemWithHint.hintObject);
        }
        if (buttonSprite != null)
        {
            if (item.hintObject != null)
            {
                Destroy(item.hintObject);
            }
            item.hintObject = new GameObject();
            item.hintObject.transform.position = item.transform.position + hintOffset;
            item.hintObject.AddComponent<SpriteRenderer>().sprite = buttonSprite;
            itemWithHint = item;
        }
    }

    protected override void OnUnsetItem(Item item)
    {
        if (item.hintObject != null)
            Destroy(item.hintObject);
    }

    public void PerformAttack()
    {
        (currentItem as Melee)?.Attack();
    }
    public void OnFinishGame()
    {
        GetComponent<PlayerMovement>().rotateWithMovement.gameObject.SetActive(false);
        finishGameObject.ShowEndingScreen();
    }

    public void TeleportEffect()
    {
        var src = GetComponent<AudioSource>();
        src.volume = 0.5f;
        src.PlayOneShot(loopEndClip);
        loopEffect.transform.position = transform.position;
        loopEffect.Play();
    }
}
