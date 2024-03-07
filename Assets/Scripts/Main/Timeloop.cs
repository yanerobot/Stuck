using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Timeloop : MonoBehaviour
{
    [SerializeField] float loopTimeSeconds = 200;
    [SerializeField] GameManager gm;
    [SerializeField] Image timerUI;
    [SerializeField] ParticleSystem loopStartEffect;
    [SerializeField] internal AudioClip loopEndSound, loopStartSound;
    [SerializeField] MusicController mc;

    public Action<bool> beforeRestart;

    internal AudioSource src;

    float currentTime;
    bool loopEnded = false;

    void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        if (!gm.isIntroFinished) return; 
        currentTime = 0;
        loopEnded = false;
        loopStartEffect.Play();
        src.PlayOneShot(loopStartSound);
    }

    void FixedUpdate()
    {
        currentTime += Time.deltaTime;

        timerUI.fillAmount = 1 - currentTime / loopTimeSeconds;

        if (currentTime > loopTimeSeconds && loopEnded == false)
        {
            loopEnded = true;
            EndLoop();
        }
    }

    public void EndLoop()
    {
        if (gm.sceneHandler is IngameHandler)
        {
            var handler = gm.sceneHandler as IngameHandler;
            transform.position = handler.PlayerInitialPosition;
        }
        mc.SetState(MusicController.NORMAL_STATE);
        Time.timeScale = 1;
        gm.canvasHandler.HideAll();
        gm.canvasHandler.ShowGameOverScreen(true);
        src.PlayOneShot(loopEndSound);
        StartCoroutine(RestartLoop());
    }

    IEnumerator RestartLoop()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        beforeRestart?.Invoke(false);
        yield return new WaitForSecondsRealtime(0.5f);
        gm.RestartScene();
    }
}
