using UnityEngine;

public abstract class SceneHandler : MonoBehaviour
{
    internal GameManager gameManager { get; private set; }

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
}
