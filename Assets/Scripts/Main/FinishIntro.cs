using UnityEngine;

public class FinishIntro : MonoBehaviour
{
    bool finished;
    [SerializeField] IngameHandler handler;
    void OnTriggerExit2D()
    {
        if (finished) return;
        finished = true;
        print("finishing intro " + gameObject.name);
        handler.FinishIntro();
        Destroy(gameObject);
    }
}
