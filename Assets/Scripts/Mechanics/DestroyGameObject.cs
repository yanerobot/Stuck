using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : Switcher
{
    public override void Activation()
    {
        Destroy(gameObject);
    }
}
