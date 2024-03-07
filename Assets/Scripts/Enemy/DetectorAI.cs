using UnityEngine;
using System.Collections.Generic;

public class DetectorAI : MonoBehaviour
{
    internal List<GameObject> weapons;
    [HideInInspector] public Health playerHealth;

    void OnEnable()
    {
        weapons = new List<GameObject>();
    }

    void OnDisable()
    {
        weapons = null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth playerHealth))
        {
            this.playerHealth = playerHealth;
        }
        if (collision.TryGetComponent(out Weapon weapon))
        {
            weapons.Add(weapon.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth _))
        {
            playerHealth = null;
        }
        if (weapons.Contains(collision.gameObject))
        {
            weapons.Remove(collision.gameObject);
        }
    }
}
