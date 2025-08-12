public class PlayerUsingSkillState : BaseState<PlayerStateManager>
{
    public bool isChanneling;
    public BaseState<PlayerStateManager> previousState;

    /*---------------------------------------------------*/

    public override void EnterState(PlayerStateManager stateManager)
    {
        isChanneling = true;
        stateManager.anim.Play("UsingSkill");
    }

    public override void UpdateState(PlayerStateManager stateManager)
    {
        if (!isChanneling)
        {
            stateManager.SwitchState(previousState);
        }
    }

    public override void ExitState(PlayerStateManager stateManager)
    {

    }
}
