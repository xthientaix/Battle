using UnityEngine;

public class EnemySearchingState : BaseState<EnemyStateManager>
{
    float time = 0;
    readonly float searchTime = 0.5f;

    /*-------------------------------------------------------*/

    public override void EnterState(EnemyStateManager stateManager)
    {
        stateManager.anim.Play("Idle");

        Transform aliveHeros = stateManager.enemyGroup.playerGroup.aliveHeros;
        int aliveCount = aliveHeros.childCount;

        if (aliveCount == 0)
        {
            //stateManager.idleState.noPlayer = true;
            stateManager.SwitchState(stateManager.idleState);
            return;
        }

        //stateManager.idleState.noPlayer = false;
        stateManager.target = aliveHeros.GetChild(Random.Range(0, aliveCount));
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        time += Time.deltaTime;
        if (time > searchTime)
        {
            time = 0;
            stateManager.SwitchState(stateManager.movingState);
        }
    }

    public override void ExitState(EnemyStateManager stateManager)
    {

    }
}
