using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviourSingleton<FadeController>
{
    private Image fade;

    private float targetAlpha = 0f;
    private float speed = 10f;

    protected override void Awake()
    {
        base.Awake();
        fade = GetComponent<Image>();
    }

    private void Update()
    {
        float a = Mathf.MoveTowards(fade.color.a, targetAlpha, Time.unscaledDeltaTime * speed);
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, a);
        fade.raycastTarget = fade.color.a <= 0.05f;
    }

    public void FadeIn(float speed)
    {
        targetAlpha = 1f;
        this.speed = speed;
    }

    public void FadeOut(float speed)
    {
        targetAlpha = 0f;
        this.speed = speed;
    }

    public void InstantFadeIn()
    {
        targetAlpha = 1f;
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
    }

    public void InstantFadeOut()
    {
        targetAlpha = 0f;
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);
    }
}
