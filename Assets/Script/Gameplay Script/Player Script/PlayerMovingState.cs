using UnityEngine;

public class PlayerMovingState : BaseState<PlayerStateManager>
{
    private Vector3 offsetMove = new(1.4f, 0, 0);
    private Vector3 pos1;
    private Vector3 pos2;

    private float moveSpeed;

    /*------------------------------------------------*/

    public override void EnterState(PlayerStateManager stateManager)
    {
        Facing(stateManager);

        // Chọn lên mục tiêu đồng minh
        if (stateManager.target != null && stateManager.target.TryGetComponent<HeroStats>(out _))
        {
            if (stateManager.heroStats.attackType == AttackType.Attacker)
            {
                stateManager.attackingState.attackType = AttackType.Attacker;
                stateManager.SwitchState(stateManager.idleState);
                return;
            }

            stateManager.attackingState.attackType = AttackType.Healer;
        }
        else
        { stateManager.attackingState.attackType = AttackType.Attacker; }

        if (stateManager.target != null)
        {
            // Mục tiêu đã chết
            if (!stateManager.target.GetComponent<IHitable>().IsAlive())
            {
                stateManager.ChangeTarget(null);
                stateManager.transform.GetComponent<Selecter>().target = null;
                stateManager.SwitchState(stateManager.searchingState);
                return;
            }
            else
            {
                UpdateLocation(stateManager);

                if (stateManager.heroStats.isRange || (Vector3.Distance(stateManager.transform.position, stateManager.locationToMove) <= 0.02f))
                {
                    stateManager.SwitchState(stateManager.attackingState);
                    return;
                }
            }
        }

        stateManager.anim.Play("Moving");

        moveSpeed = stateManager.heroStats.moveSpeed;
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        UpdateLocation(stateManager);
        Facing(stateManager);

        if (stateManager.target == null)    // Di chuyển tự do , ko có target
        {
            if (Vector3.Distance(stateManager.transform.position, stateManager.locationToMove) <= 0.02f)
            {
                stateManager.SwitchState(stateManager.idleState);
                return;
            }
        }
        else
        {
            // Mục tiêu đã chết
            if (!stateManager.target.GetComponent<IHitable>().IsAlive())
            {
                stateManager.ChangeTarget(null);
                stateManager.transform.GetComponent<Selecter>().target = null;
                stateManager.SwitchState(stateManager.searchingState);
                return;
            }
            else
            {
                if (stateManager.heroStats.isRange || (Vector3.Distance(stateManager.transform.position, stateManager.locationToMove) <= 0.02f)
                    || (Vector3.Distance(stateManager.transform.position, stateManager.target.position) < offsetMove.magnitude))
                {
                    stateManager.SwitchState(stateManager.attackingState);
                    return;
                }
            }
        }

        stateManager.transform.position = Vector3.MoveTowards(stateManager.transform.position, stateManager.locationToMove, moveSpeed * Time.deltaTime);
    }

    public override void ExitState(PlayerStateManager stateManager)
    {

    }

    private void UpdateLocation(PlayerStateManager stateManager)
    {
        if (stateManager.target == null)
        { return; }

        if (stateManager.target == stateManager.transform)
        {
            stateManager.locationToMove = stateManager.transform.position;
            return;
        }

        pos1 = stateManager.target.position - offsetMove;
        pos2 = stateManager.target.position + offsetMove;

        if (Vector3.Distance(stateManager.transform.position, pos1) < Vector3.Distance(stateManager.transform.position, pos2))
        {
            stateManager.locationToMove = pos1;
            return;
        }
        else
        {
            stateManager.locationToMove = pos2;
            return;
        }
    }

    private void Facing(PlayerStateManager stateManager)
    {
        // Xoay mặt nhân vật về hướng mục tiêu
        if (stateManager.locationToMove.x < stateManager.transform.position.x)
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
