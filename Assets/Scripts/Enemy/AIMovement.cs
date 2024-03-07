using UnityEngine;

public class AIMovement : RigidbodyMovement2D
{
    [Header("AI")]
    public float minXVelocity;

    [HideInInspector] public float inputX;
    float inputXCache;
    public override void Freeze()
    {
        inputXCache = inputX;
        inputX = 0;
    }

    public override void UnFreeze()
    {
        inputX = inputXCache;
    }

    protected override float GetHorizontalInput()
    {
        return inputX;
    }

    internal void LookLeft(bool flag)
    {
        if (inputX > 0 && flag) inputX = -1;
    }
}
