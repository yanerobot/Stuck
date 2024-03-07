using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Weapon : Item
{
    [Header("Stats")]
    [SerializeField] protected int bullets;
    [SerializeField] float maxAimAngle;
    [SerializeField] int damage;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletGravityScale;


    protected int currentBullets;

    [Header("Recoil")]
    [SerializeField] float recoilForce = 10f;
    [SerializeField] float recoilCooldown = 0.8f;
    [SerializeField] float maxRecoilAngle;
    [SerializeField] float recolilShakiness;

    [Header("References")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootingPoint;
    [SerializeField] ParticleSystem shootEffect;

    [Header("Audio")]
    [SerializeField] protected AudioClip shootSFX;
    [SerializeField] protected AudioClip emptyMagSFX;
    [SerializeField] float pitchRandomness;


    protected AudioSource src;
    SpriteRenderer GFX;
    Coroutine recoilRoutine;

    public int CurrentBullets => currentBullets;

    protected override void Awake()
    {
        base.Awake();
        GFX = GetComponentInChildren<SpriteRenderer>();
        src = GetComponent<AudioSource>();
        currentBullets = bullets;
    }

    protected void SingleShot()
    {
        var bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation, null);
        var bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.speed = bulletSpeed;
        bulletComponent.gravityScale = bulletGravityScale;
        bulletComponent.damage = damage;
        if (recoilRoutine != null) StopCoroutine(recoilRoutine);
        recoilRoutine = StartCoroutine(Recoil());
        currentBullets -= 1;
        PlayShootEffects();
    }

    protected virtual void PlayShootEffects()
    {
        shootEffect.Play();
        var initialPitch = src.pitch;
        src.pitch = src.pitch + Random.Range(-pitchRandomness, pitchRandomness);
        src.PlayOneShot(shootSFX);
        src.pitch = initialPitch;
    }

    IEnumerator Recoil()
    {
        var clampedZ = Mathf.Clamp(GFX.transform.localEulerAngles.z + recoilForce, -maxRecoilAngle + Random.Range(-5, 5), maxRecoilAngle + Random.Range(-5, 5));
        GFX.transform.localEulerAngles = GFX.transform.localEulerAngles.WhereY(Random.Range(-recolilShakiness, recolilShakiness)).WhereZ(clampedZ);

        var newRotation = GFX.transform.localRotation;

        var time = 0f;
        while (time <= recoilCooldown)
        {
            GFX.transform.localRotation = Quaternion.Slerp(newRotation, Quaternion.identity, time/recoilCooldown);
            time += Time.deltaTime;
            yield return null;
        }
        GFX.transform.localRotation = Quaternion.identity;
    }

    public override void Aim(Vector2 aimDirection)
    {
        var angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        if (Physics2D.gravity.y > 0) angle = -angle;
        var clampedAngle = Mathf.Clamp(angle, -maxAimAngle, maxAimAngle);

        transform.localEulerAngles = new Vector3(0, 0, clampedAngle);
        if (character == null) print("char");
        if (character.animator == null) print("animator");
        character.animator.SetFloat("AimY", clampedAngle);
    }
    protected override void OnToss()
    {
        StopAllCoroutines();
    }
}
