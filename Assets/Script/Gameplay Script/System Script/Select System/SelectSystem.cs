using UnityEngine;

public class SelectSystem : MonoBehaviour
{
    public static SelectSystem instance;

    [SerializeField] Selecter selecter;
    [SerializeField] Target target;

    bool onSelect;
    bool selecting;

    [Space(10)]
    [SerializeField] Transform selecterHighlight;
    [SerializeField] Transform targetHighlight;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] Transform limitSpace;  // Gắn trong inspector
    private float[] limitX = { 0, 0 };
    private float[] limitY = { 0, 0 };

    private bool endMatch;

    /*-----------------------------------------------------------------------*/

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("da co SelectSystem");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        limitX[0] = limitSpace.position.x - (limitSpace.localScale.x / 2);
        limitX[1] = limitSpace.position.x + (limitSpace.localScale.x / 2);
        limitY[0] = limitSpace.position.y - (limitSpace.localScale.y / 2);
        limitY[1] = limitSpace.position.y + (limitSpace.localScale.y / 2);

        endMatch = false;
        OnOffHighlight(false);
    }

    private void Update()
    {
        if (endMatch)
        { return; }

        if (selecter == null)
        {
            return;
        }
        else
        {
            selecterHighlight.position = selecter.transform.position;
        }

        if (target == null)
        {
            Vector2 mousePos = CheckLimitSpace();
            targetHighlight.position = mousePos;
        }
        else
        {
            targetHighlight.position = target.transform.position;
        }

        lineRenderer.SetPosition(0, selecterHighlight.position);
        lineRenderer.SetPosition(1, targetHighlight.position);
    }

    public void OnMouseDownSelect(Selecter selecter)
    {
        if (endMatch)
        { return; }

        if (this.selecter != null)
        { return; }

        this.selecter = selecter;
        onSelect = true;
        selecterHighlight.gameObject.SetActive(true);
        //OnOffHighlight(true);
    }

    public void OnMouseUpSelect()
    {
        if (endMatch)
        { return; }

        OnOffHighlight(false);

        if (selecting)
        {
            if (target == null)
            {
                selecter.TakeTarget(targetHighlight.transform.position);
            }
            else
            {
                selecter.TakeTarget(target);
            }
        }

        selecter = null;
        target = null;
        onSelect = false;
        selecting = false;
    }

    public void MouseEnterTarget(Target target)
    {
        if (endMatch)
        { return; }

        if (onSelect)
        { this.target = target; }
    }

    public void MouseExitTarget()
    {
        if (endMatch)
        { return; }

        if (onSelect)
        {
            selecting = true;
            targetHighlight.gameObject.SetActive(true);
            lineRenderer.gameObject.SetActive(true);
        }
        target = null;
    }

    private void OnOffHighlight(bool isOn)
    {
        selecterHighlight.gameObject.SetActive(isOn);
        targetHighlight.gameObject.SetActive(isOn);
        lineRenderer.gameObject.SetActive(isOn);
    }

    private Vector2 CheckLimitSpace()
    {
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = Mathf.Clamp(mousePos.x, limitX[0], limitX[1]);
        mousePos.y = Mathf.Clamp(mousePos.y, limitY[0], limitY[1]);

        return mousePos;
    }

    public void EndMatch()
    {
        OnOffHighlight(false);
        endMatch = true;
    }
}