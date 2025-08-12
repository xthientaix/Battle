using UnityEngine;

public class Target : MonoBehaviour
{
    private void OnMouseEnter()
    {
        SelectSystem.instance.MouseEnterTarget(this);
    }

    private void OnMouseExit()
    {
        SelectSystem.instance.MouseExitTarget();
    }
}