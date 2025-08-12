using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject changeScenePanel;
    [SerializeField] private GameObject storeAndStats;
    [SerializeField] private GameObject Heroes;

    /*--------------------------------------------------*/

    void Start()
    {
        Time.timeScale = 1;

        storeAndStats.SetActive(false);
        Heroes.SetActive(false);
        storeAndStats.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        Heroes.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        changeScenePanel.SetActive(true);
        _ = Fade(false);
    }

    private async Task Fade(bool isIn)
    {
        if (isIn)
        {
            changeScenePanel.SetActive(true);
            await changeScenePanel.GetComponent<Image>().DOFade(1f, 0.3f).AsyncWaitForCompletion();
            return;
        }

        await changeScenePanel.GetComponent<Image>().DOFade(0f, 0.3f).AsyncWaitForCompletion();
        changeScenePanel.SetActive(false);
    }

    private async Task StartGame(int sceneIndex)
    {
        await Fade(true);
        SceneManager.LoadScene(sceneIndex);
    }

    public void StartButton(int sceneIndex)
    {
        _ = StartGame(sceneIndex);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
