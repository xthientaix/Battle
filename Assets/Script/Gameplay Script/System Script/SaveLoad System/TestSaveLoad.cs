using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class TestSaveLoad : MonoBehaviour
{
    public HeroStats heroStats;

    [SerializeField] Transform obj;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            string jsonString = JsonUtility.ToJson(heroStats, true);
            File.WriteAllText(Application.dataPath + "/Save/TestSave.json", jsonString);
            //Debug.Log(jsonString);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!File.Exists(Application.dataPath + "/Save/TestSave.json"))
            {
                Debug.Log("No Save");
                return;
            }

            string typeString = "TestSave";
            string jsonString = File.ReadAllText(Application.dataPath + "/Save/" + typeString + ".json");
            //EnemyStats info = JsonUtility.FromJson<EnemyStats>(jsonString);
            if (obj.TryGetComponent(out MiniHeroStats miniStats))
            {
                Debug.Log("Already Stats");
                JsonUtility.FromJsonOverwrite(jsonString, miniStats);
            }
            else
            {
                Debug.Log("Creat Stats");
                JsonUtility.FromJsonOverwrite(jsonString, obj.AddComponent<MiniHeroStats>());
            }
        }
    }
}