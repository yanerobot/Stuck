using UnityEngine;

public class FinishGame : MonoBehaviour
{
    [SerializeField] Animator gameFinishedScreen;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement player))
        {
            player.Freeze();
            var equipment = player.GetComponent<PlayerEquipmentSystem>();
            equipment.finishGameObject = this;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Animator>().SetBool("GameFinished", true);
            
        }
    }

    public void ShowEndingScreen()
    {
        gameFinishedScreen.gameObject.SetActive(true);
        gameFinishedScreen.SetTrigger("Finished");

    }

    public void BackToMenu()
    {
        FindObjectOfType<GameManager>().ToMenu();
    }
}
