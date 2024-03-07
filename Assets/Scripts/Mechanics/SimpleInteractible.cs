using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInteractible : Interactible
{
    [SerializeField] Switcher[] switchers;
    InputMaster input;
    void Awake()
    {
        input = new InputMaster();
        input.Main.Interact.performed += _ => SwitchInteraction();
    }
    public override void Freeze()
    {
        input.Disable();
    }

    public override void UnFreeze()
    {
        input.Enable();
    }

    protected override void Interact()
    {
        base.Interact();
        foreach (var switcher in switchers)
        {
            switcher.Activate();
        }
        StopInteraction();
    }
}
