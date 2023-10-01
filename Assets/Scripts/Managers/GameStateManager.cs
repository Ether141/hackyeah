using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [Header("Camera FOVs")]
    [SerializeField] private CameraFovs fovs;

    [Header("Starting state")]
    [SerializeField] private float startAfterTime = 1f;
    [SerializeField] private float fadeOutSpeed = 4f;
    [SerializeField] private Vector3 startingStateCameraOffset;
    [SerializeField] private GameObject startingNest;

    [Header("Mutation state")]
    [SerializeField] private GameObject mutationWindow;

    public CameraFovs CameraFovs => fovs;

    private void Start()
    {
        FadeController.Instance.InstantFadeIn();

        this.WaitAndDo(StartGame, startAfterTime);
    }

    public void StartGame()
    {
        FadeController.Instance.FadeOut(fadeOutSpeed);
        CameraController.Instance.SetTarget(startingNest.transform, startingStateCameraOffset, true);
        CameraController.Instance.ChangeFOV(fovs.StartingStateFov, 1000f);
    }

    public void EnterMutationState()
    {
        BlurController.Instance.Blur();
        this.WaitAndDo(() => mutationWindow.SetActive(true), 0.5f);
    }
}
