using UnityEngine;

public class PlayerAttackingState : BaseState<PlayerStateManager>
{
    public AttackType attackType;
    private float attackInterval;

    /*--------------------------------------------------------------*/

    public override void EnterState(PlayerStateManager stateManager)
    {
        attackInterval = (float)(stateManager.heroStats.CurrentAttackSpeed) / 100;
        stateManager.anim.SetFloat("AttackSpeed", attackInterval);

        Facing(stateManager);
        stateManager.anim.Play("Attacking");
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        Facing(stateManager);

        if (!stateManager.target.GetComponent<IHitable>().IsAlive())
        {
            stateManager.ChangeTarget(null);
            stateManager.transform.GetComponent<Selecter>().target = null;
            stateManager.SwitchState(stateManager.searchingState);
            return;
        }

        if (Vector3.Distance(stateManager.transform.position, stateManager.target.position) > 1.8f && !stateManager.heroStats.isRange)
        {
            stateManager.SwitchState(stateManager.movingState);
            return;
        }
    }

    public override void ExitState(PlayerStateManager stateManager)
    {

    }

    private void Facing(PlayerStateManager stateManager)
    {
        if (stateManager.target == null)
        { return; }

        // Xoay mặt nhân vật về hướng mục tiêu
        if (stateManager.target.position.x < stateManager.transform.position.x)
        {
            stateManager.transform.rotation = Quaternion.Euler(0, 180, 0);
            stateManager.HPBar.transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            stateManager.transform.rotation = Quaternion.Euler(0, 0, 0);
            stateManager.HPBar.transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
