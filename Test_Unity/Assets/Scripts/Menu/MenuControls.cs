using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
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
        SceneManager.LoadScene("Menu");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
