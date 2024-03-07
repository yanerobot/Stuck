using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Elevator : Switcher
{
    [SerializeField] Animator darkenUIBG;
    [SerializeField] GameObject enemiesLast;
    [SerializeField] bool activateEnemies;
    [SerializeField] string musicState;




    PlayerMovement player;

    [SerializeField] Transform teleportPosition;

    void OnEnable()
    {
        StartCoroutine(FindPlayer());
    }

    public override void Activation()
    {
        darkenUIBG.gameObject.SetActive(true);
        darkenUIBG.SetTrigger("Transition");
        Invoke(nameof(Teleport), 1f);
    }

    void Teleport()
    {
        Physics2D.gravity = new Vector2(0, -9.81f);
        player.transform.position = teleportPosition.position;
        enemiesLast.SetActive(activateEnemies);
        var mc = FindObjectOfType<MusicController>();
        if (mc.currentState != musicState)
            mc.SetState(musicState);
    }

    IEnumerator FindPlayer()
    {
        while(player == null)
        {
            player = FindObjectOfType<PlayerMovement>();
            yield return new WaitForSeconds(0.2f);
        }
    }
}
