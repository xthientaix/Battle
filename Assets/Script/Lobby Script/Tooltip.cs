using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void ShowContent(string content)
    {
        text.text = content;
    }

    public void EraseTooltip()
    {
        text.text = "";
    }
}
