using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    [Header("StateMachine")]
    public float updateTime;

    protected State State;

    internal void SetState(State state)
    {
        print("AI: " +state);
        State = state;
        StartCoroutine(State.PerformState());
    }

    internal void ExitState()
    {
        StopAllCoroutines();
        State = null;
    }
}
