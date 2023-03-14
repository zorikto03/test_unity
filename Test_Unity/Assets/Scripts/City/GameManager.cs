using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject SettingsMenu;
    [SerializeField] GameObject GameOverMenu;
    [SerializeField] GameObject LoginCanvas;
    [SerializeField] GameObject ContinueButton;
    [SerializeField] GameObject RewardedAdvButton;

    PlayerMoving character;
    PlayerBehaviour playerBehaviour;
    CarSound carSound;
    new mainCamera camera;
    Gen_road genRoad;
    BuffTimer timer;
    Progres progres;

    Yandex yandex;
    bool isPause;
    bool questionLogin = false;
    
    private void OnEnable()
    {
        PlayerBehaviour.GameOverEvent += GameOver;
        Yandex.RewardEvent += FreeHeartByWatchingVideo;
        Yandex.LoginEvent += LoginCanvasClose;
    }


    private void OnDisable()
    {
        PlayerBehaviour.GameOverEvent -= GameOver;
        Yandex.RewardEvent -= FreeHeartByWatchingVideo;
        Yandex.LoginEvent -= LoginCanvasClose;
    }

    private void Start()
    {
        playerBehaviour = FindObjectOfType<PlayerBehaviour>();
        progres = FindObjectOfType<Progres>();
        yandex = FindObjectOfType<Yandex>();
        character = FindObjectOfType<PlayerMoving>();
        camera = FindObjectOfType<mainCamera>();
        genRoad = FindObjectOfType<Gen_road>();
        timer = FindObjectOfType<BuffTimer>();
        carSound = FindObjectOfType<CarSound>();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        isPause = !hasFocus;
        PauseTimeSound();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        isPause = pauseStatus;
        PauseTimeSound();
    }

    public void Play()
    {
        isPause = false;
        PauseTimeSound();
        SceneManager.LoadScene("city");
    }

    public void Pause()
    {
        isPause = true;
        PauseTimeSound();

        SettingsMenu.SetActive(true);
    }

    public void Continue()
    {
        isPause = false;
        PauseTimeSound();
        SettingsMenu.SetActive(false);
    }
    void PauseTimeSound()
    {
        Time.timeScale = isPause ? 0f : 1f;
        AudioListener.pause = isPause;
    }
    public void ToMainMenu()
    {
        isPause = false;
        PauseTimeSound();
        SceneManager.LoadScene("Menu");
    }

    public void ToStore()
    {
        isPause = false;
        PauseTimeSound();
        SceneManager.LoadScene("Store");
    }

    public void GameOver()
    {
        Time.timeScale = 0f;

        GameOverMenu.SetActive(true);
        try
        {
            yandex.ShowFullScreenAdvExternal();
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void Exit()
    {
        Debug.Log("Exit pressed");
        Application.Quit();
    }

    public void Restart()
    {
        isPause = false;
        PauseTimeSound();
        character.Restart();
        genRoad.Restart();
        camera.Restart();
        GameOverMenu.SetActive(false);
        timer.StopTimer();
        carSound.Restart();
        playerBehaviour.Restart();
    }

    public void ContinuePlaying()
    {
        isPause = false;
        PauseTimeSound();
        ContinueButton.SetActive(false);
        RewardedAdvButton.SetActive(true);
        playerBehaviour.ContinueByWatchingAdv();
        GameOverMenu.SetActive(false);
        carSound.Restart();
        character.ContinuePlaying();
    }

    public void FreeHeartByWatchingVideo()
    {
        ContinueButton.SetActive(true);
        RewardedAdvButton.SetActive(false);
    }

    public void LoginCanvasClose()
    {
        questionLogin = true;
        LoginCanvas.SetActive(false);
        GameOverMenu.SetActive(true);
    }
}
