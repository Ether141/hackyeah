using UnityEngine;

public class EggClass : MonoBehaviour
{
    [SerializeField] private EggsController controller;
    [SerializeField] private Sprite[] eggSprites;
    [SerializeField] private GameObject infoUI;

    private SpriteRenderer spriteRenderer;
    private float shakeForce = 0f;

    public int RequiredClicks => eggSprites.Length;

    private int clicksCount = 0;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        shakeForce = Mathf.Lerp(shakeForce, 0f, Time.deltaTime * 5f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Sin(Time.time * 45f) * shakeForce * Random.Range(1f, 1.25f));
    }

    private void ClickEgg()
    {
        spriteRenderer.sprite = eggSprites[clicksCount];
        clicksCount++;

        controller.RemoveInfo();
        CameraController.Instance.ChangeFOV(CameraController.Instance.CurrentFov - 0.25f, 4f);
        CameraController.Instance.CustomOffset += Vector3.down * 0.3f;
        CameraController.Instance.ForceSlowFocus();

        if (clicksCount == RequiredClicks)
        {
            Destroy(gameObject);
            controller.SelectClass();
        }
    }

    private void OnMouseDown()
    {
        ClickEgg();
        shakeForce = 3.5f;
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
