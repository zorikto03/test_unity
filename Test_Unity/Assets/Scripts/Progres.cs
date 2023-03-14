using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public int Coins;
    public float RecordDistance;
    public float MaxDangerousManeuvers;
    public float MaxScore;

    public bool SetData(float distance, float dangerousManeuvers, float maxScore)
    {
        bool result = false;
        
        if (RecordDistance < distance)
        {
            RecordDistance = distance;
            result = true;
        }
        
        if (MaxDangerousManeuvers < dangerousManeuvers)
        {
            MaxDangerousManeuvers = dangerousManeuvers;
            result = true;
        }

        if (MaxScore < maxScore)
        {
            MaxScore = maxScore;
            result = true;
        }
        return result;
    }


    public override int GetHashCode()
    {
        return RecordDistance.GetHashCode() ^ MaxDangerousManeuvers.GetHashCode() ^ MaxScore.GetHashCode();
    }
}

public class Progres : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PlayerDataUI;

    public PlayerInfo PlayerInfo;
    
    public static Progres Instance;


    [DllImport("__Internal")]
    static extern void SaveExtern(string data);

    [DllImport("__Internal")]
    static extern void LoadExtern();

    [DllImport("__Internal")]
    static extern void SetLeaderBoardScore(int score);

    Yandex yandex;

    private void Start()
    {
        if (PlayerInfo == null)
            PlayerInfo = new PlayerInfo();
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            yandex = FindObjectOfType<Yandex>();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            transform.parent = null;
            DontDestroyOnLoad(this);
            Instance = this;

#if UNITY_WEBGL 
            //LoadExtern();
#endif
        }
    }

    public void Save()
    {
#if UNITY_WEBGL
        if (!string.IsNullOrEmpty(yandex?.PlayerName))
        {
            var json = JsonUtility.ToJson(PlayerInfo);
            SaveExtern(json);
            var score = Mathf.CeilToInt(PlayerInfo.MaxScore * 100);
            SetLeaderBoardScore(score);
        }
#endif
    }

    public void SetPlayerInfo(string data)
    {
        var info = JsonUtility.FromJson<PlayerInfo>(data);
        PlayerInfo.SetData(info.RecordDistance, info.MaxDangerousManeuvers, info.MaxScore);
        if (PlayerInfo.GetHashCode() != info.GetHashCode())
        {
            Save();
        }
        ShowPlayerInfo();
    }

    public void ShowPlayerInfo()
    {
        PlayerDataUI.text = $"Coins: {PlayerInfo.Coins}\nMax Distance: {PlayerInfo.RecordDistance}\nMax Dangerous Maneuvers: {PlayerInfo.MaxDangerousManeuvers}\nMax Score: {PlayerInfo.MaxScore}";
    }
}
