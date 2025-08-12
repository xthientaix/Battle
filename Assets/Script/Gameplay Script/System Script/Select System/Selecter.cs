using UnityEngine;

public class Selecter : MonoBehaviour
{
    public Target target;
    [SerializeField] private Vector3 spacePos;

    private PlayerStateManager stateManager;

    private void Awake()
    {
        stateManager = transform.GetComponent<PlayerStateManager>();
    }

    private void OnMouseDown()
    {
        SelectSystem.instance.OnMouseDownSelect(this);
    }

    private void OnMouseUp()
    {
        SelectSystem.instance.OnMouseUpSelect();
    }

    //chọn trúng enemy
    public void TakeTarget(Target target)
    {
        // Chọn lại mục tiêu cũ (đang target) , giữ nguyên ko làm gì
        if (target == this.target)
        { return; }

        this.target = target;
        stateManager.ChangeTarget(target.transform);
        stateManager.SwitchState(stateManager.movingState);
    }

    //ko chọn enemy , lấy vị trí con trỏ
    public void TakeTarget(Vector3 position)
    {
        spacePos = position;
        target = null;
        stateManager.ChangeTarget(null);
        stateManager.locationToMove = spacePos;
        stateManager.SwitchState(stateManager.movingState);
    }
}