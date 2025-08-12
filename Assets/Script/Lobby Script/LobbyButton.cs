using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class LobbyButton : MonoBehaviour
{
    [SerializeField] SwapSystem swapSystem;

    [SerializeField] GameObject currentPanel;
    [SerializeField] GameObject nextPanel;

    [SerializeField] float effectTime;

    public void ClickButton()
    {
        if (swapSystem != null)
        {
            swapSystem.OffSwapSystem();
        }

        if (currentPanel != null)
        {
            _ = DoCurrentPanel();
        }

        if (nextPanel != null)
        {
            _ = DoNextPanel();
        }
    }

    private async Task DoCurrentPanel()
    {
        currentPanel.GetComponent<RectTransform>().DOScale(0.2f, effectTime);
        await currentPanel.GetComponent<CanvasGroup>().DOFade(0.2f, effectTime).AsyncWaitForCompletion();
        currentPanel.SetActive(false);
    }

    private async Task DoNextPanel()
    {
        nextPanel.SetActive(true);
        nextPanel.GetComponent<RectTransform>().DOScale(1f, effectTime);
        await nextPanel.GetComponent<CanvasGroup>().DOFade(1f, effectTime).AsyncWaitForCompletion();
    }
}
