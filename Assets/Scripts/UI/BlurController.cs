using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurController : MonoBehaviourSingleton<BlurController>
{
    [SerializeField] private Volume volume;

    private DepthOfField depthOfField;
    private float targetBlur = 1f;

    public bool IsBlurred => targetBlur == 100f;

    private void Start()
    {
        VolumeProfile volumeProfile = volume.profile;
        volumeProfile.TryGet(out depthOfField);
    }

    private void Update() => depthOfField.focalLength.Override(Mathf.Lerp(depthOfField.focalLength.GetValue<float>(), targetBlur, Time.deltaTime * 2f));

    public void Blur() => targetBlur = 100f;

    public void RemoveBlur() => targetBlur = 1f;
}
