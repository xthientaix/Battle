using UnityEngine;

public class EnemyUsingSkillState : BaseState<EnemyStateManager>
{
    public bool isChanneling;
    public BaseState<EnemyStateManager> previousState;
    private readonly int changeTargetPercent = 70;

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
            // Có tỉ lệ thay đổi mục tiêu tấn công sau khi sử dụng kỹ năng (chuyển sang trạng thái searhchingState)
            if (Random.Range(0, 100) < changeTargetPercent)
            {
                stateManager.target = null;
                stateManager.SwitchState(stateManager.searchingState);
                return;
            }

            stateManager.SwitchState(previousState);
        }
    }

    public override void ExitState(EnemyStateManager stateManager)
    {

    }
}
