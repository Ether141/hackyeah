using UnityEngine;
using Text = TMPro.TextMeshProUGUI;

public class BonesManager : MonoBehaviourSingleton<BonesManager>
{
    [SerializeField] private Transform bone;
    [SerializeField] private Text text;

    private float shakeForce = 0f;

    public int BonesCount { get; private set; } = 0;

    private void Update()
    {
        shakeForce = Mathf.Lerp(shakeForce, 0f, Time.deltaTime * 5f);
        bone.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Sin(Time.time * 50f) * shakeForce * Random.Range(1f, 1.25f));
    }

    public void AddBones(int bonesCount)
    {
        BonesCount += bonesCount;
        text.text = $"{BonesCount}x";
        shakeForce = 6f;
    }
}
