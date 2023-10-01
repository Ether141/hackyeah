using UnityEngine;

public class GameStateManager : MonoBehaviourSingleton<GameStateManager>
{
    [Header("Camera FOVs")]
    [SerializeField] private CameraFovs fovs;

    [Header("Misc")]
    [SerializeField] private GameObject gameplayUI;

    [Header("Starting state")]
    [SerializeField] private float startAfterTime = 1f;
    [SerializeField] private float fadeOutSpeed = 4f;
    [SerializeField] private Vector3 startingStateCameraOffset;
    [SerializeField] private GameObject startingNest;
    [SerializeField] private Animator startingAnimator;
    [SerializeField] private GameObject boomEffect;
    [SerializeField] private GameObject player;

    [Header("Mutation state")]
    [SerializeField] private GameObject mutationWindow;

    public CameraFovs CameraFovs => fovs;

    private void Start()
    {
        FadeController.Instance.InstantFadeIn();
        gameplayUI.SetActive(false);

        this.WaitAndDo(StartGame, startAfterTime);
    }

    public void StartGame()
    {
        FadeController.Instance.FadeOut(fadeOutSpeed);
        CameraController.Instance.SetTarget(startingNest.transform, startingStateCameraOffset, true);
        CameraController.Instance.ChangeFOV(fovs.StartingStateFov, 1000f);
    }

    public void ContinueStartGame()
    {
        boomEffect.SetActive(true);

        startingAnimator.Play("Beggining");
        CameraController.Instance.SetTarget(player.transform);
        CameraController.Instance.ForceSlowFocus();
        CameraController.Instance.ChangeFOV(fovs.DefaultFov, 4f);

        this.WaitAndDo(() =>
        {
            player.GetComponent<SpriteRenderer>().enabled = true;
        }, 0.5f);

        this.WaitAndDo(() =>
        {
            player.GetComponent<PlayerController>().canMove = true;
            gameplayUI.SetActive(true);
        }, 2f);
    }

    public void EnterMutationState()
    {
        BlurController.Instance.Blur();
        this.WaitAndDo(() => mutationWindow.SetActive(true), 0.5f);
    }
}
