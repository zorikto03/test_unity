using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject SettingsMenu;

    void Start()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene("city");
    }

    public void Settings()
    {
        SettingsMenu.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void SettingsButtonBack()
    {
        SettingsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void ToStore()
    {
        SceneManager.LoadScene("Store");
    }

    public void Exit()
    {
        Debug.Log("Exit pressed");
        Application.Quit();
    }
}
