using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePiece : Health
{
    public override void Die()
    {
        Destroy(gameObject);
    }

    public override void OnDamage()
    {
        
    }
}
