using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] protected GameObject playerSkillsUI;
    [SerializeField] protected PlayerGroup playerGroup;
    protected HeroStats heroStats;

    protected void Awake()
    {
        playerGroup = transform.GetComponentInParent<PlayerGroup>();
        heroStats = transform.GetComponent<HeroStats>();
    }

    protected void Start()
    {
        OffSkillUI();
        //playerGroup.transform.GetComponent<HeroStatsHolder>().GetSavedHeroStats(heroStats);
    }

    protected void OnMouseDown()
    {
        playerGroup.OffCurrentPlayerUI();
        playerSkillsUI.SetActive(true);
        playerGroup.CurrentPlayer = this;
    }

    public void OffSkillUI()
    {
        playerSkillsUI.SetActive(false);
    }
}
