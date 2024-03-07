
public class ActivatableBlock : Switcher
{
    void Awake()
    {
        gameObject.SetActive(false);
    }

    public override void Activation()
    {
        gameObject.SetActive(IsActivated);
    }
}
