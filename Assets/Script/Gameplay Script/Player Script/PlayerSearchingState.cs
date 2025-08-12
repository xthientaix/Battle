using UnityEngine;

public class PlayerSearchingState : BaseState<PlayerStateManager>
{
    float time = 0;
    readonly float searchTime = 0.3f;

    /*-----------------------------------------------*/

    public override void EnterState(PlayerStateManager stateManager)
    {
        stateManager.anim.Play("Idle");

        Transform aliveEnemies = stateManager.playerGroup.enemyGroup.aliveEnemies;
        int aliveCount = aliveEnemies.childCount;

        if (aliveCount == 0)
        {
            stateManager.SwitchState(stateManager.idleState);
            return;
        }

        stateManager.ChangeTarget(aliveEnemies.GetChild(Random.Range(0, aliveCount)));
        stateManager.transform.GetComponent<Selecter>().target = stateManager.target.transform.GetComponent<Target>();
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        time += Time.deltaTime;
        if (time > searchTime)
        {
            time = 0;
            stateManager.SwitchState(stateManager.movingState);
            return;
        }
    }

    public override void ExitState(PlayerStateManager stateManager)
    {

    }
}
