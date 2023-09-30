using UnityEngine;

public class CameraController : MonoBehaviourSingleton<CameraController>
{
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private float targetRequiredShift = 0.5f;
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float fovChangeSpeed = 10f;

    public Vector3 TargetPosition { get; private set; }
    public Vector3 CustomOffset { get; set; }
    public Camera Cam { get; private set; }

    private Vector2 TargetShift => new Vector2(Mathf.Abs(target.position.x - transform.position.x), Mathf.Abs(target.position.y - (transform.position.y - offset.y)));
    public float StartFov { get; private set; }
    public float CurrentFov => Cam.orthographicSize;

    private Vector3 shakeOffset;
    private float shakeForce = 0f;
    private float targetFov;
    private const float shakeAmplitude = 10f;
    private const float fixedZ = -10f;

    protected override void Awake()
    {
        base.Awake();
        Cam = GetComponent<Camera>();
        StartFov = targetFov = Cam.orthographicSize;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Shake(Random.Range(0.5f, 1.25f));

        Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, targetFov, Time.deltaTime * fovChangeSpeed);

        shakeForce = Mathf.Lerp(shakeForce, 0f, Time.deltaTime * 5f);
        shakeOffset = new Vector3(Mathf.Sin(Time.time * shakeAmplitude) * shakeForce * Random.Range(1f, 1.25f), Mathf.Sin(Time.time * shakeAmplitude) * shakeForce * Random.Range(1f, 1.25f), 0f);
    }

    public void Shake(float force)
    {
        shakeForce = force;
    }

    private void FixedUpdate()
    {
        if (TargetShift.x > targetRequiredShift || TargetShift.y > targetRequiredShift)
            TargetPosition = new Vector3(target.position.x, target.position.y, fixedZ) + offset + CustomOffset;

        TargetPosition += shakeOffset;
        transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * speed);
    }

    public void ChangeFOV(float fov, float speed)
    {
        targetFov = fov;
        fovChangeSpeed = speed;
    }

    public void ResetFOV(float speed)
    {
        targetFov = StartFov;
        fovChangeSpeed = speed;
    }

    public void SetTarget(Transform newTarget, Vector3? offset = null, bool forceFocus = false)
    {
        target = newTarget;
        CustomOffset = offset.GetValueOrDefault(Vector3.zero);

        if (forceFocus)
        {
            ForceFocus();
        }
    }

    public void ForceFocus() => transform.position = TargetPosition = new Vector3(target.position.x, target.position.y, fixedZ) + offset + CustomOffset;

    public void ForceSlowFocus() => TargetPosition = new Vector3(target.position.x, target.position.y, fixedZ) + offset + CustomOffset;
}