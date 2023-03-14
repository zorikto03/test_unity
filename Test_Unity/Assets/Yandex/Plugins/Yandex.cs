using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Yandex : MonoBehaviour
{
    [SerializeField] PlayerDataFromYandex playerDataYandex;
    [SerializeField] GameObject rateGameButton;

    [DllImport("__Internal")]
    private static extern void Hello();

    [DllImport("__Internal")]
    private static extern void GetPlayerData();

    [DllImport("__Internal")]
    private static extern void RateGame();

    [DllImport("__Internal")]
    private static extern void ShowFullScreenAdv();

    [DllImport("__Internal")]
    private static extern void ShowRewardedVideo();

    [DllImport("__Internal")]
    private static extern void GetLeaderboard();

    [DllImport("__Internal")]
    private static extern void Login();

    [DllImport("__Internal")]
    private static extern void Logging(string text);

    string _playerName = string.Empty;
    Texture2D _playerImage = null;

    public static Action RewardEvent;
    public static Action LoginEvent;
    public string PlayerName => _playerName;

    void Awake()
    {
        try
        {
#if UNITY_WEBGL
            //GetPlayerData();
#endif
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void LoginButton()
    {
#if UNITY_WEBGL
        Login();
#endif
    }

    public void LoginResult(bool result)
    {
        LoginEvent?.Invoke();
    }

    public void RateGameButton()
    {
#if UNITY_WEBGL
        RateGame();
#endif
    }

    public void SetFeedbackValue(bool value)
    {
        if (value)
        {
            rateGameButton.SetActive(false);
        }
    }

    public void SetName(string name)
    {
        _playerName = name;
        playerDataYandex.SetName(_playerName);
    }

    public void SetPhoto(string url)
    {
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
            _playerImage = ((DownloadHandlerTexture)request.downloadHandler).texture;
            playerDataYandex.SetPhoto(_playerImage);
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
            foreach(var entry in entries)
            {
                var entr = (JObject)entry;
                var score = entr.GetValue("score");
                var player = (JObject)entr.GetValue("player");
                var playerName = player.GetValue("publicName");
                result.Add(new Tuple<string, string>(playerName.ToString(), score.ToString()));
            }
        }

        Debug.Log(result);
        playerDataYandex.SetLeaderBoard(result);
    }
}
