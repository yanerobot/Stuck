using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverTextRandomizer : MonoBehaviour
{
    [SerializeField, TextArea] string[] textToShow;

    TextMeshProUGUI text;
    int previousID;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        var id = Random.Range(0, textToShow.Length);
        if (id == previousID) 
            id = Random.Range(0, textToShow.Length);
        text.text = textToShow[id];
    }
}
