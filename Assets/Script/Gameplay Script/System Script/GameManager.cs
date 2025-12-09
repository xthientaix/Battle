using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool isMatchCompleted = false;

    [SerializeField] private TextMeshProUGUI endText;
    [SerializeField] private string victoryText;
    [SerializeField] private string defeatedText;

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject completePanel;
    [SerializeField] private GameObject changeScenePanel;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseButton;

    [Header("-----BackGround-----")]
    [SerializeField] private SpriteRenderer backGround;
    [SerializeField] private List<Sprite> backGroundSprite;

    [Header("-----Sound-----")]
    [SerializeField] private AudioSource backgroundAudio;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip loseSound;

    /*--------------------------------------------------*/

    private void Start()
    {
        changeScenePanel.SetActive(true);
        panel.SetActive(false);
        completePanel.SetActive(false);
        pausePanel.SetActive(false);
        completePanel.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 1);
        pausePanel.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 1);

        backGround.sprite = backGroundSprite[Random.Range(0, backGroundSprite.Count)];
        backgroundAudio.loop = true;

        _ = Fade(false);
    }

    public void EndGame(bool isWin)
    {
        backgroundAudio.loop = false;

        if (isWin)
        {
            endText.text = victoryText;
            backgroundAudio.clip = victorySound;
        }
        else
        {
            endText.text = defeatedText;
            backgroundAudio.clip = loseSound;
        }

        pauseButton.SetActive(false);
        Invoke(nameof(ShowPanel), 1f);
        isMatchCompleted = true;
        GameObject.FindGameObjectWithTag("PlayerGroup").GetComponent<PlayerGroup>().SaveHeros();
    }

    private async Task ShowPanel()
    {
        backgroundAudio.Play();
        panel.SetActive(true);
        await panel.GetComponent<Image>().DOFade(0.3f, 0.2f).AsyncWaitForCompletion();
        completePanel.SetActive(true);
        completePanel.GetComponent<RectTransform>().DOScale(1f, 0.3f);

        SelectSystem.instance.EndMatch();
    }

    private async Task ExitToMenu(int sceneIndex)
    {
        await Fade(true);
        SceneManager.LoadScene(sceneIndex);
    }

    public void PauseGame(bool isPause)
    {
        if (isPause)
        {
            Time.timeScale = 0;
            pauseButton.SetActive(!isPause);
            panel.SetActive(isPause);
            pausePanel.SetActive(isPause);
            pausePanel.GetComponent<RectTransform>().DOScale(1f, 0.3f).SetEase(Ease.InOutCubic).SetUpdate(true);
        }
        else
        {
            pausePanel.GetComponent<RectTransform>().DOScale(0.1f, 0.3f).SetEase(Ease.InOutCubic).SetUpdate(true)
                .OnComplete(() =>
                {
                    pausePanel.SetActive(isPause);
                    panel.SetActive(isPause);
                    pauseButton.SetActive(!isPause);
                    Time.timeScale = 1;
                });
        }

    }

    private async Task Fade(bool isIn)
    {
        if (isIn)
        {
            changeScenePanel.SetActive(true);
            await changeScenePanel.GetComponent<Image>().DOFade(1f, 0.7f).SetEase(Ease.OutCubic).SetUpdate(true).AsyncWaitForCompletion();
            return;
        }

        await changeScenePanel.GetComponent<Image>().DOFade(0f, 1f).SetEase(Ease.OutCubic).SetUpdate(true).AsyncWaitForCompletion();
        changeScenePanel.SetActive(false);
    }

    public void MenuButton(int sceneIndex)
    {
        _ = ExitToMenu(sceneIndex);
    }
}
