using UnityEngine;

public class IngameHandler : SceneHandler
{
    [SerializeField] Transform levelStartingPoint;
    [SerializeField] Transform timeLoopPoint;

    public Vector2 PlayerInitialPosition
    {
        get
        {
            if (gameManager.isIntroFinished)
                return timeLoopPoint.position;
            return levelStartingPoint.position;
        }
    }
        


    public void LoadNextScene()
    {
        gameManager.LoadNextScene();
    }

    public void RestartScene()
    {
        gameManager.RestartScene();
    }

    public void ToMenu()
    {
        gameManager.LoadScene((int)StaticScenes.MAIN_MENU);
    }

    public void GameOver()
    {
        gameManager.canvasHandler.HideAll();
        gameManager.canvasHandler.ShowGameOverScreen(true);
        gameManager.RestartScene();
    }
    public void LevelCompleted()
    {
        gameManager.canvasHandler.ShowLevelCompletedScreeen(true);
    }

    public void FinishIntro()
    {
        gameManager.FinishIntro();
    }
}
