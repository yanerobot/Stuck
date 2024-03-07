using UnityEngine;

public class WeaponMagazine : MonoBehaviour
{
    [SerializeField] Transform magazine;
    Transform currentMagazine;

    void Awake()
    {
        currentMagazine = magazine;
    }

    internal void UnAttach()
    {
        currentMagazine.SetParent(null);
    }

    internal void Attach(Transform magazine)
    {
        magazine.SetParent(transform);
        currentMagazine = magazine;
    }
}
