using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Progres : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PlayerDataUI;
    [SerializeField] GameObject loginPanel;   

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
        if (level == 0)
        {
            SetDataLocal();
            ShowPlayerInfo();
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
            SetDataLocal();
            ShowPlayerInfo();
            ShowLoginQuestion();
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

    void SetDataLocal()
    {
        var load = PlayerInfo.LocalLoad();
        if (load.RecordDistance != 0)
        {
            PlayerInfo = load;
        }
    }

    void ShowLoginQuestion()
    {
        if (PlayerInfo.RecordDistance != 0)
        {
            loginPanel.SetActive(true);
        }
    }

    public void ShowPlayerInfo()
    {
        PlayerDataUI.text = $"Coins: {PlayerInfo.Coins}\nMax Distance: {PlayerInfo.RecordDistance}\nMax Dangerous Maneuvers: {PlayerInfo.MaxDangerousManeuvers}\nMax Score: {PlayerInfo.MaxScore}";
    }

    public void Login()
    {
        yandex.LoginButton();
    }

    public void DisableLoginPanel()
    {
        loginPanel.SetActive(false);
    }
}
