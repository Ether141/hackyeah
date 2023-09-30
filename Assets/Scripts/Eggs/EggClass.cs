using UnityEngine;

public class EggClass : MonoBehaviour
{
    [SerializeField] private EggsController controller;
    [SerializeField] private Sprite[] eggSprites;
    [SerializeField] private GameObject infoUI;

    private SpriteRenderer spriteRenderer;

    public int RequiredClicks => eggSprites.Length;

    private int clicksCount = 0;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void ClickEgg()
    {
        spriteRenderer.sprite = eggSprites[clicksCount];
        clicksCount++;

        print($"{clicksCount} / {RequiredClicks}");

        if (clicksCount == RequiredClicks)
        {
            Destroy(gameObject);
            controller.SelectClass();
        }
    }

    private void OnMouseDown()
    {
        ClickEgg();
    }

    private void OnMouseEnter()
    {
        controller.ShowInfo(infoUI);
    }

    private void OnMouseExit()
    {
        controller.HideInfo();
    }
}
