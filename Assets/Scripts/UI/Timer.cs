using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float time = 240f;
    [SerializeField] private Image fill1;
    [SerializeField] private Image fill2;
    [SerializeField] private Text text;

    private float timer;

    private void Start()
    {
        timer = time;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        text.text = Mathf.RoundToInt(timer).ToString();

        fill1.fillAmount = fill2.fillAmount = timer / time;

        if (timer <= 0f)
        {
            timer = 0f;
            Destroy(this);
        }
    }
}
