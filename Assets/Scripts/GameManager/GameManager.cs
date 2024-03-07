using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : StateMachineStatic
{
    [SerializeField] MusicController mc;

    public GameObject player;
    public GameObject gameplay;
    public CanvasHandler canvasHandler;
    public int currentSceneID;

    [HideInInspector]
    public Camera mainCamera;

    internal SceneHandler sceneHandler;

    InputMaster input;

    bool isPaused;

    public bool isIntroFinished = false;
    void Start()
    {
        input = new InputMaster();
        input.Enable();
        input.Main.Pause.performed += _ => PauseGame();
        mainCamera = Camera.main;
        gameplay.SetActive(false);
        currentSceneID = 1;
        if (SceneManager.sceneCount > 1)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).buildIndex != 0)
                {
                    currentSceneID = SceneManager.GetSceneAt(i).buildIndex;
                    break;
                }
            }
        }
        if (IsStaticScene(currentSceneID))
            SetState(new LoadStaticScene(this, currentSceneID));
        else
            SetState(new LoadIngameScene(this, currentSceneID));
    }

    public bool IsSceneLoaded(int ID)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).buildIndex == ID)
                return true;
        }
        return false;
    }

    public void FindSceneHandler(int loadedScene)
    {
        var scene = SceneManager.GetSceneByBuildIndex(loadedScene);
        var sceneObjs = scene.GetRootGameObjects();

        if (sceneObjs.Length == 0)
            throw new ArgumentException("Provided scene wasn't valid. Objects couldn't be found. Scene: " + scene.buildIndex);
        
        foreach (var obj in sceneObjs)
        {
            if (obj.TryGetComponent(out SceneHandler handler))
                sceneHandler = handler;
        }
        if (sceneHandler != null)
            sceneHandler.Init(this);
    }
    public IEnumerator LoadScene(int sceneToLoad)
    {
        State.canExit = false;
        canvasHandler.ShowLoadingScreen(true);

        var loading = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        yield return new WaitUntil(() => loading.isDone);

        SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));

        FindSceneHandler(sceneToLoad);
        
        canvasHandler.ShowLoadingScreen(false);
        State.canExit = true;
    }
    public IEnumerator UnloadScene(int sceneToUnload)
    {
        State.canExit = false;
        canvasHandler.ShowLoadingScreen(true);

        if (sceneToUnload == 1) print("Unloading main menu");
        var loading = SceneManager.UnloadSceneAsync(sceneToUnload);
        yield return new WaitUntil(() => loading.isDone);

        canvasHandler.ShowLoadingScreen(false);
        State.canExit = true;
    }
    bool IsStaticScene(int ID)
    {
        var staticScenes = Enum.GetValues(typeof(StaticScenes));

        foreach (var staticID in staticScenes)
        {
            if (ID == (int)staticID)
                return true;
        }
        return false;
    }

    public void NewGame()
    {
        TransitionTo(2);
    }

    public void RestartScene()
    {
        print("Restarting");

        TransitionTo(currentSceneID);
    }
    public void LoadNextScene()
    {
        TransitionTo(currentSceneID + 1);
    }

    public void ToMenu()
    {
        TransitionTo((int)StaticScenes.MAIN_MENU);
        mc.SetState(MusicController.NORMAL_STATE);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            canvasHandler.Hide("PauseScreen");
            isPaused = false;
            return;
        }
        Time.timeScale = 0;
        canvasHandler.Show("PauseScreen");
        isPaused = true;
    }
    void TransitionTo(int sceneID)
    {
        Time.timeScale = 1;
        if (IsStaticScene(sceneID))
            SetState(new LoadStaticScene(this, sceneID));
        else
            SetState(new LoadIngameScene(this, sceneID));
    }
    int count = 0;
    public void FinishIntro()
    {
        count++;
        if (count < 2) return;
        isIntroFinished = true;
    }
}
public enum StaticScenes
{
    GAMEPLAY,
    MAIN_MENU
}
