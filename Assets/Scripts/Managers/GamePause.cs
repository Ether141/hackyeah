using UnityEngine;

public class GamePause : MonoBehaviourSingleton<GamePause>
{
    public bool IsPaused { get; private set; } = false;

    public void Pause()
    {
        if (IsPaused)
        {
            return;
        }

        IsPaused = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (!IsPaused)
        {
            return;
        }

        IsPaused = false;
        Time.timeScale = 1f;
    }
}
