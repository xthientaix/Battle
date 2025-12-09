using UnityEngine;

public class EnemyMovingState : BaseState<EnemyStateManager>
{
    private Vector3 offsetMove = new(1.7f, 0, 0);
    private Vector3 pos1;
    private Vector3 pos2;

    private float moveSpeed;

    public bool isFirstLocation;

    /*------------------------------------------------*/

    public override void EnterState(EnemyStateManager stateManager)
    {
        Facing(stateManager);

        if (stateManager.target != null)
        {
            // Mục tiêu đã chết
            if (!stateManager.target.GetComponent<IHitable>().IsAlive())
            {
                stateManager.target = null;
                stateManager.SwitchState(stateManager.searchingState);
                return;
            }
            else
            {
                UpdateLocation(stateManager);

                if (stateManager.enemyStats.isRange || (Vector3.Distance(stateManager.transform.position, stateManager.locationToMove) <= 0.03f))
                {
                    stateManager.SwitchState(stateManager.attackingState);
                    return;
                }
            }
        }
        stateManager.anim.Play("Moving");
        //stateManager.PlaySoundEffect("move");
        moveSpeed = stateManager.enemyStats.moveSpeed;
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        UpdateLocation(stateManager);
        Facing(stateManager);

        if (stateManager.target == null)    //di chuyển tự do
        {
            if (Vector3.Distance(stateManager.transform.position, stateManager.locationToMove) <= 0.03f)
            {
                if (isFirstLocation)
                {
                    stateManager.enemyGroup.AfterFirstLocation(stateManager.enemyStats);
                    isFirstLocation = false;
                }
                stateManager.SwitchState(stateManager.searchingState);
                return;
            }
        }
        else
        {
            if (!stateManager.target.GetComponent<IHitable>().IsAlive())
            {
                stateManager.target = null;
                stateManager.SwitchState(stateManager.searchingState);
                return;
            }
            else
            {
                if (stateManager.enemyStats.isRange || (Vector3.Distance(stateManager.transform.position, stateManager.locationToMove) <= 0.03f)
                    || (Vector3.Distance(stateManager.transform.position, stateManager.target.position) < offsetMove.magnitude))
                {
                    stateManager.SwitchState(stateManager.attackingState);
                    return;
                }
            }
        }

        stateManager.transform.position = Vector3.MoveTowards(stateManager.transform.position, stateManager.locationToMove, moveSpeed * Time.deltaTime);
        stateManager.sortingGroup.sortingOrder = -(int)(stateManager.transform.position.y * 100);
    }

    public override void ExitState(EnemyStateManager stateManager)
    {

    }

    private void UpdateLocation(EnemyStateManager stateManager)
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

    private void Facing(EnemyStateManager stateManager)
    {
        // Xoay mặt nhân vật về hướng mục tiêu
        if (stateManager.locationToMove.x < stateManager.transform.position.x)
        {
            stateManager.transform.rotation = Quaternion.Euler(0, 0, 0);
            stateManager.HPBar.transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            stateManager.transform.rotation = Quaternion.Euler(0, 180, 0);
            stateManager.HPBar.transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
