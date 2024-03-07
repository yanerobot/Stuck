using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    public const string 
        NORMAL_STATE = "Normal", 
        ACTION_STATE = "Action", 
        FINISH_STATE = "Finish";

    [SerializeField] AudioClip actionClip, mainMusicClip;

    AudioSource src;

    public string currentState;

    void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SetState(NORMAL_STATE);

    }

    public void SetState(string state)
    {
        currentState = state;

        switch (currentState)
        {
            case FINISH_STATE:
                src.Stop();
                return;
            case ACTION_STATE:
                src.clip = actionClip;
                break;
            case NORMAL_STATE:
                src.clip = mainMusicClip;
                break;
        }
        src.Play();
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            src.pitch = 0.93f;
            src.volume = 0.21f;
        }
        else
        {
            src.pitch = 1;
            src.volume = 0.326f;
        }
    }
}
