using System.IO;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    private static string GetPath()
    {
#if UNITY_ANDROID || UNITY_IOS
        return Application.persistentDataPath;
#else
        return Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
#endif
    }

    public static string GetSavePath(string typeString)
    {
        string gameFolder = GetPath();
        string saveFolder = Path.Combine(gameFolder, "Save");

        Directory.CreateDirectory(saveFolder);
        return Path.Combine(saveFolder, typeString + ".json");
    }

    public static void LoadHeroStats(HeroStats hero)
    {
        string typeString = hero.type.ToString();
        string pathString = GetSavePath(typeString);    // đường dẫn file save

        if (File.Exists(pathString))
        {
            // đọc và lấy thông tin save từ đường dẫn
            string jsonString = File.ReadAllText(pathString);
            JsonUtility.FromJsonOverwrite(jsonString, hero);
        }
        else
        {
            // chưa có save , tạo mới hero với thông số default rồi save lại
            string jsonString = JsonUtility.ToJson(HeroStatsHolder.instance.GetDefaultStats(hero.type));
            JsonUtility.FromJsonOverwrite(jsonString, hero);
            hero.CalculateLevel();
            jsonString = JsonUtility.ToJson(hero, true);
            File.WriteAllText(pathString, jsonString);
        }
    }

    public static void LoadHeroStats(MiniHeroStats hero)
    {
        string typeString = hero.type.ToString();
        string pathString = GetSavePath(typeString);    //đường dẫn file save

        //  có save
        if (File.Exists(pathString))
        {
            //  đọc và lấy thông tin save từ đường dẫn
            string jsonString = File.ReadAllText(pathString);
            JsonUtility.FromJsonOverwrite(jsonString, hero);
        }
        else
        {
            //  chưa có save , tạo save mới với thông số default
            string jsonString = JsonUtility.ToJson(HeroStatsHolder.instance.GetDefaultStats(hero.type));
            JsonUtility.FromJsonOverwrite(jsonString, hero);
            hero.CalculateLevel();
            jsonString = JsonUtility.ToJson(hero, true);
            File.WriteAllText(pathString, jsonString);
        }
    }

    public static void SaveHeroStats(HeroStats hero)
    {
        string typeString = hero.type.ToString();
        string pathString = GetSavePath(typeString);

        string jsonString = JsonUtility.ToJson(hero, true);
        File.WriteAllText(pathString, jsonString);
    }

    public static void SaveHeroStats(MiniHeroStats hero)
    {
        string typeString = hero.type.ToString();
        string pathString = GetSavePath(typeString);

        string jsonString = JsonUtility.ToJson(hero, true);
        File.WriteAllText(pathString, jsonString);
    }
}
