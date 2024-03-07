using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystemDisabler : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerEquipmentSystem equipmentSystem))
        {
            equipmentSystem.Toss();
            equipmentSystem.Freeze();
            FindObjectOfType<MusicController>().SetState(MusicController.FINISH_STATE);
        }
    }
}
