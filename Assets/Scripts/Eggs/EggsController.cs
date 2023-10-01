using UnityEngine;

public class EggsController : MonoBehaviour
{
    private EggClass[] eggs;
    private GameObject currentInfo;

    private void Start()
    {
        eggs = new EggClass[transform.childCount];

        for (int i = 0; i < eggs.Length; i++)
        {
            eggs[i] = transform.GetChild(i).GetComponent<EggClass>();
        }
    }

    public void ShowInfo(GameObject info)
    {
        currentInfo?.SetActive(false);
        info.SetActive(true);
        currentInfo = info;
    }

    public void HideInfo()
    {
        currentInfo?.SetActive(false);
    }

    public void SelectClass()
    {
        HideInfo();

        foreach (var egg in eggs)
        {
            Destroy(egg);
        }

        GameStateManager.Instance.ContinueStartGame();
    }
}
