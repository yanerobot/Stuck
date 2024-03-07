using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Hint : Interactible
{
    [Header("Hint Refs")]
    public Image hintUI;
    public TextMeshProUGUI textMesh;
    public Image hintUIImage;
    [Header("HInt")]
    [SerializeField] Sprite hintUISprite;
    [SerializeField, TextArea] string hintUIText;
    [SerializeField] Color hintColor;
    [SerializeField] float fontSize;
    [SerializeField] Color fontColor;
    [Header("Disable")]
    [SerializeField] bool disableMainImage = false;
    public Image border;
    [SerializeField] Color disableColor;

    Color borderColorInitial, UIColorInitial;
    float initialSizeFont;

    InputMaster input;

    void Awake()
    {
        input = new InputMaster();
        input.Enable();
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
        if (disableMainImage && border != null)
        {
            borderColorInitial = border.color;
            UIColorInitial = hintUI.color;
            hintUI.color = disableColor;
            border.color = disableColor;
        }
        hintUI.gameObject.SetActive(true);
        hintUI.color = hintColor;
        if (hintUISprite != null)
        {
            hintUIImage.gameObject.SetActive(true);
            hintUIImage.sprite = hintUISprite;
        }
        else if (hintUIText != "")
        {
            textMesh.gameObject.SetActive(true);
            textMesh.text = hintUIText;
            if (fontSize != 0)
            {
                initialSizeFont = textMesh.fontSize;
                textMesh.fontSize = fontSize;
            }
            textMesh.color = fontColor;
        }
    }

    protected override void StopInteraction()
    {
        base.StopInteraction();
        if (disableMainImage && border != null)
        {
            border.color = borderColorInitial;
            hintUI.color = UIColorInitial;
        }
        if (fontSize != 0 && initialSizeFont != 0)
            textMesh.fontSize = initialSizeFont;
        hintUI.gameObject.SetActive(false);
        hintUIImage.sprite = null;
        hintUIImage.gameObject.SetActive(false);
        textMesh.text = "";
        textMesh.gameObject.SetActive(false);
    }
}
