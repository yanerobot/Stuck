using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CodeLockUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI screenTextField;
    [SerializeField] RectTransform stuffOnlyTextUI;

    [SerializeField] Image MainBody;

    [SerializeField] Image screenImage;
    [SerializeField] Color correctCodeColor, wrongCodeColor, defaultColor;

    CodeLock currentCodeLock;

    Button[] codeButtons;

    void Awake()
    {
        AddButtonsToEvent();
    }

    void AddButtonsToEvent()
    {
        codeButtons = GetComponentsInChildren<Button>();


        foreach (var button in codeButtons)
        {
            int number;
            var isInt = int.TryParse(button.name, out number);
            if (!isInt)
            {
                Debug.Log($"{button.name} can't be parsed to Int");
                continue;
            }
            button.onClick.AddListener(() => RecieveInput(number));
        }
    }
    void SetTextField(string currentCodeText)
    {
        for (int i = currentCodeText.Length; i < currentCodeLock.UnlockCombination.Length; i++)
        {
            currentCodeText += "_";
        }
        screenTextField.text = currentCodeText;
    }
    void RecieveInput(int number)
    {
        if (currentCodeLock == null)
        {
            Debug.LogError("There is no lock, but you got access to it");
            return;
        }

        var currentCodeText = currentCodeLock.AddNumber(number);
        SetTextField(currentCodeText);
    }

    public void RemoveLastCharacter()
    {
        if (currentCodeLock == null)
        {
            Debug.LogError("There is no lock, but you got access to it");
            return;
        }

        if (currentCodeLock.code.Length <= 0) return;

        currentCodeLock.code = currentCodeLock.code.Remove(currentCodeLock.code.Length - 1);
        SetTextField(currentCodeLock.code);
    }

    public void ClearTextField()
    {
        if (currentCodeLock == null)
        {
            Debug.LogError("There is no lock, but you got access to it");
            return;
        }

        currentCodeLock.code = "";
        SetState(CodeUIState.Default);
    }

    public void SetCurrentCodeLock(CodeLock codeLock)
    {
        currentCodeLock = codeLock;
        if (currentCodeLock.isStuffOnly)
            stuffOnlyTextUI.gameObject.SetActive(true);
        MainBody.color = currentCodeLock.codeLockColor;
        gameObject.SetActive(true);
        if (codeLock)

        if (currentCodeLock.currentState == CodeUIState.WrongCode)
            currentCodeLock.currentState = CodeUIState.Default;
        
        SetState(currentCodeLock.currentState);
    }
    public void UnsetCodelock(CodeLock codeLock)
    {
        if (currentCodeLock != codeLock) return;

        MainBody.color = Color.black;
        if (currentCodeLock.isStuffOnly)
            stuffOnlyTextUI.gameObject.SetActive(false);
        SetState(CodeUIState.Default);
        currentCodeLock = null;
        gameObject.SetActive(false);
    }
    public void SetState(CodeUIState state)
    {
        switch(state) {
            case CodeUIState.Default:
                screenImage.color = defaultColor;
                SetTextField("");
                break;
            case CodeUIState.CorrectCode:
                screenImage.color = correctCodeColor;
                SetTextField(currentCodeLock.UnlockCombination);
                Invoke(nameof(Deactivate), 1);
                break;
            case CodeUIState.WrongCode:
                screenImage.color = wrongCodeColor;
                break;
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

public enum CodeUIState
{
    Default,
    CorrectCode,
    WrongCode
}
