using UnityEngine;

public class InvisibleWall : Switcher
{
    [SerializeField] float timeToWait = 0.3f;

    SpriteRenderer sr;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        Activate(false); //invisibility
    }

    public override void Activation()
    {
        if (timeToWait == 0)
            ToggleWall();
        else
            Invoke(nameof(ToggleWall), 0.3f);
    }

    void ToggleWall()
    {
        sr.enabled = !IsActivated;
    }
}
