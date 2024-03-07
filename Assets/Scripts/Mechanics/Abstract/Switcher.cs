using UnityEngine;

public abstract class Switcher : MonoBehaviour
{
    bool isActivated;
    public bool IsActivated
    {
        get { return isActivated; }
    }
    public void Activate()
    {
        isActivated = !isActivated;
        Activation();
    }

    public void Activate(bool value)
    {
        isActivated = value;
        Activation();
    }

    public abstract void Activation();
}
