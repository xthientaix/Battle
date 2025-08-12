using System.Collections.Generic;
using UnityEngine;

public class AllSkill : MonoBehaviour
{
    /*-----------------------Class Singleton-------------------------*/


    public static AllSkill Instance;


    /*---------------------------------------------------------------*/


    [Header("Gắn các skill ScriptableObject vào đây")]
    public List<SkillData> skillList;

    private Dictionary<int, SkillData> skillDict;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Nếu đã có instance khác, hủy cái mới
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // (Tùy chọn) Giữ lại khi chuyển scene

        Initialize(); // Khởi tạo Dictionary ngay lập tức
    }


    public void Initialize()
    {
        skillDict = new Dictionary<int, SkillData>();

        foreach (var skill in skillList)
        {
            if (skill == null)
            {
                Debug.LogWarning("Skill null trong danh sách!");
                continue;
            }

            if (skillDict.ContainsKey(skill.id))
            {
                Debug.LogError($"Trùng ID skill: {skill.id} ({skill.skillName})");
                continue;
            }

            skillDict.Add(skill.id, skill);
        }
    }

    public SkillData GetSkill(int id)
    {
        return skillDict.TryGetValue(id, out SkillData skill) ? skill : null;
    }
}
