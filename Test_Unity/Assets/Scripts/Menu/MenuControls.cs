using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    public static MenuControls Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayPressed()
    {
        SceneManager.LoadScene("city");
    }


    public void ExitPressed()
    {
        Debug.Log("Exit pressed");
        Application.Quit();
    }

    public void PausePressed()
    {
        //SceneManager.LoadScene("Menu");
        
        TouchEvent.Instance.isPaused = !TouchEvent.Instance.isPaused;
    }

    public void ContinuePressed()
    {
        TouchEvent.Instance.isPaused = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        Instance = this;
    }
}
