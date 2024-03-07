using UnityEngine;

public class GravityActivator : MonoBehaviour
{
    [SerializeField] Switcher[] swichers;
    GravityFlip gf;
    void Awake()
    {
        gf = GetComponent<GravityFlip>();
        gf.OnGravityChange += () => Activate();
    }

    void Activate()
    {
        foreach(var sw in swichers)
        {
            sw?.Activate();
        }
    }
}
