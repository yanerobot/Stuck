using UnityEngine;

public class GravityFlip : Switcher
{
    void OnEnable()
    {
        Physics2D.gravity = new Vector2(0, -9.81f);
    }
    public delegate void Action();
    public event Action OnGravityChange;
    public override void Activation()
    {
        Physics2D.gravity = -Physics2D.gravity;
        OnGravityChange?.Invoke();
    }
}
