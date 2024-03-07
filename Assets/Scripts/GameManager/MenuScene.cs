using UnityEngine;
public class MenuScene : SceneHandler
{
    public void NewGame()
    {
        gameManager.NewGame();
    }

    public void LoadScene(int sceneID)
    {
        gameManager.LoadScene(sceneID);
    }

    public void QuitGame()
    {
        gameManager.QuitGame();
    }
}
