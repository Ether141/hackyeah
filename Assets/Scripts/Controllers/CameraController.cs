using UnityEngine;

public class CameraContoller : MonoBehaviourSingleton<CameraContoller>
{
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private float targetRequiredShift = 0.5f;
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float fovChangeSpeed = 10f;

    public Vector3 TargetPosition { get; private set; }
    public Camera Cam { get; private set; }

    private Vector2 TargetShift => new Vector2(Mathf.Abs(target.position.x - transform.position.x), Mathf.Abs(target.position.y - (transform.position.y - offset.y)));
    public float StartFov { get; private set; }

    private float targetFov;
    private const float fixedZ = -10f;
    private Vector3 customOffset;

    protected override void Awake()
    {
        base.Awake();
        Cam = GetComponent<Camera>();
        StartFov = targetFov = Cam.orthographicSize;
    }

    private void Update()
    {
        Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, targetFov, Time.deltaTime * fovChangeSpeed);
    }

    private void FixedUpdate()
    {
        if (TargetShift.x > targetRequiredShift || TargetShift.y > targetRequiredShift)
            TargetPosition = new Vector3(target.position.x, target.position.y, fixedZ) + offset + customOffset;

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
        customOffset = offset.GetValueOrDefault(Vector3.zero);

        if (forceFocus)
        {
            ForceFocus();
        }
    }

    public void ForceFocus() => transform.position = TargetPosition = new Vector3(target.position.x, target.position.y, fixedZ) + offset + customOffset;
}