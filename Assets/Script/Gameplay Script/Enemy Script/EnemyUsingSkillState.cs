public class EnemyUsingSkillState : BaseState<EnemyStateManager>
{
    public bool isChanneling;
    public BaseState<EnemyStateManager> previousState;

    /*---------------------------------------------------*/

    public override void EnterState(EnemyStateManager stateManager)
    {
        //Kiểm tra Enemy Object có đang disable không
        if (!stateManager.gameObject.activeInHierarchy)
        { return; }

        isChanneling = true;
        stateManager.anim.Play("UsingSkill");
    }

    public override void UpdateState(EnemyStateManager stateManager)
    {
        if (!isChanneling)
        {
            stateManager.SwitchState(previousState);
        }
    }

    public override void ExitState(EnemyStateManager stateManager)
    {

    }
}
