public class EnemyIdleState : BaseState<EnemyStateManager>
{
    public bool noPlayer;
    float time;

    /*------------------------------------------------------------*/

    public override void EnterState(EnemyStateManager stateManager)
    {
        stateManager.anim.Play("Idle");
    }

    public override void ExitState(EnemyStateManager stateManager)
    {

    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        /*if (noPlayer)
        { return; }

        time += Time.deltaTime;
        if (time > 1f)
        {
            time = 0;
            //stateManager.SwitchState(stateManager.searchingState);
        }*/
    }
}