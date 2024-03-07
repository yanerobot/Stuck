using System.Collections;
using UnityEngine;

public class Automatic : Weapon
{
    [Header("Pistol")]
    public bool isAuto = true;

    [Header("Automatic")]
    [SerializeField] float delayBetweenShots;

    Coroutine shooting;
    public override void Use()
    {
        if (currentBullets == 0)
        {
            src.PlayOneShot(emptyMagSFX);
            return;
        }

        if (!isAuto)
        {
            SingleShot();
            return;
        }

        if (shooting != null) return;

        shooting = StartCoroutine(AutoShoot(delayBetweenShots));
    }
    public override void StopUsing()
    {
        if (shooting != null) 
            StopCoroutine(shooting);
        shooting = null;
    }

    IEnumerator AutoShoot(float delay)
    {
        while (currentBullets > 0)
        {
            SingleShot();
            yield return new WaitForSeconds(delay);
        }

        src.PlayOneShot(emptyMagSFX);
    }

    protected override void OnEquip()
    {
        if (isAuto)
            character.animator.SetBool("isHoldingRifle", true);
        else
            character.animator.SetBool("isHoldingPistol", true);
    }

    protected override void OnToss()
    {
        StopAllCoroutines();
        if (isAuto)
            character.animator.SetBool("isHoldingRifle", false);
        else
            character.animator.SetBool("isHoldingPistol", false);
    }
}
