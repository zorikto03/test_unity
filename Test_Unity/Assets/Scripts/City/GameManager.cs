using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject SettingsMenu;
    [SerializeField] GameObject GameOverMenu;
    PlayerMoving character;
    mainCamera camera;
    Gen_road genRoad;

    private void OnEnable()
    {
        PlayerMoving.onChangeRoad += ChangeRoad;
    }

    private void OnDisable()
    {
        PlayerMoving.onChangeRoad -= ChangeRoad;
    }

    private void Start()
    {
        character = FindObjectOfType<PlayerMoving>();
        camera = FindObjectOfType<mainCamera>();
        genRoad = FindObjectOfType<Gen_road>();
    }

    public void Play()
    {
        PlayerMoving.onStartRun?.Invoke();
        SceneManager.LoadScene("city");
    }

    public void Pause()
    {
        PlayerMoving.onStopRun?.Invoke();
        SettingsMenu.SetActive(true);
    }

    public void Continue()
    {
        PlayerMoving.onStartRun?.Invoke();
        SettingsMenu.SetActive(false);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ToStore()
    {
        SceneManager.LoadScene("Store");
    }

    public void GameOver()
    {
        GameOverMenu.SetActive(true);
    }

    public void Exit()
    {
        Debug.Log("Exit pressed");
        Application.Quit();
    }

    public void Restart()
    {
        character.Restart();
        genRoad.Restart();
        camera.Restart();
        GameOverMenu.SetActive(false);
    }

    void ChangeRoad()
    {
        genRoad.CurrentType++;
        Debug.Log($"Change road {genRoad.CurrentType}");
    }
}
