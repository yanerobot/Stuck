using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScientistHealth : EnemyHealth
{
    SimpleAIScientist AI;
    public GameObject enemyToSpawnOnDie;
    public Switcher switcher;
    GameObject newEnemy;

    protected override void Awake()
    {
        base.Awake();
        AI = GetComponent<SimpleAIScientist>();
    }

    void OnDestroy()
    {
        Destroy(newEnemy);
    }
    public override void OnDamage()
    {
        base.OnDamage();
        if (AI.runAway != null)
            AI.StopAllCoroutines();
        else
            AI.StartCoroutine(AI.RunAwayFrom(AI.mainTarget.position));
    }

    public override void Die()
    {
        switcher?.Activate();

        Invoke(nameof(SpawnEnemy), 0.5f);

        FindObjectOfType<MusicController>().SetState(MusicController.ACTION_STATE);
        base.Die();
    }

    void SpawnEnemy()
    {
        enemyToSpawnOnDie.SetActive(true);
    }
}
