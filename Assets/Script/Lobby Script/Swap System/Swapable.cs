using UnityEngine;
using UnityEngine.EventSystems;

public enum SwapType
{
    Hero,
    Item,
    Skill
}

public class Swapable : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] SwapSystem swapSystem;
    [SerializeField] SwapType type;

    [Space(10)]
    [SerializeField] GameObject canSwapPanel;   // item mới gắn panel này , hero lineup ko gắn
    private bool canSwap = true;

    private void Awake()
    {
        swapSystem = GameObject.FindGameObjectWithTag("SwapSystem").GetComponent<SwapSystem>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        swapSystem.SelectSwapable(gameObject, type);
    }

    public void SetCanSwap(bool canSwap)
    {
        this.canSwap = canSwap;

        if (canSwapPanel == null)
        { return; }

        canSwapPanel.SetActive(!this.canSwap);
    }
}
