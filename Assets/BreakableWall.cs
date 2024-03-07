using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class BreakableWall : Health
{
    [SerializeField] GameObject mainSprite;
    [SerializeField] GameObject destroyedState;
    [SerializeField] float force;

    GameObject[] destroyedElements;

    AudioSource src;
    Animator animator;
    BoxCollider2D col;
    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        src = GetComponent<AudioSource>();
        destroyedElements = new GameObject[destroyedState.transform.childCount];
        for (int i = 0; i < destroyedState.transform.childCount; ++i)
        {
            destroyedElements[i] = destroyedState.transform.GetChild(i).gameObject;
        }
    }
    public override void Die()
    {
        col.enabled = false;
        destroyedState.gameObject.SetActive(true);
        mainSprite.gameObject.SetActive(false);
        foreach (var element in destroyedElements)
        {
            var dir = (element.transform.localPosition - Vector3.zero).normalized;
            element.GetComponent<Rigidbody2D>().AddForce(dir * force, ForceMode2D.Impulse);
        }
        Invoke(nameof(DestroySelf), 3f);
    }

    public override void OnDamage()
    {
        src.Play();
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
