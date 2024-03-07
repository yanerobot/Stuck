using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerController : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] string mixerFieldName;



    void OnEnable()
    {
        mixer.GetFloat(mixerFieldName, out float mixerVal);
        slider.value = Mathf.Pow(10, mixerVal * 0.05f);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat(mixerFieldName, Mathf.Log10(value) * 20);
    }
}
