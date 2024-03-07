using System;
using UnityEngine;

public class CodeLock : Interactible
{
    InputMaster input;
    [SerializeField] string unlockCombination;
    [SerializeField] Switcher[] switchers;
    [SerializeField] CodeLockUI codeLockUI;
    [SerializeField] bool enableReuse;
    public Color codeLockColor;
    public bool isStuffOnly = false;

    internal string code = "";
    public string UnlockCombination => unlockCombination;


    [HideInInspector]
    public CodeUIState currentState = CodeUIState.Default;

    void Awake()
    {
        input = new InputMaster();
        input.Main.Interact.performed += _ => SwitchInteraction();
    }
    void OnValidate()
    {
        try
        {
            int x = int.Parse(unlockCombination);
            
        }
        catch (FormatException)
        {
            Debug.LogError("Input string is invalid on GameObject: " + gameObject);
        }
    }

    protected override void Interact()
    {
        base.Interact();
        codeLockUI.SetCurrentCodeLock(this);

        if (enableReuse && currentState == CodeUIState.CorrectCode)
        {
            foreach (var switcher in switchers)
            {
                switcher.Activate(true);
            }
        }
    }

    protected override void StopInteraction()
    {
        base.StopInteraction();
        if (currentState != CodeUIState.CorrectCode)
            Clear();
        codeLockUI.UnsetCodelock(this);
    }

    public string AddNumber(int number)
    {
        if (currentState == CodeUIState.CorrectCode)
        {
            return UnlockCombination;
        }
        else if (currentState == CodeUIState.WrongCode)
        {
            currentState = CodeUIState.Default;
            codeLockUI.SetState(CodeUIState.Default);
        }
        if (code.Length >= unlockCombination.Length)
            Clear();

        code += number.ToString();
        if (code.Length == unlockCombination.Length)
            Verify();

        return code;
    }

    public void Clear()
    {
        code = "";
    }

    public void Verify()
    {
        if (code == unlockCombination)
        {
            currentState = CodeUIState.CorrectCode;
            codeLockUI.SetState(currentState);
            foreach(var switcher in switchers)
            {
                switcher?.Activate(true);
            }
        }
        else
        {
            currentState = CodeUIState.WrongCode;
            codeLockUI.SetState(currentState);
        }
    }

    public override void Freeze()
    {
        input.Disable();
    }

    public override void UnFreeze()
    {
        input.Enable();
    }
}
