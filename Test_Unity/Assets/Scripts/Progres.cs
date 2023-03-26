using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Progres : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PlayerDataUI;

    public PlayerInfo PlayerInfo;

    static Progres _instance;
    public static Progres Instance => _instance;
    
    Yandex yandex;
    bool _musicIsOn;
    float _musicSlider;
    bool _isLoggedIn;

    public bool MusicToggle
    {
        get => _musicIsOn;
        set => _musicIsOn = value; 
    }
    public float MusicSlider
    {
        get => _musicSlider;
        set => _musicSlider = value;
    }
    public bool LoggedIn 
    {
        get => _isLoggedIn;
        set => _isLoggedIn = value;
    }


    private void Start()
    {
        if (PlayerInfo == null)
            PlayerInfo = new PlayerInfo();

        if (!_isLoggedIn)
        {
            SetDataLocal();
        }
        ShowPlayerInfo();
    }


    private void Awake()
    {
        if (_instance == null)
        {
            transform.parent = null;
            _instance = this;
        }
        DontDestroyOnLoad(this);

        yandex = FindObjectOfType<Yandex>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneWasLoaded;
        Yandex.GameInstanceLoadedEvent += GameInstanceLoadedEventHandler;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneWasLoaded;
        Yandex.GameInstanceLoadedEvent -= GameInstanceLoadedEventHandler;
    }

    private void SceneWasLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.buildIndex == 0)
        {
            if (!_isLoggedIn)
            {
                SetDataLocal();
            }
            ShowPlayerInfo();
        }
    }


    public void Save()
    {
#if UNITY_WEBGL
        if (_isLoggedIn)
        {
            var json = JsonUtility.ToJson(PlayerInfo);
            yandex.SaveData(json);
            var score = Mathf.CeilToInt(PlayerInfo.MaxScore * 100);
            yandex.SetLeaderBoardScoreData(score);
        }
#endif
    }

    public void SetPlayerInfo(string data)
    {
        yandex.Logger("SetPlayerInfo called");

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
            PlayerInfo.SetData(load.RecordDistance, load.MaxDangerousManeuvers, load.MaxScore);
        }
    }

    public void ShowPlayerInfo()
    {
        PlayerDataUI.text = Language.Instance.Lang switch
        {
            "en" => $"Max Distance: {PlayerInfo.RecordDistance}\nMax Dangerous Maneuvers: {PlayerInfo.MaxDangerousManeuvers}\nMax Score: {PlayerInfo.MaxScore}",
            "ru" => $"Дистанция: {PlayerInfo.RecordDistance}\nКол-во опасных маневров: {PlayerInfo.MaxDangerousManeuvers}\nОчки: {PlayerInfo.MaxScore}",
            _ => $"Max Distance: {PlayerInfo.RecordDistance}\nMax Dangerous Maneuvers: {PlayerInfo.MaxDangerousManeuvers}\nMax Score: {PlayerInfo.MaxScore}"
        };
    }

    void GameInstanceLoadedEventHandler()
    {
         yandex.CheckLoginYandex();
    }

}
