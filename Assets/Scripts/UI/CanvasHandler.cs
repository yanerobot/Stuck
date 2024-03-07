using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CanvasHandler : MonoBehaviour
{
    public List<GameObject> UIElements;

    void OnValidate()
    {
        StoreUIElements();
    }

    void Awake()
    {
        StoreUIElements();
    }
    void StoreUIElements()
    {
        UIElements = new List<GameObject>();
        foreach (Transform child in transform)
        {
            UIElements.Add(child.gameObject);
        }
    }
    public GameObject GetReference(string name)
    {
        var obj = transform.Find(name)?.gameObject;

        return obj;
    }

    public void Show(string name)
    {
        transform.Find(name)?.gameObject.SetActive(true);
    }
    public void HideAll()
    {
        foreach (var UIBlock in UIElements)
        {
            UIBlock.SetActive(false);
        }
    }

    public void Hide(string name)
    {
        transform.Find(name)?.gameObject.SetActive(false);
    }

    public void ShowLoadingScreen(bool flag)
    {
        if (flag) Show("LoadingScreen");
        else Hide("LoadingScreen");
    }
    public void ShowGameOverScreen(bool flag)
    {
        if (flag) Show("GameOverScreen");
        else Hide("GameOverScreen");
    }
    public void ShowLevelCompletedScreeen(bool flag)
    {
        if (flag) Show("LevelCompleted");
        else Hide("LevelCompleted");
    }
    public void ShowGameplayUI()
    {
        HideAll();
        Show("IngameUI");
    }
}
