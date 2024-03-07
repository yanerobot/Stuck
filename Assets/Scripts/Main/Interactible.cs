using UnityEngine;
using System.Collections.Generic;

public abstract class Interactible : MonoBehaviour, IFreezible
{
    [SerializeField] Sprite buttonSprite;
    [SerializeField] Vector3 hintOffset;
    [SerializeField] bool keepStateAfterLeaving;


    PlayerEquipmentSystem user;
    protected bool isInteracting;

    GameObject keyHintObj;
    static List<Interactible> allInteractibles = new List<Interactible>();
    void Start()
    {
        Freeze();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var user = collision.GetComponent<PlayerEquipmentSystem>();
        if (user == null) return;

        DisableAllHints();

        allInteractibles.Add(this);

        this.user = user;

        PrepareInteraction();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        var user = collision.GetComponent<PlayerEquipmentSystem>();
        if (user == null) return;

        allInteractibles.Remove(this);
        
        StopPrepareInteraction();

        this.user = null;
    }

    public void PrepareInteraction()
    {
        if (user == null) return;

        user.DisableItemPick();
        CreateKeyHint();
        UnFreeze();
    }

    public void StopPrepareInteraction()
    {
        if (user == null) return;

        user.EnableItemPick();
        DestroyKeyHint();
        Freeze();
        if (keepStateAfterLeaving == false)
            StopInteraction();
    }

    static Interactible closestInteractible;
    static float closestDistance;
    public static void DisableAllHints()
    {
        foreach (var interactible in allInteractibles)
        {
            if (interactible.user == null) return;

            interactible.StopPrepareInteraction();
            var curDistance = Vector2.Distance(interactible.transform.position, interactible.user.transform.position);
            if (curDistance < closestDistance)
            {
                closestDistance = curDistance;
                closestInteractible = interactible;
            }
        }
    }

    public static void EnableClosestHint()
    {
        closestInteractible.PrepareInteraction();
    }
    protected void SwitchInteraction()
    {
        if (user == null) return;


        if (isInteracting)
            StopInteraction();
        else
            Interact();
    }
    void CreateKeyHint()
    {
        if (buttonSprite != null)
        {
            keyHintObj = new GameObject();
            keyHintObj.transform.SetParent(transform);
            keyHintObj.transform.localPosition = hintOffset;
            var sr = keyHintObj.AddComponent<SpriteRenderer>();
            sr.sprite = buttonSprite;
            sr.sortingLayerName = "UI";
            sr.sortingOrder = -1;
        }
    }

    void DestroyKeyHint()
    {
        if (keyHintObj != null)
        {
            Destroy(keyHintObj);
        }
    }

    protected virtual void Interact()
    {
        user.Freeze();
        isInteracting = true;
    }

    protected virtual void StopInteraction()
    {
        user.UnFreeze();
        isInteracting = false;
    }

    public abstract void Freeze();
    public abstract void UnFreeze();
}
