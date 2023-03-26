using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Yandex : MonoBehaviour
{
    [SerializeField] GameObject loginPanel;

    public static Action RewardEvent;
    public static Action GameInstanceLoadedEvent;
    public delegate void GamePause(bool value);
    public static GamePause GamePauseEvent;

    public delegate void ActivateRateButton(bool value);
    public static ActivateRateButton ActivateRateButtonEvent;


    [DllImport("__Internal")]
    private static extern void Login();
    [DllImport("__Internal")]
    private static extern void CheckLogin();

    [DllImport("__Internal")]
    private static extern void GetPlayerData();

    [DllImport("__Internal")]
    private static extern void RateGame();

    [DllImport("__Internal")]
    private static extern void CheckRateGame();

    [DllImport("__Internal")]
    private static extern void ShowFullScreenAdv();

    [DllImport("__Internal")]
    private static extern void ShowRewardedVideo();

    [DllImport("__Internal")]
    private static extern void GetLeaderboard();

    [DllImport("__Internal")]
    private static extern void Logging(string text);

    [DllImport("__Internal")]
    private static extern void SaveExtern(string data);

    [DllImport("__Internal")]
    private static extern void LoadExtern();

    [DllImport("__Internal")]
    private static extern void SetLeaderBoardScore(int score);


    Progres progres;

    private void Start()
    {
        ShowLoginQuestion();
    }

    private void Awake()
    {
        progres = FindObjectOfType<Progres>();
    }


    public void RateGameButton()
    {
#if UNITY_WEBGL
        RateGame();
#endif
    }

    public void CheckRateGameButton()
    {
        CheckRateGame();
    }

    public void ActivateRateGameButton(string valueString)
    {
        bool value = JsonUtility.FromJson<bool>(valueString);
        ActivateRateButtonEvent?.Invoke(value);
    }

    public void SetFeedbackValue(string valueString)
    {
        bool value = JsonUtility.FromJson<bool>(valueString);
        ActivateRateButtonEvent?.Invoke(!value);
    }

    public void SetName(string name)
    {
        Logger("SetName called: " + name);
    }

    public void SetPhoto(string url)
    {
        Logger("SetPhoto called");
        StartCoroutine(DownLoadPhoto(url));
    }

    IEnumerator DownLoadPhoto(string mediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
            Logging(request.error);
        }
        else
        {
            //progres.PlayerImage = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }

    public void ShowFullScreenAdvExternal()
    {
#if UNITY_WEBGL
        ShowFullScreenAdv();
#endif
    }

    public void ShowRewardedVideoExternal()
    {
#if UNITY_WEBGL
        ShowRewardedVideo();
#endif
    }

    public void OnRewarded()
    {
        RewardEvent.Invoke();
    }

    public void GetLeaderBoardExtern()
    {
        GetLeaderboard();
    }

    public void ParseLeaderBoard(string source)
    {
        List<Tuple<string, string>> result = new();

        var jsonObject = JsonUtility.FromJson<JObject>(source);
        var entries = jsonObject.GetValue("entries");
        if (entries != null)
        {
            foreach (var entry in entries)
            {
                var entr = (JObject)entry;
                var score = entr.GetValue("score");
                var player = (JObject)entr.GetValue("player");
                var playerName = player.GetValue("publicName");
                result.Add(new Tuple<string, string>(playerName.ToString(), score.ToString()));
            }
        }

        Debug.Log(result);
    }

    public void GamePlayYandex()
    {
        GamePauseEvent?.Invoke(false);
    }

    public void GamePauseYandex()
    {
        GamePauseEvent?.Invoke(true);
    }

    public void LoginYandex()
    {
        Login();
    }
    public void CheckLoginYandex()
    {
        CheckLogin();
    }

    public void Logger(string logString)
    {
        Logging(logString);
    }

    public void GameInstanceLoaded()
    {
        GameInstanceLoadedEvent?.Invoke();
    }

    public void SaveData(string json)
    {
        SaveExtern(json);
    }

    public void SetLeaderBoardScoreData(int value)
    {
        SetLeaderBoardScore(value);
    }

    void ShowLoginQuestion()
    {
        Logger("ShowLoginQuestion called: progres.LoggedIn is " + progres.LoggedIn);
        loginPanel.SetActive(!progres.LoggedIn);
    }

    public void DisableLoginPanel()
    {
        Logger("DisableLoginPanel called");
        progres.LoggedIn = true;
        loginPanel.SetActive(false);
    }
}
