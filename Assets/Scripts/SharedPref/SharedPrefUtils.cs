using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SharedPrefUtils : MonoBehaviour {

    public const string PRODUCTION_LEVEL = "ProductionLevel";

    public static void SetProductionData(string userData)
    {
        PlayerPrefs.SetString(PRODUCTION_LEVEL, userData);
    }

    public static ProductionLevelData GetProductionData()
    {
        ProductionLevelData user = null;
        if (PlayerPrefs.HasKey(PRODUCTION_LEVEL))
        {
            user = JsonUtility.FromJson<ProductionLevelData>(PlayerPrefs.GetString(PRODUCTION_LEVEL));
        }

        return user;
    }

    public const string PAUSE_TIME = "PauseTime";

    public static void SetPauseTime(string userData)
    {
        PlayerPrefs.SetString(PAUSE_TIME, userData);
    }

    public static string GetPauseTime()
    {
        string user = null;
        if (PlayerPrefs.HasKey(PAUSE_TIME))
        {
            user = PlayerPrefs.GetString(PAUSE_TIME);
        }

        return user;
    }

    public const string GAME_VALUES = "GameValues";

    public static void SetGameValues(string userData)
    {
        PlayerPrefs.SetString(GAME_VALUES, userData);
    }

    public static GameValues GetGameValues()
    {
        GameValues user = null;
        if (PlayerPrefs.HasKey(GAME_VALUES))
        {
            user = JsonUtility.FromJson<GameValues>(PlayerPrefs.GetString(GAME_VALUES));
        }

        return user;
    }


    public const string SHOWN_HINTS = "shownHints";

    public static void SetHintShow()
    {
        PlayerPrefs.SetString(SHOWN_HINTS, "1");
    }

    public static bool GetHintShown()
    {
        if (PlayerPrefs.HasKey(SHOWN_HINTS))
        {
            return true;
        }

        return false;
    }

}
